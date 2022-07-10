using System.Text.Json.Serialization;
using Api.Repositories;
using Api.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();



// DI
{
	builder.Services.AddSingleton<IProductsRepository, InMemProductsRepository>();
	builder.Services.AddSingleton<IUsersRepository, InMemUsersRepository>();
}


var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {

// }

// app.UseAuthorization();

app.UseMiddleware<ExceptionHandlerMiddleware>();


app.MapControllers();

app.Run();
