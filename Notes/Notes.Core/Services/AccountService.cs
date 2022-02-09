using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Notes.Core.Contracts;
using Notes.Core.Interfaces;
using Notes.Data.Entities;
using Notes.Data.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Notes.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork repository;
        private readonly IEncryptionService encryptionService;
        private readonly IMapper mapper;

        public AccountService(IUnitOfWork repository, IEncryptionService encryptionService, IMapper mapper)
        {
            this.repository = repository;
            this.encryptionService = encryptionService;
            this.mapper = mapper;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request, HttpContext httpContext)
        {
            User user = repository.Users.Get(request.Email);

            if (user == null)
            {
                return new LoginResponse("Пользователя с указанным адресом электронной почты не существует");
            }

            if (!encryptionService.ValidatePassword(request.Password, user.Password))
            {
                return new LoginResponse("Пароль введён неверно");
            }

            await SignInAsync(httpContext, user);

            return new LoginResponse();
        }

        private static async Task SignInAsync(HttpContext httpContext, User user)
        {
            var claims = new List<Claim> { new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email) };

            var claimsIdentity = new ClaimsIdentity(
                claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            if (repository.Users.Get(request.Email) != null)
            {
                return new RegistrationResponse("Пользователь с указанным адресом электронной почты уже существует");
            }

            var user = mapper.Map<User>(request);

            user.Password = encryptionService.HashPassword(user.Password);

            repository.Users.Add(user);
            await repository.SaveChangesAsync();

            return new RegistrationResponse();
        }

        public async Task LogoutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
