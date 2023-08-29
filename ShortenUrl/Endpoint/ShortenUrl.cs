using System.Net;
using FluentValidation.Results;
using ShortenUrl.Service;

namespace ShortenUrl.Endpoint;

public record UriRequest(Uri Url, int Size);

public record UriReponse(Uri Url);

public class ShortenUrl : Endpoint<UriRequest, UriReponse>
{
    private readonly IShortenUrlService _shortenUrlService;
    private readonly IDatabaseRepository _databaseRepository;

    public ShortenUrl(IShortenUrlService shortenUrlService, IDatabaseRepository databaseRepository)
    {
        _shortenUrlService = shortenUrlService;
        _databaseRepository = databaseRepository;
    }

    public override void Configure()
    {
        Post("/api/shorten/");
        AllowAnonymous();
        Validator<ShortenUrlValidator>();
    }

    public override async Task HandleAsync(UriRequest request, CancellationToken ct)
    {
        var baseDomain = "https://" +HttpContext.Request.Host.Value;
        var (shortUrl,token) = _shortenUrlService.GenerateShortUrl(baseDomain, request.Size);
        var storedUrl = new StoredUrl(token, request.Url.ToString());
        await _databaseRepository.InsertAsync(storedUrl);
        await SendCreatedAtAsync<AccessToUrl>(shortUrl,new UriReponse(shortUrl),cancellation:ct);
    }
}