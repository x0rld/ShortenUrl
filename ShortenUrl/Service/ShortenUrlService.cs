
namespace ShortenUrl.Service;

public class ShortenUrlServiceService :IShortenUrlService
{
    private readonly IShortIdProvider _idProvider;

    public ShortenUrlServiceService(IShortIdProvider idProvider)
    {
        _idProvider = idProvider;
    }

    public  (Uri shortendUrl, string token) GenerateShortUrl(string baseDomain,int idSize)
    {
        var token = _idProvider.Generate(idSize);
        return (new Uri($"{baseDomain}/{token}", UriKind.RelativeOrAbsolute), token);
    }
}

public interface IShortenUrlService
{
    
    (Uri shortendUrl, string token) GenerateShortUrl(string baseDomain,int idSize);
}