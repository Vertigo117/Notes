using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

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
        /// <returns>Задача, которая содержит результат выполнения асинхронной операции.
        /// Результат операции представляет собой экземпляр сущности типа <typeparamref name="TEntity"/>
        /// с указанным уникальным идентификатором либо <see cref="null"/>, если экземпляра с таким идентификатором
        /// не существует</returns>
        Task<TEntity> GetAsync(object id);

        /// <summary>
        /// Получить все экземпляры сущности, соответствующие условию
        /// </summary>
        /// <param name="condition">Условие для выборки</param>
        /// <returns>Задача, которая содержит результат выполнения асинхронной операции.
        /// Результат представляет собой коллекцию элементов типа <typeparamref name="TEntity"/></returns>
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> condition);

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

        /// <summary>
        /// Получить все экземпляры сущности
        /// </summary>
        /// <returns>Коллекция экземпляров сущности</returns>
        Task<IEnumerable<TEntity>> GetAsync();
    }
}
