using Identity.API.Data;
using Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Polly;

namespace Identity.API
{
    public class SeedData
    {
        public static async Task SeedUsers(IServiceScope scope, IConfiguration config, ILogger logger)
        {
            var retryPolicy = CreateRetryPolicy(config, logger);
            var context = scope.ServiceProvider.GetRequiredService<IdentityApiDbContext>();
            await retryPolicy.ExecuteAsync(async () =>
            {
                await context.Database.MigrateAsync();
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
            });
        }
        private static AsyncPolicy CreateRetryPolicy(IConfiguration configuration, Microsoft.Extensions.Logging.ILogger logger)
        {
            var retryMigrations = false;
            bool.TryParse(configuration["RetryMigrations"], out retryMigrations);

            // Only use a retry policy if configured to do so.
            // When running in an orchestrator/K8s, it will take care of restarting failed services.
            if (retryMigrations)
            {
                return Policy.Handle<Exception>().
                    WaitAndRetryForeverAsync(
                        sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                        onRetry: (exception, retry, timeSpan) =>
                        {
                            logger.LogWarning(
                                exception,
                                "Exception {ExceptionType} with message {Message} detected during database migration (retry attempt {retry})",
                                exception.GetType().Name,
                                exception.Message,
                                retry);
                        }
                    );
            }

            return Policy.NoOpAsync();
        }
    }
}
