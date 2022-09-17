using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MyTemplate.Web.Security.Entities;

[Table("RoleClaims")]
public class RoleClaim : IdentityRoleClaim<Guid>
{

}
