using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace MyTemplate.Web.Security.Entities;

[Table("UserClaims")]
public class UserClaim : IdentityUserClaim<Guid>
{
  
}
