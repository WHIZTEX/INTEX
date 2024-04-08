using INTEX.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Microsoft.AspNetCore.Http.StatusCodes;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

var config = builder.Configuration;

// Add services to the container.
var connectionString = config["ConnectionStrings:INTEX"] ?? throw new InvalidOperationException("Connection string 'INTEX' not found.");
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
services.AddDatabaseDeveloperPageExceptionFilter();

// Adding Identity Services
services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Configuring Cookie Notification Policy
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential 
    // cookies is needed for a given request.
    options.CheckConsentNeeded = _ => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.ConsentCookieValue = "true";
});

// Configuring additional password requirements
services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 16;
    options.Password.RequiredUniqueChars = 8;
    options.User.RequireUniqueEmail = true;
});
services.AddControllersWithViews();

// Adding authentication and 3rd Party 
services.AddAuthentication()
    .AddMicrosoftAccount(options =>
    {
        options.ClientId = config["Authentication:Microsoft:ClientId"] ?? throw new InvalidOperationException("Authentication string 'Microsoft:ClientId' not found.");
        options.ClientSecret = config["Authentication:Microsoft:ClientSecret"] ?? throw new InvalidOperationException("Authentication string 'Microsoft:ClientSecret' not found.");
    })
    .AddGoogle(options =>
    {
        options.ClientId = config["Authentication:Google:ClientId"] ?? throw new InvalidOperationException("Authentication string 'Google:ClientId' not found.");
        options.ClientSecret = config["Authentication:Google:ClientSecret"] ?? throw new InvalidOperationException("Authentication string 'Google:ClientSecret' not found.");
    });

// Changing Redirect code to 307
services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = Status307TemporaryRedirect;
});

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
app.UseCookiePolicy();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();