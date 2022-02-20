using AutoMapper;
using Notes.Core.Contracts;
using Notes.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Notes.Core.MappingProfiles
{
    public class NoteProfile : Profile
    {
        public NoteProfile()
        {
            CreateMap<Note, NoteDto>();
            CreateMap<NoteUpsertDto, Note>();
        }
    }
}
