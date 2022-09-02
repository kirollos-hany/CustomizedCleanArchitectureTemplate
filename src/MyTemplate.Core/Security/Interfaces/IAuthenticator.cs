using MyTemplate.Core.Security.Entities;

namespace MyTemplate.Core.Security.Interfaces;

public interface IAuthenticator<TUser> where TUser : User
{
    public Task<IAuthenticationResult> AuthenticateByEmailAsync(string email, string password);

    public Task<IAuthenticationResult> AuthenticateByNameAsync(string name, string password);
}