
namespace ShortenUrl.Service;

public class ShortenUrlServiceService :IShortenUrlService
{
    private readonly ShortIdProvider _idProvider;

    public ShortenUrlServiceService(ShortIdProvider idProvider)
    {
        _idProvider = idProvider;
    }

    public Uri GenerateShortUrl(Uri baseDomain,int idSize)
    {
        
        return new Uri(baseDomain + _idProvider.Generate(idSize));
    }
}

public interface IShortenUrlService
{
    
    Uri GenerateShortUrl(Uri baseDomain,int idSize);
}