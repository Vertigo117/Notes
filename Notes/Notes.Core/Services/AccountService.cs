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

        public async Task<TokenDto> LoginAsync(UserLoginDto request)
        {
            User user = await repository.Users.GetAsync(request.Email);

            if (user == null || !encryptionService.ValidatePassword(request.Password, user.PasswordHash))
            {
                return null;
            }

            return new TokenDto 
            {
                UserName = user.Name,
                Token = jwtService.Generate(request.Email) 
            };
        }

        public async Task<UserDto> RegisterAsync(UserUpsertDto request)
        {
            if (await IsExistingUser(request.Email))
            {
                return null;
            }

            var user = mapper.Map<User>(request);
            user.PasswordHash = encryptionService.HashPassword(request.Password);

            repository.Users.Add(user);
            await repository.SaveChangesAsync();

            return mapper.Map<UserDto>(user);
        }

        private async Task<bool> IsExistingUser(string email)
        {
            return (await repository.Users.GetAsync(user => user.Email == email)).Any();
        }
    }
}
