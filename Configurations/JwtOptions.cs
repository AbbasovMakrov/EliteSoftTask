namespace EliteSoftTask.Configurations;

public class JwtOptions
{
    public string Secret { get; init; }
    public string Issuer { get; init; }
}