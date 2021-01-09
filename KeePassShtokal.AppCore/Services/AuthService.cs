using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using KeePassShtokal.AppCore.DTOs;
using KeePassShtokal.AppCore.Helpers;
using KeePassShtokal.Infrastructure;
using KeePassShtokal.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace KeePassShtokal.AppCore.Services
{
    public class AuthService:IAuthService
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IMemoryCache _memoryCache;
        private const string Pepper = "fjdkRB%(^!re";

        public AuthService(MainDbContext passwordWalletContext, IMemoryCache memoryCache)
        {
            _mainDbContext = passwordWalletContext;
            _memoryCache = memoryCache;
        }

        public async Task<Status> Register(RegistrationDto registrationDto)
        {
            var user = _mainDbContext.Users.Any(u => u.Username == registrationDto.Username);
            if (user)
            {
                var status= new Status
                {
                    Success = false,
                    Message = $"User with login {registrationDto.Username} exist"
                };
                return status;
            }

            var salt = Guid.NewGuid().ToString();
            var passwordHash = PrepareHashPassword(registrationDto.Password, salt, registrationDto.IsPasswordKeptAsHash);

            var newUser = new User
            {
                Username = registrationDto.Username,
                PasswordHash = passwordHash,
                Salt = salt,
                IsPasswordKeptAsSha = registrationDto.IsPasswordKeptAsHash
            };

             await _mainDbContext.AddAsync(newUser);
             await _mainDbContext.SaveChangesAsync();

            return new Status(true, "You've successfully signed up");
        }

        public async Task<Status> Login(LoginDto loginDto)
        {
            var user = await _mainDbContext.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            var userStatus = CheckUser(user);
            if (!userStatus.Success)
            {
                return userStatus;
            }

            var userPasswordStatus =  CheckUserPassword(user, loginDto);
            return !userPasswordStatus.Success ? userPasswordStatus : new Status(true, TokenHelper.GetToken(user));
            //await UpdateIncorrectSignInCount(loginModel.IpAddress, user, false, true);
        }


        public string PrepareHashPassword(string password, string salt, bool isKeptAsHash)
        {
            var passwordForHash = isKeptAsHash ?
                $"{Pepper}{salt}{password}"
                : $"{salt}{password}";

            var passwordHash = isKeptAsHash ?
                HashHelper.Sha512(passwordForHash) :
                HashHelper.HmacSha512(passwordForHash, Pepper);

            return passwordHash;
        }

        private Status CheckUser(User user)
        {
            if (user == null)
            {
                return new Status(false, "User not found");
            }


            return new Status(true, "");
        }

        private Status CheckUserPassword(User user, LoginDto loginDto)
        {
            const string errorMessage = "Wrong password";
            var passwordHash = PrepareHashPassword(loginDto.Password, user.Salt, user.IsPasswordKeptAsSha);

            if (passwordHash != user.PasswordHash)
            {
                return new Status(false, errorMessage);
            }

            _memoryCache.GetOrCreate($"Password for {loginDto.Username}", (x) =>
            {
                x.AbsoluteExpiration = DateTime.UtcNow.AddMinutes(60);
                x.Value = passwordHash;

                return passwordHash;
            });
            return new Status(true, "");
        }
    }
}
