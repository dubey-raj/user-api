using Amazon.SQS;
using Microsoft.AspNetCore.Identity;
using UserService.Configurations;
using UserService.DataStorage.DAL;
using UserService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddUsersDBContext(builder.Configuration);
builder.Services.AddScoped<IUserService, GlamUserService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddAWSService<IAmazonSQS>();
builder.Services.AddScoped<IEventPublisher, EventPublisher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(policy => policy
.AllowAnyMethod()
.AllowAnyHeader()
.AllowAnyOrigin()
);
var basePath = "/user-api";
// Ensure correct path handling in ALB
app.UsePathBase(new PathString(basePath));
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
