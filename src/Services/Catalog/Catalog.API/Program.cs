
//using Catalog.Infrastructure;
using Catalog.API.Configuration;
using Catalog.Application.Features.Command;
using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b => b.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MapperConfig>());
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly);
});
var connecitonStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CatalogDbContext>(options =>
{

    options.UseNpgsql((connecitonStr), b =>{
        b.MigrationsAssembly("Catalog.Infrastructure");
        b.EnableRetryOnFailure(

            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorCodesToAdd: null);
    }    );

});


// Configure the HTTP request pipeline.
var app = builder.Build();

//ensure db exists and apply model
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
    Console.WriteLine("Connected to database", context.Database.GetType());
}

//if (app.Environment.IsDevelopment())
//{ }


//  auto migration 
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
    try
    {
        db.Database.Migrate();
        Console.WriteLine("Database migrated successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Warning: Database migration failed: {ex.Message}");
        Console.WriteLine("App will start anyway - check database connection");
        // Don't crash - let Swagger still work
    }
}

app.UseCors("AllowAll");
app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
