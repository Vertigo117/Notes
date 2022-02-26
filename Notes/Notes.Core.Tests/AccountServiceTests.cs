using AutoFixture.Xunit2;
using AutoMapper;
using Moq;
using Notes.Core.Contracts;
using Notes.Core.Interfaces;
using Notes.Core.Services;
using Notes.Core.Tests.Attributes;
using Notes.Data.Entities;
using Notes.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Notes.Core.Tests
{
    public class AccountServiceTests
    {
        [Theory]
        [AutoMoqData]
        public async Task LoginAsync_ProperCredentialsPassed_RepositoryGetAsyncMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            AccountService accountService,
            UserLoginDto credentials,
            User user)
        {
            //Arrange
            unitOfWorkMock.Setup(unitOfWork => unitOfWork.Users.GetAsync(It.IsAny<object>())).ReturnsAsync(user);

            //Act
            await accountService.LoginAsync(credentials);

            //Assert
            unitOfWorkMock.Verify(repo => repo.Users.GetAsync(It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoginAsync_UserNotFound_ReturnsNull(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            AccountService accountService,
            UserLoginDto credentials)
        {
            //Arrange
            User user = null;
            unitOfWorkMock.Setup(unitOfWork => unitOfWork.Users.GetAsync(It.IsAny<object>())).ReturnsAsync(user);

            //Act
            TokenDto actual = await accountService.LoginAsync(credentials);

            //Assert
            Assert.Null(actual);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoginAsync_UserFound_EncryptionServiceValidatePasswordCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] Mock<IEncryptionService> encryptionService,
            AccountService accountService,
            UserLoginDto credentials,
            User user)
        {
            //Arrange
            unitOfWorkMock.Setup(unitOfWork => unitOfWork.Users.GetAsync(It.IsAny<object>())).ReturnsAsync(user);
            encryptionService
                .Setup(service => service.ValidatePassword(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            //Act
            await accountService.LoginAsync(credentials);

            //Assert
            encryptionService.Verify(service =>
                service.ValidatePassword(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoginAsync_InvorrectPassword_ReturnsNull(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] Mock<IEncryptionService> encryptionService,
            AccountService accountService,
            UserLoginDto credentials,
            User user)
        {
            //Arrange
            unitOfWorkMock.Setup(unitOfWork => unitOfWork.Users.GetAsync(It.IsAny<object>())).ReturnsAsync(user);
            encryptionService
                .Setup(service => service.ValidatePassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            //Act
            TokenDto actual = await accountService.LoginAsync(credentials);

            //Assert
            Assert.Null(actual);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoginAsync_CorrectPassword_JwtServiceGenerateMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] Mock<IEncryptionService> encryptionServiceMock,
            [Frozen] Mock<IJwtService> jwtServiceMock,
            AccountService accountService,
            UserLoginDto credentials,
            User user,
            string jwt)
        {
            //Arrange
            unitOfWorkMock.Setup(unitOfWork => unitOfWork.Users.GetAsync(It.IsAny<object>())).ReturnsAsync(user);
            encryptionServiceMock
                .Setup(service => service.ValidatePassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            jwtServiceMock.Setup(service => service.Generate(It.IsAny<string>())).Returns(jwt);


            //Act
            await accountService.LoginAsync(credentials);

            //Assert
            jwtServiceMock.Verify(service => service.Generate(It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoginAsync_CorrectPassword_ProperTokenDtoReturned(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] Mock<IEncryptionService> encryptionServiceMock,
            AccountService accountService,
            UserLoginDto credentials,
            User user)
        {
            //Arrange
            unitOfWorkMock.Setup(unitOfWork => unitOfWork.Users.GetAsync(It.IsAny<object>())).ReturnsAsync(user);
            encryptionServiceMock
                .Setup(service => service.ValidatePassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            //Act
            TokenDto actual = await accountService.LoginAsync(credentials);

            //Assert
            Assert.NotNull(actual);
            Assert.IsType<TokenDto>(actual);
            Assert.Equal(actual.UserName, user.Name);
        }

        [Theory]
        [AutoMoqData]
        public async Task RegisterAsync_UserPassed_RepositoryGetAsyncMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            AccountService accountService,
            UserUpsertDto user,
            IEnumerable<User> users)
        {
            //Arrange
            unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.Users.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(users);

            //Act
            await accountService.RegisterAsync(user);

            //Assert
            unitOfWorkMock
                .Verify(unitOfWork => unitOfWork.Users.GetAsync(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task RegisterAsync_ExistingUserPassed_ReturnsNull(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            AccountService accountService,
            UserUpsertDto user,
            IEnumerable<User> users)
        {
            //Arrange
            unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.Users.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(users);

            //Act
            UserDto actual = await accountService.RegisterAsync(user);

            //Assert
            Assert.Null(actual);
        }

        [Theory]
        [AutoMoqData]
        public async Task RegisterAsync_NonExistingUserPassed_MapperMapUpsertDtoMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] Mock<IMapper> mapperMock,
            AccountService accountService,
            UserUpsertDto user,
            User userEntity)
        {
            //Arrange
            unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.Users.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());
            mapperMock.Setup(mapper => mapper.Map<User>(It.IsAny<UserUpsertDto>())).Returns(userEntity);

            //Act
            await accountService.RegisterAsync(user);

            //Assert
            mapperMock.Verify(mapper => mapper.Map<User>(It.IsAny<UserUpsertDto>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task RegisterAsync_NonExistingUserPassed_EncryptionServiceHashPasswordCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] Mock<IMapper> mapperMock,
            [Frozen] Mock<IEncryptionService> encryptionServiceMock,
            AccountService accountService,
            UserUpsertDto user,
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

            //Act
            await accountService.RegisterAsync(user);

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
            AccountService accountService,
            UserUpsertDto user,
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

            //Act
            await accountService.RegisterAsync(user);

            //Assert
            unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task RegisterAsync_NonExistingUserPassed_UnitOfWorkUsersAddMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] Mock<IMapper> mapperMock,
            [Frozen] Mock<IEncryptionService> encryptionServiceMock,
            AccountService accountService,
            UserUpsertDto user,
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

            //Act
            await accountService.RegisterAsync(user);

            //Assert
            unitOfWorkMock.Verify(unitOfWork => unitOfWork.Users.Add(It.IsAny<User>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task RegisterAsync_NonExistingUserPassed_MapperMapUserMethodCalled(
            [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
            [Frozen] Mock<IMapper> mapperMock,
            [Frozen] Mock<IEncryptionService> encryptionServiceMock,
            AccountService accountService,
            UserUpsertDto user,
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

            //Act
            await accountService.RegisterAsync(user);

            //Assert
            mapperMock.Verify(mapper => mapper.Map<UserDto>(It.IsAny<User>()), Times.Once);
        }
    }
}
