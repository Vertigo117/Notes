using AutoMapper;
using Notes.Core.Contracts;
using Notes.Core.MappingProfiles;
using Notes.Core.Tests.Attributes;
using Notes.Data.Entities;
using Xunit;

namespace Notes.Core.Tests
{
    public class MapperTests
    {
        [Theory]
        [AutoMoqData]
        public void Map_UserPassedUserDtoTyped_WorksAsExpected(User user)
        {
            //Arrange
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new UserProfile()));
            var mapper = new Mapper(configuration);

            //Act
            UserDto actual = mapper.Map<UserDto>(user);

            //Assert
            Assert.Equal(actual.Email, user.Email);
            Assert.Equal(actual.Name, user.Name);
        }
    }
}
