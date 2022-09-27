namespace MyTemplate.Web.Security.Token.Configuration;

public class JwtConfig : IJwtConfig
{
  public string Secrets { get; set; } = string.Empty;
  public string Issuer { get; set; } = string.Empty;
  public string Audience { get; set; } = string.Empty;

  public int Duration { get; set; }
}