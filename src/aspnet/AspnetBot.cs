using System.Net;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileProviders;

public class AspnetBot
{
    public static HttpListener HttpListener = new HttpListener();
    public static WebApplication WebApplication = default!;

    public static void Start()
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(new WebApplicationOptions() { WebRootPath = $"{Settings.ASPNET_ROOT_DIRECTORY}/wwwroot" });
        Action<RazorPagesOptions> razerOptions = new Action<RazorPagesOptions>((options) => options.RootDirectory = $"{Settings.ASPNET_ROOT_DIRECTORY}");
        builder.Services.AddRazorPages(razerOptions);
        WebApplication = builder.Build();
        WebApplication.UseHttpsRedirection();
        WebApplication.UseStaticFiles();
        WebApplication.MapRazorPages();

        Program.Utility.Log(Enums.LogLevel.NONE, "Razor Pages has built!");
        WebApplication.Run();
    }
}