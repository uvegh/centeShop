using Cart.Application.Features.Command.Cart.AddItem;
using Cart.Application.Interface;
using Cart.Application.Services;
using Cart.Domain.IRepository;
using Cart.Infrastructure.Redis;
using Cart.Infrastructure.Repository;
using Catalog.API.Configuration;
using MassTransit;
using Serilog;
var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b => b.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});


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

Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h =>
    {
        h.Username("guest");
        h.Password("guest");

    });

    });
});




// Add HttpClient for Catalog API
builder.Services.AddHttpClient<ICatalogClient, CatalogClient>(client =>
{
    var catalogUrl = builder.Configuration.GetConnectionString("CatalogServiceUrl")
                     ?? "http://catalog-api:80";//fallback url
    client.BaseAddress = new Uri(catalogUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//}
app.UseCors("AllowAll");
app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
