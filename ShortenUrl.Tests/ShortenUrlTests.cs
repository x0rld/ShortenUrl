using ShortenUrl.Service;

namespace ShortenUrl.Tests;

public class ShortenUrlTests
{
    private readonly IShortenUrlService _sut;
    private const string Uri = "https://google.com";
    public ShortenUrlTests()
    {
        _sut = new ShortenUrlServiceService(new ShortIdProvider(new Random(50)));
    }


    [Fact]
    public void GetShortUrl_Should_Return_An_Uri_Not_Null()
    {
        var (actualUri,_)= _sut.GenerateShortUrl(Uri,1);
        Check.That(actualUri).IsNotNull();
    }
    
    [Fact]
    public void GetShortUrl_Should_Return_An_Url_With_A_Key_Of_Length_10()
    {
        var (_,actualKey) = _sut.GenerateShortUrl(Uri,10);
        Check.That(actualKey.Length).Is(10);
    }
}