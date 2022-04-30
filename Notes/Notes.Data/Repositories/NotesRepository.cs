using Microsoft.EntityFrameworkCore;
using Notes.Data.Contexts;
using Notes.Data.Entities;
using Notes.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Notes.Data.Repositories
{
    /// <summary>
    /// Репозиторий заметок
    /// </summary>
    public class NotesRepository : Repository<Note>, INotesRepository
    {
        private readonly NotesContext context;

        public NotesRepository(NotesContext context) : base(context)
        {
            this.context = context;
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<Note>> GetPagedAsync(Expression<Func<Note, bool>> condition, int skip, int take)
        {
            return await context.Notes
                .Where(condition)
                .OrderBy(note => note.CreationDate)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        ///<inheritdoc/>
        public async Task<int> GetTotalAsync(Expression<Func<Note, bool>> condition)
        {
            return await context.Notes.Where(condition).CountAsync();
        }
    }
}
