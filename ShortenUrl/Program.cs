global using FastEndpoints;
using System.Data;
using Dapper;
using FastEndpoints.Swagger;
using Microsoft.Data.Sqlite;
using ShortenUrl;
using ShortenUrl.Service;
using Serilog;
using SQLitePCL;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, configuration) =>
    configuration.Enrich.FromLogContext()
        .WriteTo.RollingFile("Logs/shortenURl.log",
            outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}")
        .WriteTo.Console());
builder.Services.AddSingleton<IDbConnection>(it => new SqliteConnection("Data Source= shortURL2.sqlite") );
builder.Services.AddScoped(it => new Random() );
builder.Services.AddScoped<IShortIdProvider, ShortIdProvider>();
builder.Services.AddScoped<IShortenUrlService, ShortenUrlServiceService>();
builder.Services.AddScoped<IDatabaseRepository, DatabaseRepository>();
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
await InitDatabase();

app.UseSerilogRequestLogging();
app.Run();

async Task InitDatabase()
{
    var dbConnection = app.Services.GetRequiredService<IDbConnection>();
    dbConnection.Open();
    var initSqlScript = await File.ReadAllTextAsync("init.sql");
    var isExist = await dbConnection.QueryFirstOrDefaultAsync<bool>("SELECT 1 FROM sqlite_master WHERE type='table' AND name='storedUrl'");
    if (isExist)
    {
     return;   
    }
    await dbConnection.QueryAsync(initSqlScript);
}
