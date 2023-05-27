global using FastEndpoints;
using ShortenUrl;

var builder = WebApplication.CreateBuilder();
builder.Services.AddSingleton<ShortIdProvider>(provider => new ShortIdProvider(new Random()));
builder.Services.AddFastEndpoints();

var app = builder.Build();
app.UseAuthorization();
app.UseFastEndpoints();
app.Run();

app.MapGet("/", () => "Hello World!");

app.Run();