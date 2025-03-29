using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using POS.Web.Middlewares;
using POS.Web.Services;

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
            builder.Services.AddServerSideBlazor()
            .AddCircuitOptions(options =>
            {
                options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
                options.JSInteropDefaultCallTimeout = TimeSpan.FromSeconds(30);
            });
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHttpClient<BaseApiService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:6703/");
            });
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddScoped<CustomAuthStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthStateProvider>());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}
