using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notes.Data.Contexts;

namespace Notes.Api.Extensions
{
    public static class HostExtensions
    {
        public static IHost ApplyMigrations(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<NotesContext>();
            context.Database.Migrate();
            return host;
        }
    }
}
