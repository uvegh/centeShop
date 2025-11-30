using Amazon.Lambda.AspNetCoreServer;
using Cart.Application.Features.Command.Cart;
using Cart.Application.Features.Command.Cart.AddItem;
using Cart.Application.Interface;
using Cart.Application.Services;
using Cart.Domain.IRepository;
using Cart.Infrastructure.Redis;
using Cart.Infrastructure.Repository;
using Catalog.API.Configuration;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cart.API;

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

        // AutoMapper
        services.AddAutoMapper(cfg => cfg.AddProfile<MapperConfig>());

        // Repository
        services.AddScoped<ICartRespository, RedisCartRepository>();

        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(AddToCartCommand).Assembly);
        });

        // Singleton for Redis connection - EXACTLY from your Program.cs
        services.AddSingleton(new RedisConnectionFactory(
            Configuration.GetConnectionString("Redis")!));

        // HttpClient for Catalog API
        services.AddHttpClient<ICatalogClient, CatalogClient>(client =>
        {
            var catalogUrl = Configuration.GetConnectionString("CatalogServiceUrl")
                             ?? Configuration["CatalogServiceUrl"]
                             ?? "https://8b27mm9cc5.execute-api.eu-west-2.amazonaws.com";

            Console.WriteLine($"Cart API using Catalog URL: {catalogUrl}");
            client.BaseAddress = new Uri(catalogUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        // MassTransit for publishing events to Amazon MQ
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((ctx, cfg) =>
            {
                //var rabbitMqHost = Configuration["RabbitMq__Host"] ?? "localhost";
                //var rabbitMqUser = Configuration["RabbitMq__Username"] ?? "guest";
                //var rabbitMqPass = Configuration["RabbitMq__Password"] ?? "guest";

                //Console.WriteLine($"Connecting to RabbitMQ: {rabbitMqHost}");

                //cfg.Host(rabbitMqHost, "/", h =>
                //{
                //    h.Username(rabbitMqUser);
                //    h.Password(rabbitMqPass);
                //});

                Console.WriteLine("MQ USERNAME = " + Environment.GetEnvironmentVariable("MQ_USERNAME"));
                Console.WriteLine("MQ PASSWORD = " + Environment.GetEnvironmentVariable("MQ_PASSWORD"));


                cfg.Host(
   "b-f89950cb-d7a2-4384-a5e5-4373e86ff412.mq.eu-west-2.on.aws",
   5671,
   "/",
   h =>
   {
       h.Username(Environment.GetEnvironmentVariable("MQ_USERNAME"));
       h.Password(Environment.GetEnvironmentVariable("MQ_PASSWORD"));
       h.UseSsl();   // VERY IMPORTANT
   });
            });


        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
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