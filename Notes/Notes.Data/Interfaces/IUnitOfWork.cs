using Notes.Data.Entities;
using System.Threading.Tasks;

namespace Notes.Data.Interfaces
{
    /// <summary>
    /// Интерфейс для реализации класса-обёртки над репозиториями
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Репозиторий пользователей
        /// </summary>
        public IRepository<User> Users { get; }

        /// <summary>
        /// Репозиторий заметок
        /// </summary>
        public INotesRepository Notes { get; }

        /// <summary>
        /// Сохранить изменения
        /// </summary>
        /// <returns>Результат выполнения асинхронной операции</returns>
        Task SaveChangesAsync();
    }
}
