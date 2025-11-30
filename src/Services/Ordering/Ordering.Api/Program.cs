
using Microsoft.EntityFrameworkCore;
using Ordering.API.Consumers;
using Ordering.Application.Features.Order.Command;

using Ordering.Domain.Interface;
using Ordering.Infrastructure.Common;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repository;
using Serilog;
var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b => b.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});


Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Service", "Ordering.API") // ⭐ identifies the service
    .WriteTo.Console()
    .WriteTo.Seq("http://seq:5431")
    .CreateLogger();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly);


  

});
Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .CreateLogger();

builder.Host.UseSerilog();
//register dispatcher so db can be constructed
builder.Services.AddScoped<IOrderingRepository, OrderingRepository>();
builder.Services.AddScoped<DomainEventDispatcher>();

builder.Services.AddDbContext<OrderingDbContext>(options =>
{
    var connStr = builder.Configuration.GetConnectionString("OrderingDbConnection");
    options.UseNpgsql(connStr, (b) =>
    {
        b.MigrationsAssembly("Ordering.Infrastructure");
        b.EnableRetryOnFailure(
            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(5),
        errorCodesToAdd: null);
    });
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumers(typeof(CartCheckedOutConsumer).Assembly);
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        //cfg.ReceiveEndpoint("cart-checkedout-event", e =>
        //{
        //    e.ConfigureConsumer<CartCheckedOutConsumer>(ctx);
        //});
        cfg.ReceiveEndpoint("cart-checkedout-event", e =>
        {
            e.ConfigureConsumers(ctx);
        });

    }
    );

});
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors("AllowAll");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
    Console.WriteLine(context.Database.GetConnectionString());
}

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
//    db.Database.Migrate();   // <--- Create the tables
//    Console.WriteLine("Database migrated successfully");
//}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
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
//if (app.Environment.IsDevelopment())
//{
//    }
app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
