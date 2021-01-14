using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeePassShtokal.AppCore.DTOs;
using KeePassShtokal.AppCore.Helpers;
using KeePassShtokal.Infrastructure;
using KeePassShtokal.Infrastructure.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace KeePassShtokal.AppCore.Services
{
    public class EntryService:IEntryService
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IMemoryCache _memoryCache;
        public EntryService(MainDbContext mainDbContext, IMemoryCache memoryCache)
        {
            _mainDbContext = mainDbContext;
            _memoryCache = memoryCache;
        }

        public async Task<Status> AddEntry(AddEntryDto addEntryDto, int userId)
        {
            var user =  _mainDbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return new Status(false, "User not exist");
            }
            
            var passwordE = SymmetricEncryptor.EncryptString(addEntryDto.PasswordDecrypted, user.PasswordHash);

            var newEntry = new Entry
            {
                Username = addEntryDto.Username,
                PasswordE = passwordE,
                Description = addEntryDto.Description,
                Email = addEntryDto.Email,
                WebAddress = addEntryDto.WebAddress
            };

            try
            {
                await _mainDbContext.AddAsync(newEntry);
                await _mainDbContext.SaveChangesAsync();
                return new Status
                {
                    Success = true,
                    Message = "Added new password"
                };

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new Status
                {
                    Success = false,
                    Message = "Something went wrong"
                };
            }

        }
    }
}
