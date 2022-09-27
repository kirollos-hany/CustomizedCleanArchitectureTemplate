namespace MyTemplate.Web.Security.Token.Configuration;

public interface IJwtConfig
{
  string Secrets { get; set; }
  string Issuer { get; set; }
  string Audience { get; set; }

  int Duration { get; set; }
}
