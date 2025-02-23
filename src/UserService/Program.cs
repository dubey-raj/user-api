using Amazon.SQS;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Serilog.Formatting.Json;
using Serilog;
using UserService.Configurations;
using UserService.DataStorage.DAL;
using UserService.Services;
using UserService.Middlewares;

var builder = WebApplication.CreateBuilder(args);

ConfigureLogging(builder);
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
AddSwaggerGen(builder);
builder.Services.AddUsersDBContext(builder.Configuration);
builder.Services.AddScoped<IUserService, GlamUserService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddAWSService<IAmazonSQS>();
builder.Services.AddScoped<IEventPublisher, EventPublisher>();

var app = builder.Build();

var env = builder.Environment.EnvironmentName?.ToLower();
builder.Configuration.AddJsonFile("appsettings.json", false, true);
if (env == "local")
{
    builder.Configuration.AddJsonFile($"appsettings.local.json", false, true);
}
else
{
    builder.Configuration.AddJsonFile($"appsettings.dev.json", false, true);
}
// Configure the HTTP request pipeline.
app.UseCors(policy => policy
.AllowAnyMethod()
.AllowAnyHeader()
.AllowAnyOrigin()
);
var basePath = "/user-api";
// Ensure correct path handling in ALB
app.UsePathBase(new PathString(basePath));
app.UseSerilogRequestLogging();
app.UseMiddleware<GlobalExceptionHandler>();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "swagger";
    c.SwaggerEndpoint($"{basePath}/swagger/v1/swagger.json", "UserService V1");
});
app.UseRouting();
app.MapControllers();
app.UseHttpsRedirection();
app.Run();

static void ConfigureLogging(WebApplicationBuilder builder)
{
    //Configure serilog as logger
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console(new JsonFormatter(renderMessage: true))
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
    builder.Host.UseSerilog();
}

static void AddSwaggerGen(WebApplicationBuilder builder)
{
    builder.Services.AddSwaggerGen(c =>
    {
        //Add JWT Bearer Authentication to Swagger UI
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter 'Bearer <your-token>' below. Example: Bearer eyJhbGciOi..."
        });

        //Apply Authentication Globally
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    });
}
