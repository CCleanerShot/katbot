using System.Reflection;

/// <summary>
/// For usage inside AspnetRoutes methods. NOTE: The method must be public!
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public partial class APIRouteAttribute : Attribute { }


/// <summary>
/// Create routes in the /API folder.
/// </summary>
public partial class AspnetRoutes
{
    public WebApplication WebApplication;

    public AspnetRoutes(WebApplication _WebApplication)
    {
        WebApplication = _WebApplication;
    }

    public void MapAPIRoutes()
    {
        IEnumerable<MethodInfo> methods = GetType().GetMethods()
            .Where(s => s.GetCustomAttribute<APIRouteAttribute>() != null);

        foreach (MethodInfo method in methods)
            method.Invoke(this, null);
    }
}