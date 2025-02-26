using System.Net;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class AspnetBot
{
    public static HttpListener HttpListener = new HttpListener();
    public static WebApplication WebApplication = default!;

    public static void Start()
    {
        string fullPath = Program.Utility.GetASPPath(Settings.ASPNET_ROOT_DIRECTORY);
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        builder.Services.AddRazorPages((options) => options.RootDirectory = $"{fullPath}/Pages");

        if (Settings.ENVIRONMENT == "production")
            builder.WebHost.UseUrls([Settings.SITE_URL_1]);
        else
            builder.WebHost.UseUrls(["http://localhost:3000"]);

        builder.Logging.ClearProviders();
        WebApplication = builder.Build();
        WebApplication.UseHttpsRedirection();
        WebApplication.UseStaticFiles();
        WebApplication.MapRazorPages();
        Program.Utility.Log(Enums.LogLevel.NONE, "Razor Pages has built!");
        WebApplication.Run();
    }
}