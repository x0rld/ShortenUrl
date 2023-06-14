
namespace ShortenUrl.Service;

public class ShortenUrlServiceService :IShortenUrlService
{
    private readonly IShortIdProvider _idProvider;

    public ShortenUrlServiceService(IShortIdProvider idProvider)
    {
        _idProvider = idProvider;
    }

    public  (Uri shortendUrl,string Key) GenerateShortUrl(string baseDomain,int idSize)
    {
        var key = _idProvider.Generate(idSize);
        return (new Uri($"{baseDomain}/{key}", UriKind.RelativeOrAbsolute), key);
    }
}

public interface IShortenUrlService
{
    
    (Uri shortendUrl,string Key) GenerateShortUrl(string baseDomain,int idSize);
}