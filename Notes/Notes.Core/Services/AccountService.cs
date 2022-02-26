using AutoMapper;
using Notes.Core.Contracts;
using Notes.Core.Interfaces;
using Notes.Data.Entities;
using Notes.Data.Interfaces;
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

        public async Task<TokenDto> LoginAsync(UserLoginDto credentials)
        {
            User user = await repository.Users.GetAsync(credentials.Email);

            if (user == null || !encryptionService.ValidatePassword(credentials.Password, user.PasswordHash))
            {
                return null;
            }

            return new TokenDto 
            {
                UserName = user.Name,
                Token = jwtService.Generate(credentials.Email) 
            };
        }

        public async Task<UserDto> RegisterAsync(UserUpsertDto user)
        {
            if (await IsExistingUser(user))
            {
                return null;
            }

            var userEntity = mapper.Map<User>(user);
            userEntity.PasswordHash = encryptionService.HashPassword(user.Password);

            repository.Users.Add(userEntity);
            await repository.SaveChangesAsync();

            return mapper.Map<UserDto>(userEntity);
        }

        private async Task<bool> IsExistingUser(UserUpsertDto user)
        {
            return (await repository.Users.GetAsync(userEntity => userEntity.Email == user.Email)).Any();
        }
    }
}
