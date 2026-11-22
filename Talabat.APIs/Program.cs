
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Interface;
using Talabat.Repository;
using Talabat.Repository.Identity;
using Talabat.Repository.Identity.DataSeed;
using Talabat.Repository.Implementations;
using Talabat.Repository.Interfaces;
using Talabat.Service;
using StackExchange.Redis;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });

            //Redis
            builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connectionString = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connectionString);
            });

            //Identity DbContext
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            // Add services to the container.

            builder.Services.AddControllers();
            //builder.Services.AddScoped<IBasketRepository,BasketRepository>() ;
            //builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
            }).AddEntityFrameworkStores<AppIdentityDbContext>();

            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                           .AddJwtBearer(options => {
                               options.TokenValidationParameters = new TokenValidationParameters() {
                                   ValidateIssuer = true,
                                   ValidIssuer = builder.Configuration["Jwt:Issuer"],
                                   ValidateAudience = true,
                                   ValidAudience = builder.Configuration["Jwt:Audience"],
                                   ValidateLifetime = true,
                                   ValidateIssuerSigningKey = true,
                                   IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                               };


                           });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddOpenApi();
            // Depandency Injection for Generic Repository
           builder.Services.ApplicationServices();

            var app = builder.Build();

            #region Update DataBase 

            var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var _context = services.GetRequiredService<AppDbContext>();
            var _IdentityDBContext = services.GetRequiredService<AppIdentityDbContext>();


            var LoggerFactory = services.GetRequiredService<ILoggerFactory>();


            try
            {
                await _context.Database.MigrateAsync(); // Update DataBase (Business )
                //Data Seeding
                await AppDbContextSeed.SeedAsync(_context);
                await _IdentityDBContext.Database.MigrateAsync(); // Update DataBase (Identity)
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUSerAsync(userManager);
            }
            catch (Exception ex)
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, message: "an Error has been occured during Appling The Migrations ");

                //Console.WriteLine(ex.Message);
            }

            #endregion
            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseStatusCodePagesWithReExecute("/errors/{0}"); // Handle Status Code Pages
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
