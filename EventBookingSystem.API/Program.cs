
using AutoMapper;
using EventBookingSystem.Application.Common.Interfaces;
using EventBookingSystem.Application.Contract;
using EventBookingSystem.Application.Services.Implementation;
using EventBookingSystem.Application.Services.Interface;
using EventBookingSystem.Domain.Entities;
using EventBookingSystem.infrastructure.Data;
using EventBookingSystem.Infrastructure.Repository;
using EventBookingSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


namespace EventBookingSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            builder.Services.AddDbContext<ApplicationDBContext>(option =>
            option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<ITranslationService,TranslationService>();
            builder.Services.AddHttpClient<ITranslationService, TranslationService>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDBContext>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IEventImageService, EventImageService>(provider =>
            {
                var repo = provider.GetRequiredService<IUnitOfWork>();
                var env = provider.GetRequiredService<IWebHostEnvironment>();
                var map = provider.GetRequiredService<IMapper>();
                return new EventImageService(repo, env.WebRootPath,map);
            });
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IEventService, EventService>(provider=>
            {
                var env = provider.GetRequiredService<IWebHostEnvironment>();
                var repo = provider.GetRequiredService<IUnitOfWork>();
                var map = provider.GetRequiredService<IMapper>();
                return new EventService(env.WebRootPath,repo,map);
            }
                );
            builder.Services.AddAutoMapper(typeof(MappingConfig));
            var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");
            // In Program.cs or Startup.cs
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            

            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.RequireHttpsMetadata = false;
                option.SaveToken = true;
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    NameClaimType = "name"
                };
            });
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Event Booking System API",
                    Version = "v1"
                });

                // Optional: Add JWT bearer token support
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer into field. Example: Bearer eyJhbGciOiJIUzI1...",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new string[] {}
                    }
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Booking System API V1");
                    c.RoutePrefix = "swagger"; // Serve Swagger UI at the root: https://localhost:5001/
                });
            }
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllers();

            app.Run();
        }
    }
}
