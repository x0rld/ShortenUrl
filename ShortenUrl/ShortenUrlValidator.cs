using FluentValidation;
using ShortenUrl.Endpoint;

namespace ShortenUrl;

public class ShortenUrlValidator : Validator<UriRequest>
{
    public ShortenUrlValidator()
    {
        RuleFor(x => x.Size)
            .GreaterThanOrEqualTo(10)
            .WithMessage("too small size token");

        RuleFor(x => x.Uri)
            .NotEmpty()
            .WithMessage("the url must be set");
    }

}