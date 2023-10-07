using Microsoft.EntityFrameworkCore;
using TestAPI.Context;
using AutoMapper;
using TestAPI.Interfaces;
using TestAPI.Repositories;
using Microsoft.OpenApi.Models;
using TestAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwagger();
builder.Services.AddControllers();
builder.Services.AddJWTAuthentication(configuration);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();