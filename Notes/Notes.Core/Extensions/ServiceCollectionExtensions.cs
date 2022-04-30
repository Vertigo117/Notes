using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Notes.Core.Interfaces;
using Notes.Core.Services;
using System.Reflection;

namespace Notes.Core.Extensions
{
    /// <summary>
    /// Содержит методы расширений для регистрации сервисов слоя бизнес-логики
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Добавить сервисы слоя бизнес-логики
        /// </summary>
        /// <param name="services">Контейнер сервисов</param>
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<INotesService, NotesService>();

            services.AddFluentValidation(configuration => 
                configuration.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}
