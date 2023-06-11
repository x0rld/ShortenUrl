namespace ShortenUrl.Endpoint;

// ReSharper disable once UnusedType.Global
public class AccessToUrl : EndpointWithoutRequest<string>
{
    private readonly IDatabaseRepository _databaseRepository;

    public AccessToUrl(IDatabaseRepository databaseRepository)
    {
        _databaseRepository = databaseRepository;
    }
    public override void Configure()
    {
        Get("/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var urlId = Route<string>("Id");
        ArgumentException.ThrowIfNullOrEmpty(urlId);
        var website = await _databaseRepository.QueryAsync<StoredUrl>(urlId);
        if (website is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        await SendRedirectAsync(website.Website, cancellation: ct);
    }
}