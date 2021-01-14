using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeePassShtokal.AppCore.DTOs;
using KeePassShtokal.AppCore.Helpers;
using KeePassShtokal.Infrastructure;
using KeePassShtokal.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
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

            var newUserEntry = new UsersEntries
            {
                IsUserOwner = true,
                Entry = newEntry,
                User = user
            };

            

            try
            {
                await _mainDbContext.AddAsync(newUserEntry);
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

        public async Task<Status> EditEntry(EditEntryDto editEntryDto, int userId)
        {
            var owner = await _mainDbContext.Users.FirstOrDefaultAsync(user => user.UserId == userId);
            if (owner == null)
            {
                return new Status(false, "User owner not found");
            }

            var userEntry =
                await _mainDbContext.UsersEntries.Include(p => p.Entry).FirstOrDefaultAsync(x =>
                    x.EntryId == editEntryDto.EntryId && x.UserId == userId);
            if (userEntry == null)
            {
                return new Status(false, "Entry not found");
            }

            if(!userEntry.IsUserOwner) return new Status(false, "You cannot edit shared for you entry");

            try
            {
                var entryToEdit = userEntry.Entry;

                if (!string.IsNullOrEmpty(editEntryDto.PasswordDecrypted))
                {
                    entryToEdit.PasswordE = SymmetricEncryptor.EncryptString(editEntryDto.PasswordDecrypted, owner.PasswordHash);
                }

                entryToEdit.Username = editEntryDto.Username;
                entryToEdit.Description = editEntryDto.Description;
                entryToEdit.Email = editEntryDto.Email;
                entryToEdit.WebAddress = editEntryDto.WebAddress;


                _mainDbContext.Update(entryToEdit);
                await _mainDbContext.SaveChangesAsync();
                return new Status(true, "Password has been successfully edited");
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
