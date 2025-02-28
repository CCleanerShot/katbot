using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public abstract class RootModel : PageModel
{
    protected readonly ILogger<RootModel> Logger;

    [BindProperty]
    public Credentials Credentials { get; set; } = new Credentials();

    public RootModel(ILogger<RootModel> logger)
    {
        Logger = logger;
    }

    public virtual async Task<IActionResult> CustomGet() { return Page(); }
    public virtual async Task CustomPost() { return; }
    public virtual async Task<IActionResult> HTMX_Get() { return BadRequest("No such HTMX route exists!"); }
    public virtual async Task HTMX_Post() { return; }

    public async Task<IActionResult> OnGet()
    {
        if (Request.IsHtmx())
            return await HTMX_Get();
        else
            return await CustomGet();
    }

    public async Task OnPost()
    {
        if (Request.IsHtmx())
            await HTMX_Post();
        else
            await CustomPost();
    }
}