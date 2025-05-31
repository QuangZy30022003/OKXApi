    using Microsoft.EntityFrameworkCore;
    using Repositories.Models;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;
    using Repositories;
    using Services;
using Services.Interfaces;
using Services.Service;
//using Repositories.Models;

var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    // Đọc chuỗi kết nối từ appsettings.json
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Thêm cấu hình JWT Authentication
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        }); 

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services
                 .AddRepository()
                 .AddServices();

builder.Services.AddHttpClient<IOKXService, OKXService>();

var app = builder.Build();

    // Pipeline cấu hình
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    // **Bắt buộc phải có**
    app.UseAuthentication(); 
    app.UseAuthorization();

    app.MapControllers();

    app.Run();