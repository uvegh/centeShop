using Microsoft.AspNetCore.Authentication.JwtBearer;
using Polly;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console().WriteTo.File("logs/gateway-.log", rollingInterval: RollingInterval.Day).CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();
    // Add services to the container.

    //Cors
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy
            =>
        {
            policy.AllowAnyHeader().AllowAnyMethod()
.AllowAnyOrigin();
        });

    });
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.Audience = builder.Configuration["Keycloak:Audience"];
        options.RequireHttpsMetadata = bool.Parse(builder.Configuration["keycloak:RequireHttpsMetadata"] ?? "false");
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = (context) =>
            {
                Log.Warning("Authentication failed {Exception}", context.Exception);
                return Task.CompletedTask;

            }
        };

    });
    //auth policies
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("authenticated", policy =>
        {
            policy.RequireAuthenticatedUser();

        });

        options.AddPolicy("admin", policy =>
        {
            policy.RequireRole("admin");

        });

        options.AddPolicy("customer", policy =>
        {
            policy.RequireRole("customer");
        });

    });
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



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

}
catch (Exception)
{
    Log.Error(" Fatal, Api Gateway failed to start");
}

