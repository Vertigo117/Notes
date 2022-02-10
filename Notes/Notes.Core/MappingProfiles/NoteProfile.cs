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
            CreateMap<Note, GetNoteResponse>();
            CreateMap<CreateNoteRequest, Note>();
            CreateMap<Note, CreateNoteResponse>();
            CreateMap<UpdateNoteRequest, Note>();
            CreateMap<IEnumerable<Note>, GetAllNotesResponse>()
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.ToList()));
        }
    }
}
