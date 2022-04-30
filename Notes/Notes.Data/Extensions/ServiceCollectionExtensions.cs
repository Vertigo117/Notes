using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notes.Data.Contexts;
using Notes.Data.Entities;
using Notes.Data.Interfaces;
using Notes.Data.Repositories;

namespace Notes.Data.Extensions
{
    /// <summary>
    /// Содержит методы расширения для добавления сервисов слоя доступа к данным
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Добавить контекст данных и репозитории
        /// </summary>
        /// <param name="services">Контейнер сервисов</param>
        /// <param name="configuration">Параметры конфигурации</param>
        public static void AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(nameof(NotesContext));

            services.AddDbContext<NotesContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IUnitOfWork, RepositoryWrapper>();
            services.AddScoped<IRepository<User>, Repository<User>>();
            services.AddScoped<INotesRepository, NotesRepository>();
        }
    }
}
