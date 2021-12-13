namespace BloodSugarTracking.Services;

public interface ITenantProvider
{
    public Task<string> GetTenantId();
}