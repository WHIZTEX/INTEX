using INTEX.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using static Microsoft.AspNetCore.Http.StatusCodes;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

var config = builder.Configuration;

var vault = new SecretClient(new Uri(config["KeyVault"]!), new DefaultAzureCredential());
var dbConn = vault.GetSecret("INTEX").Value.Value;
var microsoftId = vault.GetSecret("MicrosoftClientId").Value.Value;
var microsoftSecret = vault.GetSecret("MicrosoftClientSecret").Value.Value;
var googleId = vault.GetSecret("GoogleClientId").Value.Value;
var googleSecret = vault.GetSecret("GoogleClientSecret").Value.Value;

// Add context files
services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(dbConn));

// Add instance of repository based off interface
services.AddScoped<IRepo, EfRepo>();

// Add exception pages
services.AddDatabaseDeveloperPageExceptionFilter();

// Adding identity services
services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Configuring Cookie Notification Policy
services.Configure<CookiePolicyOptions>(options =>
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
    // Number of characters in a password that must be different
    // Prevents `Aa1!Aa1!Aa1!Aa1!` style passwords
    options.Password.RequiredUniqueChars = 8;
    // Requires an email address to be unique
    options.User.RequireUniqueEmail = true;
});

// Configuring for controllers
services.AddControllersWithViews();

// Adding authentication and 3rd Party 
services.AddAuthentication()
    .AddMicrosoftAccount(options =>
    {
        options.ClientId = microsoftId;
        options.ClientSecret = microsoftSecret;
    })
    .AddGoogle(options =>
    {
        options.ClientId = googleId;
        options.ClientSecret = googleSecret;
    });

// Changing Redirect code to 307
services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = Status307TemporaryRedirect;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Append("Content-Security-Policy", "font-src 'self' https://fonts.googleapis.com https://fonts.gstatic.com");
        await next();
    });

    app.UseExceptionHandler("/Customer/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Activating HTTPS/Cookie services
app.UseHttpsRedirection();
app.UseCookiePolicy();

// Activating static and routing
app.UseStaticFiles();
app.UseRouting();

// Activating identity services
app.UseAuthentication();
// *MUST* be after Authentication or users cannot log out
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Running app
app.Run();
