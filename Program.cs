using System.Text;
using System.Text.Json.Serialization;
using Api.Repositories;
using Api.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();




// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
// {
// 	options.TokenValidationParameters = new TokenValidationParameters()
// 	{

// 		ValidateAudience = true,
// 		ValidateLifetime = true,
// 		ValidateIssuerSigningKey = true,
// 		ValidIssuer = builder.Configuration["JWT:Issuer"],
// 		ValidAudience = builder.Configuration["JWT:Audience"],
// 		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
// 	};
// });

// builder.Services.AddAuthorization();



// DI
{
	builder.Services.AddSingleton<IProductsRepository, InMemProductsRepository>();
	builder.Services.AddSingleton<IUsersRepository, InMemUsersRepository>();
}


var app = builder.Build();

app.UseExceptionHandler("/error");


// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {

// }

// app.UseAuthentication();
// app.UseAuthorization();

// app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseMiddleware<JwtMiddleware>();


app.MapControllers();

app.Run();
