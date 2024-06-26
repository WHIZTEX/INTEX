using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using INTEX.Models;
using INTEX.Models.DatabaseModels;
using INTEX.Models.Infrastructure;
using Microsoft.ML.OnnxRuntime;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using newfeature;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

var config = builder.Configuration;

var vault = new SecretClient(new Uri(config["KeyVault"]!), new DefaultAzureCredential());
var dbConn = (await vault.GetSecretAsync("INTEX")).Value.Value;
var microsoftId = (await vault.GetSecretAsync("MicrosoftClientId")).Value.Value;
var microsoftSecret = (await vault.GetSecretAsync("MicrosoftClientSecret")).Value.Value;
var googleId = (await vault.GetSecretAsync("GoogleClientId")).Value.Value;
var googleSecret = (await vault.GetSecretAsync("GoogleClientSecret")).Value.Value;

builder.Services.AddSignalR();
// Add context files
services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(dbConn, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure();
    });
});

// Add instance of repository based off interface
services.AddScoped<IRepo, EfRepo>();

// Add exception pages
services.AddDatabaseDeveloperPageExceptionFilter();

// Adding identity services
services.AddDefaultIdentity<Customer>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
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

var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
var onnxRelative = Path.Combine("Models", "MachineLearning", "fraudModel.onnx");
var onnxAbsolute = Path.Combine(baseDirectory, onnxRelative);
// Adding Inference Session singleton of InferenceSession from fraud model
services.AddSingleton(new InferenceSession(onnxAbsolute));
services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // Add HttpContextAccessor

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

// Activating session
app.UseSession();

// Activating identity services
app.UseAuthentication();
// *MUST* be after Authentication or users cannot log out
app.UseAuthorization();

app.MapControllerRoute(
    name: "Home",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Admin",
    pattern: "{controller=Admin}/{action=Index}/{id?}");
    
app.MapRazorPages();
app.MapHub<ChatHub>("/chatHub");

// Running app
app.Run();
