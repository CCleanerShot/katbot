using System.Net;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class AspnetBot
{
    public static AspnetRoutes AspnetRoutes = default!;
    public static HttpListener HttpListener = new HttpListener();
    public static WebApplication WebApplication = default!;

    public static void Start()
    {
        string fullPath = Program.Utility.GetASPPath(Settings.ASPNET_ROOT_DIRECTORY);
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        builder.Services.AddAntiforgery();
        builder.Services.AddRazorPages((options) => options.RootDirectory = $"{fullPath}/Pages");
        string url;

        if (Settings.ENVIRONMENT == "development")
        {
            url = "http://localhost:3000";
        }

        else
        {
            url = Settings.SITE_URL_1;
            builder.Logging.ClearProviders();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CloudFlare Site Policy", policy => policy
                    .WithOrigins(Settings.SITE_URL_1)
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        builder.WebHost.UseStaticWebAssets();
        builder.WebHost.UseUrls([url]);
        WebApplication = builder.Build();
        WebApplication.UseAntiforgery();
        WebApplication.UseHttpsRedirection();
        WebApplication.UseStaticFiles();
        WebApplication.MapRazorPages();
        AspnetRoutes = new AspnetRoutes(WebApplication);
        AspnetRoutes.MapAPIRoutes();

        Program.Utility.Log(Enums.LogLevel.NONE, $"Razor Pages has built! (URL: {url})");
        WebApplication.Run();
    }
}