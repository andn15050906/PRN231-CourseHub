using CourseHub.API.Helpers.AppStart;
using CourseHub.API.Services.ApiDocumentation;
using CourseHub.API.Services.Domain;
using CourseHub.API.Services.Logging;
using CourseHub.API.Services.Email;
using Serilog;
using CourseHub.API.Services.AppInfo;
using CourseHub.API.Services.Authentication;
using CourseHub.API.Services.External.Payment;
using Microsoft.Extensions.FileProviders;
using CourseHub.API.Realtime.Hubs;
using CourseHub.API.Services.Realtime;

const string POLICY = "Policy";

var builder = WebApplication.CreateBuilder(args);
OneTimeRunner.InitConfig(builder);

// Add services to the container.

builder.Services
    .AddCors(o => o.AddPolicy(POLICY, builder =>
        builder
            .WithOrigins(Configurer.GetCorsOrigins())
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod())
    )
    .AddLogger()
    .AddMapper()
    .AddAppInfo()
    .AddAuthenticationServices()
    .AddEmailService()
    .AddDomainServices()
    .AddPaymentService()
    .AddDocumentation()
    .AddRealtimeService()
    .AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseDocumentation();

app
    .UseSerilogRequestLogging()
    .UseHttpsRedirection()
    .UseCors(POLICY)
    .UseAuthentication()
    .UseAuthorization()
    .UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
        RequestPath = "/api/courses/Resource/Media",
        OnPrepareResponse = context =>
        {
            System.Diagnostics.Debug.WriteLine(context.File);
        }
    });

app.MapControllers();
app.MapHub<AppHub>("/hub");

app.Run();

public partial class Program { }