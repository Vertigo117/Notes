using AutoMapper;
using Notes.Core.Contracts;
using Notes.Core.Interfaces;
using Notes.Data.Entities;
using Notes.Data.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Core.Services
{
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

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            User user = await repository.Users.GetAsync(request.Email);

            if (!encryptionService.ValidatePassword(request.Password, user?.PasswordHash))
            {
                return null;
            }

            return new LoginResponse { Token = jwtService.Generate(request.Email) };
        }

        public async Task<RegistrationResponse> RegisterAsync(RegistrationRequest request)
        {
            if (await IsExistingUser(request.Email))
            {
                return null;
            }

            var user = mapper.Map<User>(request);
            user.PasswordHash = encryptionService.HashPassword(request.Password);

            repository.Users.Add(user);
            await repository.SaveChangesAsync();

            return mapper.Map<RegistrationResponse>(user);
        }

        private async Task<bool> IsExistingUser(string email)
        {
            return (await repository.Users.GetAsync(user => user.Email == email)).Any();
        }
    }
}
