namespace mvc_project.Middelwares;
public class CheckPublicPath(RequestDelegate next)
{
    public readonly RequestDelegate _next = next;
    public async Task InvokeAsync(HttpContext context)
    {
       var path =
            context.Request.Path.Value?.ToLower();

        var allowedPaths = new[]
        {
            "/account/login",
            "/account/register"
        };

        if (path.StartsWith("/notify")|| path.StartsWith("/notificationHub"))
        {
            await _next(context);
            return;
        }
        bool isPublicPage =
            allowedPaths.Contains(path);

        bool isLoggedIn =
            context.User.Identity != null &&
            context.User.Identity.IsAuthenticated;

        if (!isLoggedIn && !isPublicPage)
        {
            context.Response.Redirect(
                "/Account/Login");
            return;
        }

        await _next(context);
    }
}