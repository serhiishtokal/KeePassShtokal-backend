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

namespace KeePassShtokal.AppCore.Services
{
    public class AuthService:IAuthService
    {
        private readonly MainDbContext _mainDbContext;
        private const string Pepper = "fjdkRB%(^!re";

        public AuthService(MainDbContext passwordWalletContext)
        {
            _mainDbContext = passwordWalletContext;
        }

        public async Task<Status> Register(RegisterDto registerDto)
        {
            var user = _mainDbContext.Users.Any(u => u.Username == registerDto.Username);
            if (user)
            {
                var status= new Status
                {
                    Success = false,
                    Message = $"User with login {registerDto.Username} exist"
                };
                return status;
            }

            var salt = Guid.NewGuid().ToString();
            var passwordHash = PrepareHashPassword(registerDto.Password, salt, registerDto.IsPasswordKeptAsHash);

            var newUser = new User
            {
                Username = registerDto.Username,
                PasswordHash = passwordHash,
                Salt = salt,
                IsPasswordKeptAsSha = registerDto.IsPasswordKeptAsHash
            };

             await _mainDbContext.AddAsync(newUser);
             await _mainDbContext.SaveChangesAsync();

            return new Status(true, "You've successfully signed up");
        }

        public Task<Status> Login(LoginDto loginDto)
        {
            throw new NotImplementedException();
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


    }
}
