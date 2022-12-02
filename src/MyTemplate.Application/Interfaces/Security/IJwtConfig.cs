namespace MyTemplate.Application.Interfaces.Security;

public interface IJwtConfig
{

  string Secrets { get; set; }
  string Issuer { get; set; }
  string Audience { get; set; }

//number of months for token to expire
  int Duration { get; set; }
}