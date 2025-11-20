using Cart.Application.Features.Command.Cart.AddItem;
using Cart.Application.Interface;
using Cart.Domain.IRepository;
using Cart.Infrastructure.Redis;
using Cart.Infrastructure.Repository;
using Cart.Infrastructure.Services;
using Catalog.API.Configuration;
using MassTransit;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICartRespository, RedisCartRepository>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(AddToCartCommand).Assembly);
});
builder.Services.AddSingleton(new RedisConnectionFactory(builder.Configuration.GetConnectionString("Redis")!));

builder.Services.AddScoped<ICartRespository, RedisCartRepository>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MapperConfig>();
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b => b.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("rabbitMq", "/", h =>
    {
        h.Username("guest");
        h.Password("guest");

    });

    });
});
builder.Services.AddHttpClient<ICatalogClient, CatalogClient>(c =>
{
    var url = builder.Configuration.GetConnectionString("CatalogServiceUrl");
    c.BaseAddress = new Uri(url)?? throw new Exception("Catalog url is missing");
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
