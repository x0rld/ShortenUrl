using FluentValidation;
using ShortenUrl.Endpoint;

namespace ShortenUrl;

// ReSharper disable once ClassNeverInstantiated.Global
public class ShortenUrlValidator : Validator<UriRequest>
{
    public ShortenUrlValidator()
    {
        RuleFor(x => x.Size)
            .GreaterThanOrEqualTo(10)
            .WithMessage("too small size token minimum 10")
            .LessThanOrEqualTo(20)
            .WithMessage("max size token 20");

        RuleFor(x => x.Uri)
            .NotEmpty()
            .WithMessage("the url must be set");
    }

}