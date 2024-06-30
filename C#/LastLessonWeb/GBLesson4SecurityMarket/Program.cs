
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using GBLesson4SecurityMarket.Abstraction;
using GBLesson4SecurityMarket.Context;
using GBLesson4SecurityMarket.Repository;
using GBLesson4SecurityMarket.Security;
using GBLesson4SecurityMarket.Service;
using System;
using System.Collections.Generic;

namespace GBLesson4SecurityMarket
{
    // Создать каталог RSA
    // openssl genpkey -algorithm RSA -out private_key.pem  -- получаем private_key.pem в RSA
    // openssl rsa -pubout -in private_key.pem -out public_key.pem  -- получаем public_key.pem в RSA
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(
                options =>
                {
                    options.AddSecurityDefinition(
                        JwtBearerDefaults.AuthenticationScheme,
                        new()
                        {
                            In = ParameterLocation.Header,
                            Description = "Please insert jwt with Bearer into filed",
                            Name = "Authorization",
                            Type = SecuritySchemeType.Http,
                            BearerFormat = "Jwt Token",
                            Scheme = JwtBearerDefaults.AuthenticationScheme
                        });
                    options.AddSecurityRequirement(
                        new()
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = JwtBearerDefaults.AuthenticationScheme
                                    }
                                },
                                new List<string>()
                            }

                        });
                });

            builder.Services.AddDbContext<UserRoleContext>(option =>
                option.UseNpgsql(builder.Configuration.GetConnectionString("db")));

            var jwt = builder.Configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>()
                      ?? throw new Exception("JwtConfiguration not found");
            builder.Services.AddSingleton(provider => jwt);
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwt.Issuer,
                        ValidAudience = jwt.Audience,
                        IssuerSigningKey = jwt.GetSigningKey()
                    };
                });

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITokenService, TokenService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
