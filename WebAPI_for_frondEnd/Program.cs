using CoreAPI2024;
using Microsoft.EntityFrameworkCore;
using WebAPI_for_frondEnd.Models;
using WebAPI_for_frondEnd.Service;
var builder = WebApplication.CreateBuilder(args);

// ���\�ӦۯS�w�ӷ��� CORS �ШD
builder.Services.AddCors(options => {
    options.AddPolicy("AllowSpecificOrigin",
        builder => {
            builder.AllowAnyOrigin()  // �e�ݪ�URL
                   .AllowAnyHeader()
                   .AllowAnyMethod();  // ���\�Ҧ�HTTP�ʵ� (GET, POST, etc.)
        });
});

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

// �`�J IEmailService
builder.Services.AddTransient<IEmailService, EmailService>(); // �`�J IEmailService

builder.Services.AddScoped<UserService>(); //���U userservice

builder.Services.AddSingleton<UserService>(); // ���U UserService

builder.Services.AddMemoryCache(); // ���U MemoryCache �A��

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

// �ҥ� CORS
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
