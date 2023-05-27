using NFluent;

namespace ShortenUrl.Tests;

public class ShortIdTests
{
    private readonly IShortIdProvider _sut;

    public ShortIdTests()
    {
        _sut = new ShortIdProvider(new Random(100));
    }

    [Fact]
    public void Generate_Should_Returns_A_Span_Based_On_Parameter_Length()
    {
        var actual = _sut.Generate(4);
        Check.That(actual.Length).Is(4);
    }
}