using BloodSugarTracking.Data;
using BloodSugarTracking.Options;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
