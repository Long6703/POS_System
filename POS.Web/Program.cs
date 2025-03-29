using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using POS.Web.Middlewares;
using POS.Web.Services;
using POS.Web.Services.IService;

namespace POS.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAntDesign();
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddHttpClient<BaseApiService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:6703/");
            });
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseStaticFiles();
            app.UseSession();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}
