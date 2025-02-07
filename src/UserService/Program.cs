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

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(policy => policy
.AllowAnyMethod()
.AllowAnyHeader()
.AllowAnyOrigin()
);
app.UseSwagger();
var basePath = string.IsNullOrEmpty(app.Configuration["BasePath"]) ? "": "/"+app.Configuration["BasePath"];
var routePrefix = string.IsNullOrEmpty(app.Configuration["BasePath"]) ? "swagger" : app.Configuration["BasePath"]+"/"+"swagger";
// Ensure correct path handling in ALB
app.UsePathBase(new PathString(basePath));
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = routePrefix;
    c.SwaggerEndpoint($"{basePath}/swagger/v1/swagger.json", "UserService V1");
});
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
