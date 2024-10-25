using CoreAPI2024;
using Microsoft.EntityFrameworkCore;
using WebAPI_for_frondEnd.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<charityContext>(
    options => options
    .UseLazyLoadingProxies()
    .UseSqlServer(builder.Configuration.GetConnectionString("Charity"))
    );
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<UserService>(); //µù¥U userservice

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
