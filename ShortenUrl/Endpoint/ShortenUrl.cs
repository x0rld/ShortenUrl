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

        if (!Uri.TryCreate(request.Url,request.Url, out var result) &&result?.Scheme is not ("https" or "http"))
        {
            AddError(new ValidationFailure
            {
                ErrorCode = 400.ToString(),
                ErrorMessage = "malformed url x",
                PropertyName = nameof(request.Url),
            });
        }
        var requestHost = HttpContext.Request;
        var baseDomain = $"{requestHost.Scheme}://{requestHost.Host.Value}";
        var (shortUrl,key) = _shortenUrlService.GenerateShortUrl(baseDomain, request.Size);
        var storedUrl = new StoredUrl(key, request.Url.ToString());
        await _databaseRepository.InsertAsync(storedUrl);
        await SendAsync(new UriReponse(shortUrl), (int) HttpStatusCode.Created, ct);
    }
}