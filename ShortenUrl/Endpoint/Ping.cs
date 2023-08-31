namespace ShortenUrl.Endpoint;

public class Ping:  EndpointWithoutRequest<string>
{
    public override void Configure()
    {
        Get("/Ping");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    { 
        await SendAsync("pong", cancellation: ct);
    }
}