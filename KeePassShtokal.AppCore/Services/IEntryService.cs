using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using KeePassShtokal.AppCore.DTOs;
using KeePassShtokal.AppCore.Helpers;

namespace KeePassShtokal.AppCore.Services
{
    public interface IEntryService
    {
        Task<Status> AddEntry(AddEntryDto addPasswordModel, int userId);

        Task<Status> EditEntry(EditEntryDto editEntryDto, int userId);

        Task<Status> DeleteEntry(int entryId, int userId);

        Task<IEnumerable<GetEntryDto>> GetAll(int userId);

        Task<Status> GetEntryPassword(int userId, int entryId);

        Task<Status> ShareEntry(int givingUserId, string receivingUsername, int entryId);
    }
}
