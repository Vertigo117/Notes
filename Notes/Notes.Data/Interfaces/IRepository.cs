using System.Collections.Generic;

namespace Notes.Data.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Получить экземпляр сущности с указанным уникальным идентификатором
        /// </summary>
        /// <param name="id">Уникальный идентификатор</param>
        /// <returns>Экземпляр сущности с указанным уникальным идентификатором</returns>
        TEntity Get(object id);

        /// <summary>
        /// Получить все экземпляры сущности
        /// </summary>
        /// <returns>Коллекция экземпляров сущности указанного типа</returns>
        IEnumerable<TEntity> Get();

        /// <summary>
        /// Обновить экземпляр сущности
        /// </summary>
        /// <param name="entity">Экземпляр сущности</param>
        void Update(TEntity entity);

        /// <summary>
        /// Создать новый экземпляр сущности
        /// </summary>
        /// <param name="entity">Экземпляр сущности</param>
        void Add(TEntity entity);

        /// <summary>
        /// Удалить экземпляр сущности
        /// </summary>
        /// <param name="entity">Экземпляр сущности</param>
        void Remove(TEntity entity);
    }
}
