
//using Catalog.Infrastructure;
using Catalog.API.Configuration;
using Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MapperConfig>());
builder.Services.AddDbContext<CatalogDbContext>(options =>
{
    var connecitonStr = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connecitonStr);
});
// Configure the HTTP request pipeline.
var app = builder.Build();

//ensure db exists and apply model
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
    Console.WriteLine("Connected to databse",context.Database.GetType());
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
