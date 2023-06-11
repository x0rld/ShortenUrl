global using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.Data.Sqlite;
using ShortenUrl;
using ShortenUrl.Service;

var builder = WebApplication.CreateBuilder();
builder.Services.AddLogging(loggingBuilder => loggingBuilder
    .AddFilter(level => level >= LogLevel.Debug)
    .AddFile("Logs/shortenURl-{Date}.log")
    .AddConsole()
);
builder.Services.AddSingleton<IShortIdProvider, ShortIdProvider>(_ => new ShortIdProvider(new Random()));
builder.Services.AddSingleton<IShortenUrlService, ShortenUrlServiceService>(provider => new ShortenUrlServiceService(
    provider.GetRequiredService<IShortIdProvider>()));
builder.Services.AddTransient<IDatabaseRepository, DatabaseRepository>(
    _ => new DatabaseRepository(new SqliteConnection("Data Source= shortURL.sqlite"))
);

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


app.UseDefaultExceptionHandler();
app.UseAuthorization();
app.UseFastEndpoints();
app.UseSwaggerGen();
app.Run();