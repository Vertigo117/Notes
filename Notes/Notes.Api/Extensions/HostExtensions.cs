using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notes.Data.Contexts;

namespace Notes.Api.Extensions
{
    /// <summary>
    /// Методы расширений для конфигурации хоста
    /// </summary>
    public static class HostExtensions
    {
        /// <summary>
        /// Применить миграции EF Core
        /// </summary>
        /// <param name="host">Хост</param>
        /// <returns>Хост</returns>
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
