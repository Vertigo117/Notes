using AutoMapper;
using Notes.Core.Contracts;
using Notes.Data.Entities;

namespace Notes.Core.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserUpsertDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<User, TokenDto>();
        }
    }
}
