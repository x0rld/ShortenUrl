global using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.Data.Sqlite;
using ShortenUrl;
using ShortenUrl.Service;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, configuration) =>
    configuration.Enrich.FromLogContext()
        .WriteTo.RollingFile("Logs/shortenURl.log",
            outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}")
        .WriteTo.Console());
builder.Services.AddSingleton<IShortIdProvider, ShortIdProvider>(_ => new ShortIdProvider(new Random()));
builder.Services.AddSingleton<IShortenUrlService, ShortenUrlServiceService>(provider => new ShortenUrlServiceService(
    provider.GetRequiredService<IShortIdProvider>()));
builder.Services.AddTransient<IDatabaseRepository, DatabaseRepository>(
    _ => new DatabaseRepository(new SqliteConnection("Data Source= shortURL.sqlite"))
);
builder.Services.AddCors(options => options.AddPolicy("corsLocalPolicy",
    policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
            .AllowAnyHeader().AllowAnyMethod();
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

app.UseCors("corsLocalPolicy");
app.UseDefaultExceptionHandler();
app.UseAuthorization();
app.UseFastEndpoints();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.UseSerilogRequestLogging();
app.Run();