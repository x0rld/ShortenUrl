using FluentValidation;
using ShortenUrl.Endpoint;

namespace ShortenUrl;

// ReSharper disable once ClassNeverInstantiated.Global
public class ShortenUrlValidator : Validator<UriRequest>
{
    private static readonly string[] ValidScheme = {"http", "htttps"};
    public ShortenUrlValidator()
    {
        RuleFor(x => x.Size)
            .GreaterThanOrEqualTo(5)
            .WithMessage("too small size token minimum 5")
            .LessThanOrEqualTo(20)
            .WithMessage("max size token 20");

        RuleFor(x => x.Url)
            .NotEmpty()
            .WithMessage("the url must be set")
            .Must(CheckValidWebsiteUrl)
            .WithMessage("malforned url"); 
    }

    private static bool CheckValidWebsiteUrl(Uri url)
    {
        return Uri.TryCreate(url,url, out var result) && !ValidScheme.Contains(result.Scheme);
    }
}