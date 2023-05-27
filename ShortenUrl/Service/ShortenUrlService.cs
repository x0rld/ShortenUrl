
namespace ShortenUrl.Service;

public class ShortenUrlServiceService :IShortenUrlService
{
    private readonly IShortIdProvider _idProvider;

    public ShortenUrlServiceService(IShortIdProvider idProvider)
    {
        _idProvider = idProvider;
    }

    public Uri GenerateShortUrl(string baseDomain,int idSize)
    {
        
        return new Uri($"{baseDomain}/{_idProvider.Generate(idSize)}",UriKind.RelativeOrAbsolute);
    }
}

public interface IShortenUrlService
{
    
    Uri GenerateShortUrl(string baseDomain,int idSize);
}