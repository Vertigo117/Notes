using AutoFixture.Xunit2;
using AutoMapper;
using Moq;
using Notes.Core.Contracts;
using Notes.Core.Interfaces;
using Notes.Core.Models;
using Notes.Core.Services;
using Notes.Core.Tests.Attributes;
using Notes.Data.Entities;
using Notes.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Notes.Core.Tests
{
    public class AccountServiceTests
    {
        [Theory]
        [AutoMoqData]
        public async Task LoginAsync_RepositoryGetAsyncMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            AccountService accountService,
            UserLoginDto credentialsDto,
            User user)
        {
            //Arrange
            unitOfWorkMock.Setup(unitOfWork => unitOfWork.Users.GetAsync(It.IsAny<object>())).ReturnsAsync(user);

            //Act
            await accountService.LoginAsync(credentialsDto);

            //Assert
            unitOfWorkMock.Verify(repo => repo.Users.GetAsync(It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoginAsync_UserNotFound_ReturnsUnsuccessfulResult(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            AccountService accountService,
            UserLoginDto credentialsDto)
        {
            //Arrange
            User user = null;
            unitOfWorkMock.Setup(unitOfWork => unitOfWork.Users.GetAsync(It.IsAny<object>())).ReturnsAsync(user);

            //Act
            Result<TokenDto> actual = await accountService.LoginAsync(credentialsDto);

            //Assert
            Assert.False(actual.IsSuccess);
            Assert.Equal("Пользователя с таким адресом электронной почты не существует", actual.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoginAsync_UserFound_EncryptionServiceValidatePasswordCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] Mock<IEncryptionService> encryptionService,
            AccountService accountService,
            UserLoginDto credentialsDto,
            User user)
        {
            //Arrange
            unitOfWorkMock.Setup(unitOfWork => unitOfWork.Users.GetAsync(It.IsAny<object>())).ReturnsAsync(user);
            encryptionService
                .Setup(service => service.ValidatePassword(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            //Act
            await accountService.LoginAsync(credentialsDto);

            //Assert
            encryptionService.Verify(service =>
                service.ValidatePassword(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoginAsync_InvorrectPassword_ReturnsUnsuccessfulResult(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] Mock<IEncryptionService> encryptionService,
            AccountService accountService,
            UserLoginDto credentialsDto,
            User user)
        {
            //Arrange
            unitOfWorkMock.Setup(unitOfWork => unitOfWork.Users.GetAsync(It.IsAny<object>())).ReturnsAsync(user);
            encryptionService
                .Setup(service => service.ValidatePassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            //Act
            Result<TokenDto> actual = await accountService.LoginAsync(credentialsDto);

            //Assert
            Assert.False(actual.IsSuccess);
            Assert.Equal("Пароль введён неверно", actual.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoginAsync_CorrectPassword_JwtServiceGenerateMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] Mock<IEncryptionService> encryptionServiceMock,
            [Frozen] Mock<IJwtService> jwtServiceMock,
            AccountService accountService,
            UserLoginDto credentialsDto,
            User user,
            string jwt)
        {
            //Arrange
            unitOfWorkMock.Setup(unitOfWork => unitOfWork.Users.GetAsync(It.IsAny<object>())).ReturnsAsync(user);
            encryptionServiceMock
                .Setup(service => service.ValidatePassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            jwtServiceMock.Setup(service => service.Generate(It.IsAny<string>(), It.IsAny<string>())).Returns(jwt);


            //Act
            await accountService.LoginAsync(credentialsDto);

            //Assert
            jwtServiceMock.Verify(service => service.Generate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoginAsync_CorrectPassword_SuccessfulResultReturned(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] Mock<IEncryptionService> encryptionServiceMock,
            AccountService accountService,
            UserLoginDto credentialsDto,
            User user)
        {
            //Arrange
            unitOfWorkMock.Setup(unitOfWork => unitOfWork.Users.GetAsync(It.IsAny<object>())).ReturnsAsync(user);
            encryptionServiceMock
                .Setup(service => service.ValidatePassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            //Act
            Result<TokenDto> actual = await accountService.LoginAsync(credentialsDto);

            //Assert
            Assert.True(actual.IsSuccess);
            Assert.NotNull(actual.Data);
        }

        [Theory]
        [AutoMoqData]
        public async Task RegisterAsync_RepositoryGetAsyncMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            AccountService accountService,
            UserUpsertDto userDto,
            IEnumerable<User> users)
        {
            //Arrange
            unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.Users.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(users);

            //Act
            await accountService.RegisterAsync(userDto);

            //Assert
            unitOfWorkMock
                .Verify(unitOfWork => unitOfWork.Users.GetAsync(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task RegisterAsync_ExistingUserPassed_ReturnsUnsuccessfulResult(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            AccountService accountService,
            UserUpsertDto userDto,
            IEnumerable<User> users)
        {
            //Arrange
            unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.Users.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(users);

            //Act
            Result<UserDto> actual = await accountService.RegisterAsync(userDto);

            //Assert
            Assert.NotNull(actual);
            Assert.False(actual.IsSuccess);
            Assert.Equal("Пользователь с таким адресом электронной почты уже существует", actual.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task RegisterAsync_PasswordConfirmationMismatch_ReturnsUnsuccessfulResult(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] UserUpsertDto userDto,
            AccountService accountService,
            User user)
        {
            //Arrange
            unitOfWorkMock.Setup(mock => mock.Users.GetAsync(It.IsAny<string>())).ReturnsAsync(user);

            //Act
            Result<UserDto> actual = await accountService.RegisterAsync(userDto);

            //Assert
            Assert.NotNull(actual);
            Assert.False(actual.IsSuccess);
            Assert.Equal("Пароль и его подтверждение должны совпадать", actual.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task RegisterAsync_NonExistingUserPassed_MapperMapUpsertDtoMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] Mock<IMapper> mapperMock,
            [Frozen] UserUpsertDto userDto,
            AccountService accountService,
            User userEntity)
        {
            //Arrange
            unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.Users.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());
            mapperMock.Setup(mapper => mapper.Map<User>(It.IsAny<UserUpsertDto>())).Returns(userEntity);
            userDto.ConfirmPassword = userDto.Password;

            //Act
            await accountService.RegisterAsync(userDto);

            //Assert
            mapperMock.Verify(mapper => mapper.Map<User>(It.IsAny<UserUpsertDto>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task RegisterAsync_NonExistingUserPassed_EncryptionServiceHashPasswordCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] Mock<IMapper> mapperMock,
            [Frozen] Mock<IEncryptionService> encryptionServiceMock,
            [Frozen] UserUpsertDto userDto,
            AccountService accountService,
            User user,
            string passwordHash)
        {
            //Arrange
            unitOfWorkMock
                .Setup(unitOfwork => unitOfwork.Users.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());
            mapperMock.Setup(mapper => mapper.Map<User>(It.IsAny<UserUpsertDto>())).Returns(user);
            encryptionServiceMock
                .Setup(encryptionService => encryptionService.HashPassword(It.IsAny<string>()))
                .Returns(passwordHash);
            userDto.ConfirmPassword = userDto.Password;

            //Act
            await accountService.RegisterAsync(userDto);

            //Assert
            encryptionServiceMock
                .Verify(encryptionService => encryptionService.HashPassword(It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task RegisterAsync_NonExistingUserPassed_UnitOfWorkSaveAsyncMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] Mock<IMapper> mapperMock,
            [Frozen] Mock<IEncryptionService> encryptionServiceMock,
            [Frozen] UserUpsertDto userDto,
            AccountService accountService,
            User user,
            string passwordHash)
        {
            //Arrange
            unitOfWorkMock
                .Setup(unitOfwork => unitOfwork.Users.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());
            mapperMock.Setup(mapper => mapper.Map<User>(It.IsAny<UserUpsertDto>())).Returns(user);
            encryptionServiceMock
                .Setup(encryptionService => encryptionService.HashPassword(It.IsAny<string>()))
                .Returns(passwordHash);
            userDto.ConfirmPassword = userDto.Password;

            //Act
            await accountService.RegisterAsync(userDto);

            //Assert
            unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task RegisterAsync_NonExistingUserPassed_UnitOfWorkUsersAddMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] Mock<IMapper> mapperMock,
            [Frozen] Mock<IEncryptionService> encryptionServiceMock,
            [Frozen] UserUpsertDto userDto,
            AccountService accountService,
            User userEntity,
            string passwordHash)
        {
            //Arrange
            unitOfWorkMock
                .Setup(unitOfwork => unitOfwork.Users.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());
            mapperMock.Setup(mapper => mapper.Map<User>(It.IsAny<UserUpsertDto>())).Returns(userEntity);
            encryptionServiceMock
                .Setup(encryptionService => encryptionService.HashPassword(It.IsAny<string>()))
                .Returns(passwordHash);
            userDto.ConfirmPassword = userDto.Password;

            //Act
            await accountService.RegisterAsync(userDto);

            //Assert
            unitOfWorkMock.Verify(unitOfWork => unitOfWork.Users.Add(It.IsAny<User>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task RegisterAsync_NonExistingUserPassed_MapperMapUserMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] Mock<IMapper> mapperMock,
            [Frozen] Mock<IEncryptionService> encryptionServiceMock,
            [Frozen] UserUpsertDto userDto,
            AccountService accountService,
            User userEntity,
            UserDto result,
            string passwordHash)
        {
            //Arrange
            unitOfWorkMock
                .Setup(unitOfwork => unitOfwork.Users.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());
            mapperMock.Setup(mapper => mapper.Map<User>(It.IsAny<UserUpsertDto>())).Returns(userEntity);
            mapperMock.Setup(mapper => mapper.Map<UserDto>(It.IsAny<User>())).Returns(result);
            encryptionServiceMock
                .Setup(encryptionService => encryptionService.HashPassword(It.IsAny<string>()))
                .Returns(passwordHash);
            userDto.ConfirmPassword = userDto.Password;

            //Act
            await accountService.RegisterAsync(userDto);

            //Assert
            mapperMock.Verify(mapper => mapper.Map<UserDto>(It.IsAny<User>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteAsync_RepositoryUsersGetAsyncMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            AccountService accountService,
            User user)
        {
            //Arrange
            unitOfWorkMock.Setup(mock => mock.Users.GetAsync(It.IsAny<string>())).ReturnsAsync(user);

            //Act
            await accountService.DeleteAsync(user.Email);

            //Assert
            unitOfWorkMock.Verify(mock => mock.Users.GetAsync(It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteAsync_ExistingEmailPassed_RepositoryUsersRemoveMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            AccountService accountService,
            User user)
        {
            //Arrange
            unitOfWorkMock.Setup(mock => mock.Users.GetAsync(It.IsAny<string>())).ReturnsAsync(user);

            //Act
            await accountService.DeleteAsync(user.Email);

            //Assert
            unitOfWorkMock.Verify(mock => mock.Users.Remove(It.IsAny<User>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteAsync_ExistingEmailPassed_RepositorySaveChangesAsyncMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            AccountService accountService,
            User user)
        {
            //Arrange
            unitOfWorkMock.Setup(mock => mock.Users.GetAsync(It.IsAny<string>())).ReturnsAsync(user);

            //Act
            await accountService.DeleteAsync(user.Email);

            //Assert
            unitOfWorkMock.Verify(mock => mock.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteAsync_NonExistingEmailPassed_RepositorySaveChangesAsyncMethodNotCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            AccountService accountService,
            string email)
        {
            //Arrange
            User user = null;
            unitOfWorkMock.Setup(mock => mock.Users.GetAsync(It.IsAny<string>())).ReturnsAsync(user);

            //Act
            await accountService.DeleteAsync(email);

            //Assert
            unitOfWorkMock.Verify(mock => mock.SaveChangesAsync(), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteAsync_NonExistingUserIdPassed_RepositoryUsersRemoveMethodNotCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            AccountService accountService,
            string email)
        {
            //Arrange
            User user = null;
            unitOfWorkMock.Setup(mock => mock.Users.GetAsync(It.IsAny<string>())).ReturnsAsync(user);

            //Act
            await accountService.DeleteAsync(email);

            //Assert
            unitOfWorkMock.Verify(mock => mock.Users.Remove(It.IsAny<User>()), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetAsync_RepositoryUsersGetAsyncMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            IEnumerable<User> users,
            AccountService accountService)
        {
            //Arrange
            unitOfWorkMock.Setup(mock => mock.Users.GetAsync()).ReturnsAsync(users);

            //Act
            await accountService.GetAsync();

            //Assert
            unitOfWorkMock.Verify(mock => mock.Users.GetAsync(), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetAsync_MapperMapMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitofWorkMock,
            [Frozen] Mock<IMapper> mapperMock,
            IEnumerable<User> users,
            IEnumerable<UserDto> userDtos,
            AccountService accountService)
        {
            //Arrange
            unitofWorkMock.Setup(mock => mock.Users.GetAsync()).ReturnsAsync(users);
            mapperMock.Setup(mock => mock.Map<IEnumerable<UserDto>>(It.IsAny<IEnumerable<User>>())).Returns(userDtos);

            //Act
            await accountService.GetAsync();

            //Assert
            mapperMock.Verify(mock => mock.Map<IEnumerable<UserDto>>(It.IsAny<IEnumerable<User>>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetAsync_UserDtoCollectionReturned(
            [Frozen] Mock<IUnitOfWork> unitofWorkMock,
            [Frozen] Mock<IMapper> mapperMock,
            IEnumerable<User> users,
            IEnumerable<UserDto> userDtos,
            AccountService accountService)
        {
            //Arrange
            unitofWorkMock.Setup(mock => mock.Users.GetAsync()).ReturnsAsync(users);
            mapperMock.Setup(mock => mock.Map<IEnumerable<UserDto>>(It.IsAny<IEnumerable<User>>())).Returns(userDtos);

            //Act
            IEnumerable<UserDto> expected = await accountService.GetAsync();

            //Assert
            Assert.NotNull(expected);
            Assert.IsType<List<UserDto>>(expected.ToList());
        }
    }
}
