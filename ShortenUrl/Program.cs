global using FastEndpoints;
using FastEndpoints.Swagger;
using ShortenUrl;
using ShortenUrl.Service;

var builder = WebApplication.CreateBuilder();
builder.Services.AddSingleton<IShortIdProvider, ShortIdProvider>(_ => new ShortIdProvider(new Random()));
builder.Services.AddSingleton<IShortenUrlService, ShortenUrlServiceService>(provider => new ShortenUrlServiceService(
    provider.GetService<IShortIdProvider>()));

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