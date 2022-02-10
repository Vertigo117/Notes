using Microsoft.AspNetCore.Builder;
using Notes.Api.Middleware;

namespace Notes.Api.Extensions
{
    /// <summary>
    /// Методы расширений для настройки Middleware
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Добавить кастомную обработку ошибок
        /// </summary>
        /// <param name="app">Пайплайн</param>
        public static void UseCustomErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddlware>();
        }
    }
}
