using Notes.Data.Contexts;
using Notes.Data.Entities;
using Notes.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Data.Repositories
{
    /// <summary>
    /// Обёртка над репозиториями для гарантии безопасности транзакций
    /// </summary>
    public class RepositoryWrapper : IUnitOfWork
    {
        private readonly IRepository<User> users;
        private readonly INotesRepository notes;
        private readonly NotesContext context;

        public RepositoryWrapper(NotesContext context, IRepository<User> users, INotesRepository notes)
        {
            this.context = context;
            this.users = users;
            this.notes = notes;
        }

        public IRepository<User> Users
        { 
            get
            {
                return users;
            }
        }

        public INotesRepository Notes
        {
            get
            {
                return notes;
            }
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
