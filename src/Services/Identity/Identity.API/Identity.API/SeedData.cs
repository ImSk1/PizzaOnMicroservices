using Identity.API.Data;
using Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.API
{
    public class SeedData
    {
        public static async Task SeedUsers(IServiceScope scope, IConfiguration config, ILogger logger)
        {
            var context = scope.ServiceProvider.GetRequiredService<IdentityApiDbContext>();


            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var pesho = await userManager.FindByNameAsync("pesho");

            if (pesho == null)
            {
                pesho = new ApplicationUser()
                {
                    UserName = "pesho",
                    Email = "PeshoPeshov@email.com",
                    EmailConfirmed = true,
                    Id = Guid.NewGuid().ToString(),
                };
                
                var result = userManager.CreateAsync(pesho, "peshoPicha123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                logger.LogDebug("pesho created");
            }
            else
            {
                logger.LogDebug("pesho already exists");
            }
        }
    }
}
