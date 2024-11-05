using CoreAPI2024;
using Microsoft.EntityFrameworkCore;
using WebAPI_for_frondEnd.Models;
using WebAPI_for_frondEnd.Service;
var builder = WebApplication.CreateBuilder(args);

// 允許來自特定來源的 CORS 請求
builder.Services.AddCors(options => {
    options.AddPolicy("AllowSpecificOrigin",
        builder => {
            builder.AllowAnyOrigin()  // 前端的URL
                   .AllowAnyHeader()
                   .AllowAnyMethod();  // 允許所有HTTP動詞 (GET, POST, etc.)
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

// 注入 IEmailService
builder.Services.AddTransient<IEmailService, EmailService>(); // 注入 IEmailService

builder.Services.AddScoped<UserService>(); //註冊 userservice

builder.Services.AddSingleton<UserService>(); // 註冊 UserService

builder.Services.AddMemoryCache(); // 註冊 MemoryCache 服務

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 啟用 CORS
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
