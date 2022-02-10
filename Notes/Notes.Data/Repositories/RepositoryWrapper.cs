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
        private IRepository<User> users;
        private IRepository<Note> notes;
        private readonly NotesContext context;

        public RepositoryWrapper(NotesContext context)
        {
            this.context = context;
        }

        public IRepository<User> Users
        { 
            get
            {
                users ??= new Repository<User>(context);
                return users;
            }
        }

        public IRepository<Note> Notes
        {
            get
            {
                notes ??= new Repository<Note>(context);
                return notes;
            }
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
