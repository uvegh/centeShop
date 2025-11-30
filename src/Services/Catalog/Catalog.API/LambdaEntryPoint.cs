//using Amazon.Lambda.AspNetCoreServer;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Catalog.Infrastructure.Data;
//using Microsoft.EntityFrameworkCore;
//using Catalog.API.Configuration;
//using Catalog.Application.Features.Command;
//using Catalog.Domain.Interfaces;
//using Catalog.Infrastructure.Repositories;

//namespace Catalog.API;

//public class LambdaEntryPoint : APIGatewayHttpApiV2ProxyFunction
//{
//    protected override void Init(IWebHostBuilder builder)
//    {
//        builder
//            .UseContentRoot(Directory.GetCurrentDirectory())
//            .UseStartup<Startup>();
//    }
//}

//public class Startup
//{
//    public IConfiguration Configuration { get; }

//    public Startup(IConfiguration configuration)
//    {
//        Configuration = configuration;
//    }

//    public void ConfigureServices(IServiceCollection services)
//    {
//        // CORS 
//        services.AddCors(options =>
//        {
//            options.AddPolicy("AllowAll", b => b.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
//        });

//        services.AddControllers();
//        services.AddEndpointsApiExplorer();
//        services.AddSwaggerGen();

//        // AutoMapper with MapperConfig 
//        services.AddAutoMapper(cfg => cfg.AddProfile<MapperConfig>());

//        // Repository 
//        services.AddScoped<IProductRepository, ProductRepository>();

//        // MediatR with CreateProductCommand.Assembly 
//        services.AddMediatR(cfg =>
//        {
//            cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly);
//        });

//        // Database configuraiton
//        var connectionStr = Configuration.GetConnectionString("DefaultConnection");
//        services.AddDbContext<CatalogDbContext>(options =>
//        {
//            options.UseNpgsql(connectionStr, b =>
//            {
//                b.MigrationsAssembly("Catalog.Infrastructure");
//                b.EnableRetryOnFailure(
//                    maxRetryCount: 10,
//                    maxRetryDelay: TimeSpan.FromSeconds(5),
//                    errorCodesToAdd: null);
//            });
//        });
//    }

//    //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//    //{
//    //    // Database migration - EXACTLY from your Program.cs
//    //    using (var scope = app.ApplicationServices.CreateScope())
//    //    {
//    //        var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
//    //        Console.WriteLine($"Connected to database: {context.Database.GetType()}");
//    //    }

//    //    // Auto migration - EXACTLY from your Program.cs
//    //    using (var scope = app.ApplicationServices.CreateScope())
//    //    {
//    //        var db = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
//    //        try
//    //        {
//    //            db.Database.Migrate();
//    //            Console.WriteLine("Database migrated successfully");
//    //        }
//    //        catch (Exception ex)
//    //        {
//    //            Console.WriteLine($"Warning: Database migration failed: {ex.Message}");
//    //            Console.WriteLine("App will start anyway - check database connection");
//    //        }
//    //    }

//    //    app.UseCors("AllowAll");
//    //    app.UseSwagger();
//    //    app.UseSwaggerUI();
//    //    app.UseRouting();
//    //    app.UseAuthorization();
//    //    app.UseEndpoints(endpoints =>
//    //    {
//    //        endpoints.MapControllers();
//    //    });
//    //}

//    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//    {
//        // Database creation and migration
//        using (var scope = app.ApplicationServices.CreateScope())
//        {
//            var db = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
//            try
//            {
//                // EnsureCreated creates the database if it doesn't exist
//                db.Database.EnsureCreated();
//                Console.WriteLine("Database created successfully");

//                // Then run migrations
//                db.Database.Migrate();
//                Console.WriteLine("Database migrated successfully");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Warning: Database setup failed: {ex.Message}");
//            }
//        }

//        app.UseCors("AllowAll");
//        app.UseSwagger();
//        app.UseSwaggerUI();
//        app.UseRouting();
//        app.UseAuthorization();
//        app.UseEndpoints(endpoints =>
//        {
//            endpoints.MapControllers();
//        });
//    }
//}
using Amazon.Lambda.AspNetCoreServer;

namespace Ordering.API;

public class LambdaEntryPoint : APIGatewayHttpApiV2ProxyFunction
{
    protected override void Init(IWebHostBuilder builder)
    {
        builder.UseStartup<Program>();
    }
}
