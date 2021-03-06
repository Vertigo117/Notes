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
        /// <param name="noteUpsertDto">Данные для создания заметки</param>
        /// <param name="email">Адрес электронной почты пользователя</param>
        /// <returns>Ответ на запрос создания заметки, который содержит данные созданной заметки</returns>
        Task<NoteDto> CreateNoteAsync(NoteUpsertDto noteUpsertDto, string email);

        /// <summary>
        /// Постраничный запрос на получение заметок
        /// </summary>
        /// <param name="skip">Сколько пропустить</param>
        /// <param name="take">Сколько взять</param>
        /// <returns>Задача, которая содержит результат выполнения асинхронного постраничного 
        /// запроса на получение заметок</returns>
        Task<PagedNotesDto> GetPagedNotesAsync(string email, int skip, int take);

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
        /// <param name="noteUpsertDto">Данные для обновления</param>
        /// <returns>Результат выполнения асинхронной операции</returns>
        Task UpdateNoteAsync(int id, NoteUpsertDto noteUpsertDto);
    }
}
