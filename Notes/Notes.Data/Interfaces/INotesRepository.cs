using Notes.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Notes.Data.Interfaces
{
    /// <summary>
    /// Дополнительные методы для репозитория заметок
    /// </summary>
    public interface INotesRepository : IRepository<Note>
    {
        /// <summary>
        /// Постраничное получение заметок
        /// </summary>
        /// <param name="condition">Условие для фильтрации</param>
        /// <param name="take">Количество заметок на странице</param>
        /// <param name="skip">Сколько пропустить</param>
        /// <returns>Задачу, которая содержит результат выполнения асинхронного запроса на получение заметок</returns>
        Task<IEnumerable<Note>> GetPagedAsync(Expression<Func<Note, bool>> condition, int skip, int take);

        /// <summary>
        /// Получить общее количество всех заметок в бд
        /// </summary>
        /// <param name="condition">Условие для фильтрации</param>
        /// <returns>Задачу, которая содержит результат выполнения асинхронного запроса</returns>
        Task<int> GetTotalAsync(Expression<Func<Note, bool>> condition);
    }
}
