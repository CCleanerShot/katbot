using System.Net;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class AspnetBot
{
    public static HttpListener HttpListener = new HttpListener();
    public static WebApplication WebApplication = default!;

    public static void Start()
    {
        string fullPath = Program.Utility.GetASPPath(Settings.ASPNET_ROOT_DIRECTORY);
        WebApplicationBuilder builder = WebApplication.CreateBuilder(new WebApplicationOptions() { WebRootPath = $"{fullPath}/wwwroot" });
        Action<RazorPagesOptions> razerOptions = new Action<RazorPagesOptions>((options) => options.RootDirectory = $"{fullPath}");
        builder.Services.AddRazorPages(razerOptions);
        WebApplication = builder.Build();
        WebApplication.UseHttpsRedirection();
        WebApplication.UseStaticFiles();
        WebApplication.MapRazorPages();

        Program.Utility.Log(Enums.LogLevel.NONE, "Razor Pages has built!");
        WebApplication.Run();
    }
}