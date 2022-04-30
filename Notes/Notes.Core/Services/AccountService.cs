using AutoMapper;
using Notes.Core.Contracts;
using Notes.Core.Interfaces;
using Notes.Core.Models;
using Notes.Data.Entities;
using Notes.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Core.Services
{
    /// <summary>
    /// Сервис для авторизации и регистрации пользователей
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork repository;
        private readonly IEncryptionService encryptionService;
        private readonly IJwtService jwtService;
        private readonly IMapper mapper;

        public AccountService(
            IUnitOfWork repository,
            IEncryptionService encryptionService,
            IJwtService jwtService,
            IMapper mapper)
        {
            this.repository = repository;
            this.encryptionService = encryptionService;
            this.jwtService = jwtService;
            this.mapper = mapper;
        }

        public async Task<Result> LoginAsync(UserLoginDto userLoginDto)
        {
            User user = await repository.Users.GetAsync(userLoginDto.Email);

            if (user is null)
            {
                return new Result { ErrorMessage = "Пользователя с таким адресом электронной почты не существует" };
            }

            if (!ValidatePassword(userLoginDto.Password, user.PasswordHash))
            {
                return new Result { ErrorMessage = "Пароль введён неверно" };
            }

            var jwt = jwtService.Generate(userLoginDto.Email, user.Name);
            var data = new TokenDto { Token = jwt };
            return new Result<TokenDto>(data);
        }

        private bool ValidatePassword(string password, string passwordHash)
        {
            return encryptionService.ValidatePassword(password, passwordHash);
        }

        public async Task<Result> RegisterAsync(UserUpsertDto userUpsertDto)
        {
            if (await IsExistingUser(userUpsertDto))
            {
                return new Result { ErrorMessage = "Пользователь с таким адресом электронной почты уже существует" };
            }

            if (IsPasswordConfirmed(userUpsertDto))
            {
                return new Result { ErrorMessage = "Пароль и его подтверждение должны совпадать" };
            }

            var user = mapper.Map<User>(userUpsertDto);
            user.PasswordHash = encryptionService.HashPassword(userUpsertDto.Password);

            repository.Users.Add(user);
            await repository.SaveChangesAsync();

            var data = mapper.Map<UserDto>(user);
            return new Result<UserDto>(data);
        }

        private static bool IsPasswordConfirmed(UserUpsertDto user)
        {
            return user.Password != user.ConfirmPassword;
        }

        public async Task DeleteAsync(string email)
        {
            User userEntity = await repository.Users.GetAsync(email);

            if (userEntity is null)
            {
                return;
            }

            repository.Users.Remove(userEntity);
            await repository.SaveChangesAsync();
        }

        private async Task<bool> IsExistingUser(UserUpsertDto user)
        {
            return (await repository.Users.GetAsync(userEntity => userEntity.Email == user.Email)).Any();
        }

        public async Task<IEnumerable<UserDto>> GetAsync()
        {
            IEnumerable<User> users = await repository.Users.GetAsync();
            return mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}
