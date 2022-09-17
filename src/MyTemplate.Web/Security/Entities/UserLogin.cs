using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MyTemplate.Web.Security.Entities;

[Table("UserLogins")]
public class UserLogin : IdentityUserLogin<Guid>
{
  
}
