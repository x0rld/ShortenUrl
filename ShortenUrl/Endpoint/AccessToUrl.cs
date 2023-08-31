using Microsoft.AspNetCore.Mvc;

namespace ShortenUrl.Endpoint;

// ReSharper disable once UnusedType.Global
public class AccessToUrl : EndpointWithoutRequest<string>
{
    private readonly IDatabaseRepository _databaseRepository;
    private readonly Serilog.ILogger _logger;

    public AccessToUrl(IDatabaseRepository databaseRepository, Serilog.ILogger logger)
    {
        _databaseRepository = databaseRepository;
        _logger = logger;
    }
    public override void Configure()
    {
        Get(new []{"/{Id}", "/"});
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<string>("Id");
        if (string.IsNullOrEmpty(id))
        {
            await SendNotFoundAsync(ct); 
            return;
        }
        var website = await _databaseRepository.QueryAsync<StoredUrl>(id);
        if (website is null)
        {
            await SendNotFoundAsync(ct);            
            _logger.Error("id {Id} not found in the database",id);
            return;
        }
        _logger.Information("the website with the id {Id} is {Url}",id,website.Website);
        
        if (HttpContext.Request.Headers.TryGetValue("referer", out var referer) && referer[0]!.Contains("swagger"))
        {
            await SendAsync(new {website.Website }.ToString()!, StatusCodes.Status302Found,ct);
            return;
        }
        await SendRedirectAsync(website.Website,false,ct);
    }
}