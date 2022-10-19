using MyOrders.Application;
using MyOrders.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseInfrastructure();
app.UseAuthorization();

app.MapControllers();

app.Run();
