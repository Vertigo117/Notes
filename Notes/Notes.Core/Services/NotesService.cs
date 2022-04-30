using AutoMapper;
using Notes.Core.Contracts;
using Notes.Core.Interfaces;
using Notes.Data.Entities;
using Notes.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Notes.Core.Services
{
    /// <summary>
    /// Сервис управления заметками
    /// </summary>
    public class NotesService : INotesService
    {
        private readonly IUnitOfWork repository;
        private readonly IMapper mapper;

        public NotesService(IUnitOfWork repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<NoteDto> CreateNoteAsync(NoteUpsertDto noteUpsertDto, string email)
        {
            var note = mapper.Map<Note>(noteUpsertDto);
            note.CreationDate = DateTime.Now;
            note.User = await repository.Users.GetAsync(email);

            repository.Notes.Add(note);
            await repository.SaveChangesAsync();

            return mapper.Map<NoteDto>(note);
        }

        public async Task<PagedNotesDto> GetPagedNotesAsync(string email, int skip, int take)
        {
            Expression<Func<Note, bool>> predicate = note => note.User.Email == email;
            
            int total = await repository.Notes.GetTotalAsync(predicate);
            IEnumerable<Note> entities = await repository.Notes.GetPagedAsync(predicate, skip, take);

            IEnumerable<NoteDto> notes = mapper.Map<IEnumerable<NoteDto>>(entities);
            
            return new PagedNotesDto { Total = total, Notes = notes };
        }

        public async Task<NoteDto> GetNoteAsync(int id)
        {
            Note note = await repository.Notes.GetAsync(id);
            return mapper.Map<NoteDto>(note);
        }

        public async Task DeleteNoteAsync(int id)
        {
            Note note = await repository.Notes.GetAsync(id);
            repository.Notes.Remove(note);
            await repository.SaveChangesAsync();
        }

        public async Task UpdateNoteAsync(int id, NoteUpsertDto noteUpsertDto)
        {
            Note note = await repository.Notes.GetAsync(id);
            mapper.Map(noteUpsertDto, note);

            repository.Notes.Update(note);
            await repository.SaveChangesAsync();
        }
    }
}
