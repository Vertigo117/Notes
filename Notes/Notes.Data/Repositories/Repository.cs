using Microsoft.EntityFrameworkCore;
using Notes.Data.Contexts;
using Notes.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Notes.Data.Repositories
{
    /// <summary>
    /// Обобщённый репозиторий для работы с указанным типом сущностей
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    public class IRepository<TEntity> : Interfaces.IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> entities;

        public IRepository(NotesContext context)
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

        public async Task<TEntity> GetAsync(object id)
        {
            return await entities.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> condition)
        {
            return await entities.Where(condition).ToListAsync();
        }

        public void Update(TEntity entity)
        {
            entities.Update(entity);
        }
    }
}
