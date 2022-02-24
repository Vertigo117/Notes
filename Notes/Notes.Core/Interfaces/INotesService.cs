using Microsoft.AspNetCore.Http;
using Notes.Core.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notes.Core.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса управления заметками
    /// </summary>
    public interface INotesService
    {
        /// <summary>
        /// Получить заметку с указанным уникальным идентификатором
        /// </summary>
        /// <param name="id">Уникальный идентификатор заметки</param>
        /// <returns>Заметка с указанным уникальным идентификатором</returns>
        Task<NoteDto> GetNoteAsync(int id);

        /// <summary>
        /// Обработать запрос на создание заметки пользователя
        /// </summary>
        /// <param name="request">Запрос на создание заметки</param>
        /// <param name="httpContext">Контекст запроса</param>
        /// <returns>Ответ на запрос создания заметки, который содержит данные созданной заметки</returns>
        Task<NoteDto> CreateNoteAsync(NoteUpsertDto request, HttpContext httpContext);

        /// <summary>
        /// Обработать запрос на получение заметок пользователя
        /// </summary>
        /// <param name="httpContext">Контекст запроса</param>
        /// <returns>Результат выполнения запроса, который содержит коллекцию заметок пользователя</returns>
        Task<IEnumerable<NoteDto>> GetAllNotesAsync(HttpContext httpContext);

        /// <summary>
        /// Удалить заметку с указанным уникальным идентификатором
        /// </summary>
        /// <param name="id">Уникальный идентификатор заметки</param>
        /// <returns>Результат выполнения асинхронной операции</returns>
        Task DeleteNoteAsync(int id);

        /// <summary>
        /// Обработать запрос на обновление данных заметки
        /// </summary>
        /// <param name="id">Уникальный идентификатор заметки</param>
        /// <param name="request">Данные для обновление</param>
        /// <returns>Результат выполнения асинхронной операции</returns>
        Task UpdateNoteAsync(int id, NoteUpsertDto request);
    }
}
