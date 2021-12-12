using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BloodSugarTracking.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}