using System.Net;
using FluentValidation.Results;
using ShortenUrl.Service;

namespace ShortenUrl.Endpoint;

public record UriRequest(Uri Uri, int Size);

public record UriReponse(Uri Uri);

public class ShortenUrl : Endpoint<UriRequest, UriReponse>
{
    private readonly IShortenUrlService _shortenUrlService;

    public ShortenUrl(IShortenUrlService shortenUrlService)
    {
        _shortenUrlService = shortenUrlService;
    }

    public override void Configure()
    {
        Post("/api/shorten/");
        AllowAnonymous();
        Validator<ShortenUrlValidator>();
    }

    public override async Task HandleAsync(UriRequest request, CancellationToken ct)
    {

        if (!Uri.TryCreate(request.Uri,request.Uri, out var result) &&result?.Scheme is not ("https" or "http"))
        {
            AddError(new ValidationFailure
            {
                ErrorCode = 400.ToString(),
                ErrorMessage = "malformed url",
                PropertyName = nameof(request.Uri),
            });
        }
        ThrowIfAnyErrors();
        var requestHost = HttpContext.Request;
        var baseDomain = $"{requestHost.Scheme}://{requestHost.Host.Value}";
        var shortUrl = _shortenUrlService.GenerateShortUrl(baseDomain, request.Size);
        await SendAsync(new UriReponse(shortUrl), (int) HttpStatusCode.Created, ct);
    }
}