using BloodSugarTracking.Models;
using Microsoft.EntityFrameworkCore;

namespace BloodSugarTracking.Data;

public class BloodSugarContext : DbContext
{
    public BloodSugarContext(DbContextOptions<BloodSugarContext> options)
        : base(options)
    {
    }

    public DbSet<BloodSugarTestResult>? BloodSugarTestResults { get; set; }

    public DbSet<User>? Users { get; set; }
}
