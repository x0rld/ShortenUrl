global using FastEndpoints;
using System.Data;
using FastEndpoints.Swagger;
using Microsoft.Data.Sqlite;
using ShortenUrl;
using ShortenUrl.Service;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(it => it.AddServerHeader = false);

builder.Services.AddDependencyInjection();
builder.Host.UseSerilog((_, configuration) =>
    configuration.Enrich.FromLogContext()
        .WriteTo.RollingFile("Logs/shortenURl.log",
            outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}")
        .WriteTo.Console());

builder.Services.AddCors(options => options.AddPolicy("corsPolicy",
    policyBuilder =>
    {
        policyBuilder.AllowAnyMethod().WithHeaders("content-type", "access-control-allow-origin")
            .WithOrigins("http://localhost:5173", "https://react-short-url.netlify.app");
    }));

builder.Services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.Title = "My API";
        s.Version = "v1";
    };
});
builder.Services.AddFastEndpoints();

var app = builder.Build();
await new Setup(app.Services.GetRequiredService<IDbConnection>()).InitDatabase();
app.UseCors("corsPolicy");
app.UseDefaultExceptionHandler();
app.UseFastEndpoints();
app.UseHealthChecks("/health");
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.UseSerilogRequestLogging();
app.Run();

internal static class Extension
{
    public static void AddDependencyInjection(this IServiceCollection services)
    {
        services.AddSingleton<IDbConnection>(it => new SqliteConnection("Data Source= shortURL.sqlite"));
        services.AddScoped(_ => new Random());
        services.AddScoped<IShortIdProvider, ShortIdProvider>();
        services.AddScoped<IShortenUrlService, ShortenUrlServiceService>();
        services.AddScoped<IDatabaseRepository, DatabaseRepository>();
        services.AddHealthChecks();
    }
}