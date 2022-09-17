using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MyTemplate.Web.Security.Entities;

[Table("UserRoles")]
public class UserRole : IdentityUserRole<Guid>
{
  
}
