using MassTransit;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Features.Order.Command;
using Ordering.Domain.Events;
using Ordering.Infrastructure.Common;
using Ordering.Infrastructure.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommandHandler).Assembly);
});

//register dispatcher so db can be constructed
builder.Services.AddScoped<DomainEventDispatcher>();
builder.Services.AddDbContext<OrderingDbContext>(options =>
{
    var connStr = builder.Configuration.GetConnectionString("OrderingDbConnection");
    options.UseNpgsql(connStr);
});

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("rabbitMq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });




    }
    );

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
