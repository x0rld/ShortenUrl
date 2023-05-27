using ShortenUrl.Service;

namespace ShortenUrl.Tests;

public class ShortenUrlTests
{
    private readonly IShortenUrlService _sut;
    private readonly string _uri = "https://google.com";
    public ShortenUrlTests()
    {
        _sut = new ShortenUrlServiceService(new ShortIdProvider(new Random(50)));
    }


    [Fact]
    public void GetShortUrl_Should_Return_An_Uri_Not_Null()
    {
        var actual = _sut.GenerateShortUrl(_uri,1);
        Check.That(actual).IsNotNull().And.IsInstanceOfType(typeof(Uri));
    }
    
    [Fact]
    public void GetShortUrl_Should_Return_An_String_With_Length_10()
    {
        var actual = _sut.GenerateShortUrl(_uri,10);
        var absolutePath = actual.AbsolutePath;
        Check.That(absolutePath[1..absolutePath.Length].Length).Is(10);
    }
}