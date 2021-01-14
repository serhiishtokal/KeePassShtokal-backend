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
    }
}
