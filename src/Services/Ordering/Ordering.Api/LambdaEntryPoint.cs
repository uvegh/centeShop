using Amazon.Lambda.AspNetCoreServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Ordering.Infrastructure.Data;
using Ordering.Domain.Interface;
using Ordering.Infrastructure.Repository;
using Ordering.Infrastructure.Common;
using Ordering.Application.Features.Order.Command;
using MassTransit;
using Ordering.API.Consumers;
using Serilog;

namespace Ordering.API;

public class LambdaEntryPoint : APIGatewayHttpApiV2ProxyFunction
{
    protected override void Init(IWebHostBuilder builder)
    {
        builder
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>();
    }
}

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", b =>
                b.AllowAnyHeader()
                 .AllowAnyOrigin()
                 .AllowAnyMethod());
        });

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly);
        });

        // Repository and DomainEventDispatcher
        services.AddScoped<IOrderingRepository, OrderingRepository>();
        services.AddScoped<DomainEventDispatcher>();

        // Database
        var connStr = Configuration.GetConnectionString("OrderingDbConnection");
        services.AddDbContext<OrderingDbContext>(options =>
        {
            options.UseNpgsql(connStr, b =>
            {
                b.MigrationsAssembly("Ordering.Infrastructure");
                b.EnableRetryOnFailure(
                    maxRetryCount: 10,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null);
            });
        });

        // MassTransit for consuming events from Amazon MQ
        services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(CartCheckedOutConsumer).Assembly);
            x.UsingRabbitMq((ctx, cfg) =>
            {
                var rabbitMqHost = Configuration["RabbitMq__Host"] ?? "localhost";
                var rabbitMqUser = Configuration["RabbitMq__Username"] ?? "guest";
                var rabbitMqPass = Configuration["RabbitMq__Password"] ?? "guest";

                Console.WriteLine($"Connecting to RabbitMQ: {rabbitMqHost}");

                cfg.Host(rabbitMqHost, "/", h =>
                {
                    h.Username(rabbitMqUser);
                    h.Password(rabbitMqPass);
                });

                cfg.ReceiveEndpoint("cart-checkedout-event", e =>
                {
                    e.ConfigureConsumers(ctx);
                });
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Database check
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
            Console.WriteLine($"Connection string: {context.Database.GetConnectionString()}");
        }

        // Database creation and migration
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
            try
            {
                db.Database.EnsureCreated();
                Console.WriteLine("Ordering database created successfully");

                db.Database.Migrate();
                Console.WriteLine("Ordering database migrated successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Database setup failed: {ex.Message}");
            }
        }

        app.UseCors("AllowAll");
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}