using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MyTemplate.Web.Security.Entities;

[Table("UserTokens")]
public class UserToken : IdentityUserToken<Guid>
{

}
