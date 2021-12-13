namespace BloodSugarTracking.Services;

public class TenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<IdentityUser> _userManager;

    public TenantProvider(IHttpContextAccessor httpContextAccessor,
        UserManager<IdentityUser> userManager)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    async Task<string> ITenantProvider.GetTenantId()
    {
        ArgumentNullException.ThrowIfNull(_httpContextAccessor.HttpContext);
        ArgumentNullException.ThrowIfNull(_httpContextAccessor.HttpContext.User.Identity);
        string? username = _httpContextAccessor.HttpContext.User.Identity.Name;
        ArgumentNullException.ThrowIfNull(username);
        IdentityUser user = await _userManager.FindByNameAsync(username);
        return user.Id;
    }
}