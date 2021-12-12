using BloodSugarTracking.Data;
using BloodSugarTracking.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

string identityDbConnectionString = builder.Configuration.GetConnectionString("IdentityDbConnectionString");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(identityDbConnectionString));
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

string bloodSugarDbConnectionString = builder.Configuration.GetConnectionString("BloodSugarDbConnectionString");
builder.Services.AddDbContext<BloodSugarContext>(option => option.UseSqlServer(bloodSugarDbConnectionString));

builder.Services.Configure<BloodSugarOptions>(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
