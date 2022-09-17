using MyTemplate.Web.Security.Entities;

namespace MyTemplate.Web.Security.Interfaces;

public interface IAuthenticator<TUser> where TUser : User
{
    public Task<IAuthenticationResult> AuthenticateByEmailAsync(string email, string password);

    public Task<IAuthenticationResult> AuthenticateByNameAsync(string name, string password);
}