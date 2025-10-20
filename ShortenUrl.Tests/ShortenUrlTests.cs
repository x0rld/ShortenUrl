using ShortenUrl.Service;
using Xunit;

namespace ShortenUrl.Tests;

public class ShortenUrlTests
{
    private readonly IShortenUrlService _sut = new ShortenUrlServiceService(new ShortIdProvider(new Random(50)));
    private const string Uri = "https://google.com";


    [Fact]
    public void GetShortUrl_Should_Return_An_Uri_Not_Null()
    {
        var (actualUri,_)= _sut.GenerateShortUrl(Uri,1);
        Assert.NotNull(actualUri);
    }
    
    [Fact]
    public void GetShortUrl_Should_Return_An_Url_With_A_Key_Of_Length_10()
    {
        var (_,actualKey) = _sut.GenerateShortUrl(Uri,10);
        Assert.Equal(10, actualKey.Length);
    }
}