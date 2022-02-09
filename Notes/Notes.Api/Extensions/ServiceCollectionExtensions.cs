using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notes.Data.Extensions;

namespace Notes.Api.Extensions
{
    /// <summary>
    /// Содержит методы расширений для настройки сервисов приложения
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Добавляет кастомную аутентификацию
        /// </summary>
        /// <param name="services">Контейнер сервисов</param>
        public static void AddCustomAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("Account/Login");
                });
        }
    }
}
