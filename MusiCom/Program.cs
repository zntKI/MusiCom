using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusiCom.Infrastructure.Data;
using MusiCom.Infrastructure.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => 
{
    options.SignIn.RequireConfirmedAccount = builder.Configuration.GetValue<bool>("Identity:RequireConfirmedAccount");
    options.SignIn.RequireConfirmedPhoneNumber = builder.Configuration.GetValue<bool>("Identity:RequireConfirmedPhoneNumber");
    options.SignIn.RequireConfirmedEmail = builder.Configuration.GetValue<bool>("Identity:RequireConfirmedEmail");
    options.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("Identity:RequireNonAlphanumeric");
    options.Password.RequiredLength = builder.Configuration.GetValue<int>("Identity:RequiredLength");
    options.Password.RequireDigit = builder.Configuration.GetValue<bool>("Identity:RequireDigit");
    options.Password.RequireUppercase = builder.Configuration.GetValue<bool>("Identity:RequireUppercase");
})
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddApplicationServices();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
});
builder.Services.AddMvc(options =>
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "Areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapDefaultControllerRoute();
    endpoints.MapRazorPages();
});

app.Run();
