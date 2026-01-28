using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.ORM.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<DefaultContext>();

        try
        {
            ctx.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while migrating database: {ex.Message}");
            throw;
        }
    }
}