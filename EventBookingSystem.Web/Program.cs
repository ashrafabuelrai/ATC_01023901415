using EventBookingSystem.Web.Services.IServices;
using EventBookingSystem.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.Options;
using AutoMapper;
using EventBookingSystem.Application.Contract;
using EventBookingSystem.Infrastructure.Services;

namespace EventBookingSystem.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
                
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IEventImageService, EventImageService>();
            builder.Services.AddScoped<ITranslationService, TranslationService>();
            builder.Services.AddAutoMapper(typeof(MappingConfig));
            /*builder.Services.AddHttpClient<IEventService, EventService>();
            builder.Services.AddHttpClient<ICategoryService, CategoryService>();
            builder.Services.AddHttpClient<IBookingService, BookingService>();*/
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(option =>
               {
                   option.Cookie.HttpOnly = true;
                   option.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                   option.LoginPath = "/Auth/Login";
                   option.AccessDeniedPath = "/Auth/AccessDenied";
                   option.SlidingExpiration = true;
               });
            builder.Services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromMinutes(100);
                option.Cookie.HttpOnly = true;
                option.Cookie.IsEssential = true;
            });
            
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            builder.Services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("ar-EG")
                };
                options.DefaultRequestCulture = new RequestCulture(culture:"ar-EG",uiCulture:"ar-EG");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            var locoptionOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locoptionOptions.Value);

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}

