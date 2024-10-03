using CourseHub.UI.Helpers.AppStart;
using CourseHub.UI.Middlewares;
using CourseHub.UI.Services;
using CourseHub.UI.Services.Cache;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);
OneTimeRunner.InitConfig(builder);

// Add services to the container.
builder.Services
    .AddRouting(options => options.LowercaseUrls = true)
    .AddResponseCompression(options =>
    {
        // "Don't compress files smaller than about 150-1000 bytes" - Microsoft
        options.EnableForHttps = true;
        options.Providers.Add<GzipCompressionProvider>();
    })
    .AddHttpContextAccessor()
    .AddSession()
    .AddCacheService()
    .AddApiServices()
    .AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions
            .AddPageRoute("/User/VerifyEmail", "verify-email/{email}/{token}")
            .AddPageRoute("/User/SignIn", "signin")
            .AddPageRoute("/User/ExternalLogin", "external")
            .AddPageRoute("/User/Register", "register")
            .AddPageRoute("/User/Profile", "profile")
            .AddPageRoute("/User/ForgotPassword", "forgot-password")
            .AddPageRoute("/User/ResetPassword", "reset-password/{email}/{token}");
    })
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNameCaseInsensitive = true);

var app = builder.Build();

await OneTimeRunner.PrepareFirstUse(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Error").UseHsts();

app
    .UseHttpsRedirection()
    .UseResponseCompression()
    .UseStaticFiles()
    .UseRouting()
    .UseSession()
    .UseAdminViewMiddleware();

app.MapRazorPages();
app.MapFallbackToPage("/Shared/404");

app.Run();