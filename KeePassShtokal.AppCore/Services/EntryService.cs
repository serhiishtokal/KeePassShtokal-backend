using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using KeePassShtokal.AppCore.DTOs.Entry;
using KeePassShtokal.AppCore.Helpers;
using KeePassShtokal.Infrastructure;
using KeePassShtokal.Infrastructure.DefaultData;
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
            //todo: add auto mapper
            var newEntry = new Entry
            {
                UserOwnerUsername = user.Username,
            };
            var newEntryState = new EntryState
            {
                Username = addEntryDto.Username,
                PasswordE = passwordE,
                Description = addEntryDto.Description,
                Email = addEntryDto.Email,
                WebAddress = addEntryDto.WebAddress,
                Entry = newEntry,
                IsDeleted = false
            };
            newEntry.CurrentEntryState = newEntryState;
            var newUserEntry = new UsersEntries
            {
                IsUserOwner = true,
                Entry = newEntry,
                User = user
            };
            var entryAction = CreateEntryAction(user, newEntryState, newEntry, ActionTypesEnum.Create);


            try
            {
                await _mainDbContext.AddAsync(newUserEntry);
                await _mainDbContext.AddAsync(entryAction);
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
                await _mainDbContext.UsersEntries.Include(p => p.Entry.CurrentEntryState).FirstOrDefaultAsync(x =>
                    x.EntryId == editEntryDto.EntryId && x.UserId == userId);
            if (userEntry == null)
            {
                return new Status(false, "Entry not found");
            }

            if(!userEntry.IsUserOwner) return new Status(false, "You cannot edit shared for you entry. You have to be an owner for edit.");

            try
            {
                var newEntryState = new EntryState
                {
                    PasswordE = SymmetricEncryptor.EncryptString(editEntryDto.PasswordDecrypted, owner.PasswordHash),
                    Username = editEntryDto.Username,
                    Description = editEntryDto.Description,
                    Email = editEntryDto.Email,
                    WebAddress = editEntryDto.WebAddress,
                    Entry = userEntry.Entry,
                    IsDeleted = false
                };
                userEntry.Entry.CurrentEntryState = newEntryState;
                var entryAction = CreateEntryAction(owner, newEntryState, userEntry.Entry, ActionTypesEnum.Edit);

                await _mainDbContext.AddAsync(entryAction);
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
        public async Task<Status> DeleteEntry(int entryId, int userId)
        {
            var userEntry =
                await _mainDbContext.UsersEntries
                    .Include(x=>x.Entry.CurrentEntryState)
                    .Include(x=>x.User)
                    .FirstOrDefaultAsync(ue =>
                    ue.EntryId == entryId && ue.UserId == userId);

            if(userEntry==null) return new Status(false, $"Cannot find entry with id: {entryId}");

            if (!userEntry.IsUserOwner) return new Status(false, "You cannot delete shared for you entry. You have to be an owner for delete.");

            var entry = userEntry.Entry;
            var currentEntryState = entry.CurrentEntryState;

            var newEntryState = new EntryState
            {
                PasswordE = currentEntryState.PasswordE,
                Username = currentEntryState.Username,
                Description = currentEntryState.Description,
                Email = currentEntryState.Email,
                WebAddress = currentEntryState.WebAddress,
                Entry = entry,
                IsDeleted = true
            };

            entry.CurrentEntryState = newEntryState;

            var entryAction = CreateEntryAction(userEntry.User, newEntryState, entry, ActionTypesEnum.Delete);

            try
            {
                await _mainDbContext.AddAsync(entryAction);
                await _mainDbContext.SaveChangesAsync();
                return new Status
                {
                    Success = true,
                    Message = $"Successfully removed entry with Id={entryId} from entries!"
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

        public async Task<IEnumerable<GetEntryDto>> GetAll(int userId)
        {
            //todo: add auto mapper 
            var userEntries = await _mainDbContext.UsersEntries
                .Include(x => x.Entry.CurrentEntryState)
                .Where(x => x.UserId == userId)
                .Select(x => new GetEntryDto
                {
                    EntryId = x.EntryId,
                    IsOwner = x.IsUserOwner,
                    UserOwnerUsername = x.Entry.UserOwnerUsername,
                    Username = x.Entry.CurrentEntryState.Username,
                    Email = x.Entry.CurrentEntryState.Email,
                    PasswordEncrypted = x.Entry.CurrentEntryState.PasswordE,
                    WebAddress = x.Entry.CurrentEntryState.WebAddress,
                    Description = x.Entry.CurrentEntryState.Description,
                    IsDeleted = x.Entry.CurrentEntryState.IsDeleted
                }).ToListAsync();

            //var getEntryDto = _mapper.Map<GetEntryDto>(userEntries[0]);

            //var getEntryDto = _mapper.Map<List<UsersEntries>, IEnumerable<GetEntryDto>>(userEntries);


            return userEntries;
        }
        public async Task<Status> GetEntryPassword(int userId, int entryId)
        {
            var userEntries = await _mainDbContext.UsersEntries
                .Include(x => x.Entry.CurrentEntryState)
                .Include(x=>x.User)
                .Where(x => x.EntryId == entryId && x.UserId==userId)
                .FirstOrDefaultAsync();
            if(userEntries==null) return  new Status{Success = false, Message = "No entry founded"};

            var userOwnerHash= await _mainDbContext.UsersEntries
                .Include(x => x.User)
                .Where(x => x.EntryId == entryId && x.IsUserOwner)
                .Select(x=>x.User.PasswordHash)
                .FirstOrDefaultAsync();

            var password = SymmetricEncryptor.DecryptToString(userEntries.Entry.CurrentEntryState.PasswordE, userOwnerHash);

            EntryAction newEntryAction = CreateEntryAction(userEntries.User, userEntries.Entry.CurrentEntryState,
                userEntries.Entry, ActionTypesEnum.View);

            try
            {
                await _mainDbContext.AddAsync(newEntryAction);
                await _mainDbContext.SaveChangesAsync();
                return new Status { Success = true, Message = password };
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
        public async Task<Status> ShareEntry(int givingUserId, string receivingUsername, int entryId)
        {
            var userEntry = await _mainDbContext.UsersEntries
                .Include(x=> x.Entry.CurrentEntryState)
                .Include(x=>x.User)
                .FirstOrDefaultAsync(ue => ue.UserId == givingUserId && ue.EntryId==entryId);

            if (userEntry == null)
            {
                return new Status(false, "Entry not found");
            }

            if (!userEntry.IsUserOwner)
            {
                return new Status(false, "You are not entry owner");
            }

            var receivingUser= await _mainDbContext.Users.
                FirstOrDefaultAsync(ue => ue.Username == receivingUsername);

            if (receivingUser == null)
            {
                return new Status(false, $"User with username {receivingUsername} not found");
            }

            if (receivingUser.UserId == givingUserId)
            {
                return new Status(false, $"You cannot share password for yourself");
            }

            UsersEntries usersEntries = new UsersEntries
            {
                CreationDateTime = DateTime.Now,
                EntryId = entryId,
                IsUserOwner = false,
                User = receivingUser
            };

            var newEntryAction = CreateEntryAction(userEntry.User, userEntry.Entry.CurrentEntryState, userEntry.Entry,
                ActionTypesEnum.Share);

            try
            {
                await _mainDbContext.AddAsync(usersEntries);
                await _mainDbContext.AddAsync(newEntryAction);
                await _mainDbContext.SaveChangesAsync();
                return new Status(true, $"The entry successfully shared for user  {receivingUsername}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new Status(false, $"Something went wrong");
            }
        }
        private EntryAction CreateEntryAction(User user, EntryState entryState, Entry entry,
            ActionTypesEnum actionTypesEnum)
        {
            var entryAction = new EntryAction
            {
                ActionType = actionTypesEnum,
                Entry = entry,
                EntryState = entryState,
                User = user,
                DateTime = DateTime.Now,
                IsRestorable = true
            };

            return entryAction;
        }

        public async Task<Status> RestoreState(int actionId, int userId)
        {
            var entryAction =await _mainDbContext.EntryActions
                .Include(x => x.Entry.CurrentEntryState)
                .Include(x => x.EntryState)
                .FirstOrDefaultAsync(x => x.ActionId == actionId);

            if(entryAction==null) return new Status(false, "Action not found");

            if (entryAction.UserId != userId) return new Status(false, "Access denied");

            entryAction.Entry.CurrentEntryState = entryAction.EntryState;

            var newEntryAction = CreateEntryAction(entryAction.User, entryAction.EntryState, entryAction.Entry,
                ActionTypesEnum.UndoAction);

            try
            {
                await _mainDbContext.AddAsync(newEntryAction);
                await _mainDbContext.SaveChangesAsync();
                return new Status(true, $"The state successfully restored");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new Status(false, $"Something went wrong");
            }

        }

        public async Task<IEnumerable<EntryActionStateDto>> GetEntryActions(int entryId, int userId)
        {
            var userEntries = await _mainDbContext.EntryActions
                .Include(x => x.Entry.CurrentEntryState)
                .Include(x=>x.ActionType)
                .Where(x => x.EntryId == entryId &&x.UserId==userId)
                .Select(x => new EntryActionStateDto
                {
                    ActionId = x.ActionId,
                    EntryId = entryId,
                    UserId = userId,
                    DateTime = x.DateTime,
                    ActionType= x.ActionType,
                    Username=x.Entry.CurrentEntryState.Username,
                    Email = x.Entry.CurrentEntryState.Email,
                    PasswordE = x.Entry.CurrentEntryState.PasswordE,
                    WebAddress = x.Entry.CurrentEntryState.WebAddress,
                    Description = x.Entry.CurrentEntryState.Description,
                    IsDeleted = x.Entry.CurrentEntryState.IsDeleted
                }).ToListAsync();

            return userEntries;
        }
    }
}
