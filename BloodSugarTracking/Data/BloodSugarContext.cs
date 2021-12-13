namespace BloodSugarTracking.Data;

public class BloodSugarContext : DbContext
{
    private readonly ITenantProvider _tenantProvider;

    public BloodSugarContext(DbContextOptions<BloodSugarContext> options,
        ITenantProvider tenantProvider) : base(options)
    {
        _tenantProvider = tenantProvider;
    }

    public DbSet<BloodSugarTestResult>? BloodSugarTestResults { get; set; }

    public DbSet<User>? Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        string tenantId = _tenantProvider.GetTenantId().Result;

        modelBuilder.Entity<User>().HasQueryFilter(u => u.TenantId == tenantId);
        modelBuilder.Entity<BloodSugarTestResult>().HasQueryFilter(b => b.TenantId == tenantId);
    }
}
