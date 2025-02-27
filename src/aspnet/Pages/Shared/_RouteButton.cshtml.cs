namespace skyblock_bot.Pages.Shared;

public class RouteButton
{
    public string Route;
    public string Title;
    public string? ImageUrl;

    public RouteButton(string _Route, string _Title, string? _ImageUrl = null)
    {
        Route = _Route;
        Title = _Title;
        ImageUrl = _ImageUrl;
    }
}