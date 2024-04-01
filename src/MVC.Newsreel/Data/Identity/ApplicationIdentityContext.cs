using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MVC.Newsreel.Data.Identity;
public class ApplicationIdentityContext : IdentityDbContext<ApplicationUser>
{
public ApplicationIdentityContext(DbContextOptions<ApplicationIdentityContext> options)
: base(options)
{
}
}