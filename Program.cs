using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using INTEX.Models;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

var config = builder.Configuration;

// Add services to the container.
var connectionString = config["ConnectionStrings:INTEX"] ??
                       throw new InvalidOperationException("Connection string 'INTEX' not found.");
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
services.AddDatabaseDeveloperPageExceptionFilter();

services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
services.AddControllersWithViews();

services.AddAuthentication();
    // .AddFacebook(options =>
    // {
    //     options.AppId = config["Authentication:Facebook:AppId"] ??
    //                    throw new InvalidOperationException("Authentication string 'Facebook:AppId' not found.");
    //     options.AppSecret = config["Authentication:Facebook:AppSecret"] ??
    //                    throw new InvalidOperationException("Authentication string 'Facebook:AppSecret' not found.");
    // })
    // .AddTwitter(options =>
    // {
    //     options.ConsumerKey = config["Authentication:Twitter:ConsumerKey"] ??
    //                    throw new InvalidOperationException("Authentication string 'Twitter:ConsumerKey' not found.");
    //     options.ConsumerSecret = config["Authentication:Twitter:ConsumerSecret"] ??
    //                    throw new InvalidOperationException("Authentication string 'Twitter:ConsumerSecret' not found.");
    // })
    // .AddGoogle(options =>
    // {
    //     options.ClientId = config["Authentication:Google:ClientId"] ??
    //                    throw new InvalidOperationException("Authentication string 'Google:ClientId' not found.");
    //     options.ClientSecret = config["Authentication:Google:ClientSecret"] ??
    //                    throw new InvalidOperationException("Authentication string 'Google:ClientSecret' not found.");
    // })
    // .AddMicrosoftAccount(options =>
    // {
    //     options.ClientId = config["Authentication:Facebook:ClientId"] ??
    //                    throw new InvalidOperationException("Authentication string 'Facebook:ClientId' not found.");
    //     options.ClientSecret = config["Authentication:Facebook:ClientSecret"] ??
    //                    throw new InvalidOperationException("Authentication string 'Facebook:ClientSecret' not found.");
    // });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();