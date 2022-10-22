using MyOrders.Application;
using MyOrders.Core;
using MyOrders.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCore();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseInfrastructure();
app.UseAuthorization();

app.MapControllers();

app.Run();
