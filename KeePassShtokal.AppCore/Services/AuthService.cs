using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using KeePassShtokal.AppCore.DTOs;
using KeePassShtokal.AppCore.Helpers;
using KeePassShtokal.AppCore.Helpers.Extensions;
using KeePassShtokal.Infrastructure;
using KeePassShtokal.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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
            var ipAddress= await _mainDbContext.IdAddresses.FirstOrDefaultAsync(u => u.IpAddressString == loginDto.IpAddress);
            var user = await _mainDbContext.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (ipAddress?.BlockedTo > DateTime.Now|| user?.BlockedTo > DateTime.Now)
            {
                var blockMessage = ipAddress != null ? ipAddress.GenerateBlockMessage()+"\n" : string.Empty;
                blockMessage += user!=null?user.GenerateBlockMessage()+ "\n" : string.Empty;
                return new Status(false, blockMessage);
            }
            if(user==null) return new Status(false, $"Wrong username or password.");
            var isCorrectLoginData = IsPasswordCorrect(user, loginDto);

            ipAddress ??= new IpAddress
            {
                IpAddressString = loginDto.IpAddress
            };

            var attempt = GenerateLoginAttempt(user, ipAddress, isCorrectLoginData);
            try
            {
                await _mainDbContext.AddAsync(attempt);
                await _mainDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new Status(false, "Something went wrong");
            }

            if (!isCorrectLoginData)
            {
                var blockMessage = ipAddress.GenerateBlockMessage() + "\n";
                blockMessage += user.GenerateBlockMessage();
                return new Status(false, $"Wrong username or password. \n {blockMessage}");
            }
            var jwToken = TokenHelper.GetToken(user);
            return new Status(true, "Bearer "+ jwToken);
        }

        public async Task<Status> ChangePassword(ChangePasswordDto changePasswordDto, int userId)
        {
            var user = await _mainDbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                return new Status(false, "Something went wrong");
            }

            var passwordHash = PrepareHashPassword(changePasswordDto.OldPassword, user.Salt, user.IsPasswordKeptAsSha);
            if (passwordHash != user.PasswordHash)
            {
                return new Status(false, "Wrong old password");
            }

            try
            {
                var memoryCacheKey = $"Password for {user.Username}";

                var newPasswordHash = UpdateUserPassword(changePasswordDto.NewPassword, changePasswordDto.IsPasswordKeptAsHash, user);
                
                await UpdateUserWallet(memoryCacheKey, user.UserId, newPasswordHash);

                await _mainDbContext.SaveChangesAsync();
                _memoryCache.Set(memoryCacheKey, newPasswordHash, DateTime.Now.AddMinutes(60));

                return new Status(true, "Successfully password change");
            }
            catch
            {
                return new Status(false, "Something went wrong");
            }
        }

        //todo after entities bug fix
        public async Task<IEnumerable<BlockedIpDto>> GetBlockedIps(int userId)
        {
            var blockedIps= await _mainDbContext.IdAddresses
                .Where(x => x.BlockedTo > DateTime.Now)
                .Select(x => new BlockedIpDto
                {
                    AddressId = x.AddressId,
                    IpAddressString = x.IpAddressString,
                    BlockedTo = x.BlockedTo,
                    IncorrectLoginCount = x.IncorrectLoginCount
                }).ToListAsync();

            return blockedIps;
        }
        //todo after entities bug fix
        public async Task<Status> UnBlockIp(int userId, int ipId)
        {
           var ip=await _mainDbContext.IdAddresses.FirstOrDefaultAsync(x => x.AddressId == ipId);
           if(ip==null) return new Status(false, "Ip Not Found");
           ip.ResetIncorrectAttempts();

           try
           {
               _mainDbContext.Update(ip);
               await _mainDbContext.SaveChangesAsync();
               return new Status(true, "Ip successfully unblocked");
           }
           catch (Exception e)
           {
               Console.WriteLine(e);
               return new Status(false, "Something went wrong");
           }
           

        }

        private string PrepareHashPassword(string password, string salt, bool isKeptAsHash)
        {
            var passwordForHash = isKeptAsHash ?
                $"{Pepper}{salt}{password}"
                : $"{salt}{password}";

            var passwordHash = isKeptAsHash ?
                HashHelper.Sha512(passwordForHash) :
                HashHelper.HmacSha512(passwordForHash, Pepper);

            return passwordHash;
        }

        private bool IsPasswordCorrect(User user, LoginDto loginDto)

        {
            var passwordHash = PrepareHashPassword(loginDto.Password, user.Salt, user.IsPasswordKeptAsSha);

            if (passwordHash != user.PasswordHash)
            {
                return false;
            }
            _memoryCache.GetOrCreate($"Password for {loginDto.Username}", (x) =>
            {
                x.AbsoluteExpiration = DateTime.UtcNow.AddMinutes(60);
                x.Value = passwordHash;

                return passwordHash;
            });
            return true;
        }

        private string UpdateUserPassword(string newPassword, bool isPasswordKept, User user)
        {
            var newSalt = Guid.NewGuid().ToString();
            var newPasswordHash = PrepareHashPassword(newPassword, newSalt, isPasswordKept);

            user.Salt = newSalt;
            user.PasswordHash = newPasswordHash;
            user.IsPasswordKeptAsSha = isPasswordKept;

            _mainDbContext.Update(user);

            return newPasswordHash;
        }

        private async Task UpdateUserWallet(string memoryCacheKey, int userId, string newPasswordHash)
        {
            _memoryCache.TryGetValue(memoryCacheKey, out string rememberPasswordHash);

            if (rememberPasswordHash == null)
            {
                return;
            }

            var entries= await GetAllUserEntriesQuery(userId).ToListAsync();

            entries.ForEach(entry =>
            {
                var oldDecryptedPasswordInWallet = SymmetricEncryptor.DecryptToString(entry.PasswordE, rememberPasswordHash);
                entry.PasswordE = SymmetricEncryptor.EncryptString(oldDecryptedPasswordInWallet, newPasswordHash);
            });
        }

        public IQueryable<Entry> GetAllUserEntriesQuery(int userId)
        {
            var entries =
                from e in _mainDbContext.Entries
                join ue in
                    (from usersEntry in _mainDbContext.UsersEntries
                          where usersEntry.UserId == userId && usersEntry.IsUserOwner == true
                          select usersEntry.EntryId)
                    on e.EntryId equals ue
                select e;

            return entries;


        }

        private LoginAttempt GenerateLoginAttempt(User user, IpAddress ipAddress, bool isSuccessful)
        {
            if (!isSuccessful)
            {
                ipAddress.IncreaseIncorrectAttempts();
                user?.IncreaseIncorrectAttempts();
            }
            else
            {
                ipAddress.ResetIncorrectAttempts();
                user.ResetIncorrectAttempts();
            }

            var loginAttempt=new LoginAttempt
            {
                User=user,
                IpAddress = ipAddress,
                DateTime = DateTime.Now,
                IsSuccessful = isSuccessful
            };
            return loginAttempt;
        }
    }
}
