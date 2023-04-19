using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Identity.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {

    }
}
