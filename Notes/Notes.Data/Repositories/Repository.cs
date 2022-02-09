using Microsoft.EntityFrameworkCore;
using Notes.Data.Contexts;
using Notes.Data.Interfaces;
using System.Collections.Generic;

namespace Notes.Data.Repositories
{
    /// <summary>
    /// Обобщённый репозиторий для работы с указанным типом сущностей
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> entities;

        public Repository(NotesContext context)
        {
            entities = context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            entities.Add(entity);
        }

        public void Remove(TEntity entity)
        {
            entities.Remove(entity);
        }

        public TEntity Get(object id)
        {
            return entities.Find(id);
        }

        public IEnumerable<TEntity> Get()
        {
            return entities;
        }

        public void Update(TEntity entity)
        {
            entities.Update(entity);
        }
    }
}
