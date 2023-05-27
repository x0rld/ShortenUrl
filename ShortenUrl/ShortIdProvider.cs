namespace ShortenUrl;

public class ShortIdProvider : IShortIdProvider
{
    private readonly Random _random;

    public ShortIdProvider(Random random)
    {
        _random = random;
    }

    public string Generate(int characterCount)
    {
        var bitCount = 6 * characterCount;
        var byteCount = (int) Math.Ceiling(bitCount / 8f);
        var buffer = new byte[byteCount];
        _random.NextBytes(buffer);

        var guid = Convert.ToBase64String(buffer);
        guid = guid.Replace('+', '-').Replace('/', '_');
        return guid[..characterCount];
    }
}

public interface IShortIdProvider
{
    string Generate(int characterCount);
}