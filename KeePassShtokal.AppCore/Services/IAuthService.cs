﻿using System.Collections.Generic;
using System.Threading.Tasks;
using KeePassShtokal.AppCore.DTOs.Auth;
using KeePassShtokal.AppCore.Helpers;

namespace KeePassShtokal.AppCore.Services
{
    public interface IAuthService
    {
        Task<Status> Register(RegistrationDto registerDto);
        Task<Status> Login(LoginDto loginDto);

        Task<Status> ChangePassword(ChangePasswordDto changePasswordDto, int userId);

        Task<IEnumerable<BlockedIpDto>> GetBlockedIps(int userId);
        Task<Status> UnBlockIp(int userId, int ip);

        Task<UserLoginInfoDto> GetUserLoginInfo(int userId);
        //Task<Status> ChangePassword(ChangePasswordDto changePasswordDto);
        //string PreapreHashPassword(string password, string salt, bool isKeptAsHash);
        //Task<AuthInfoDto> GetAuthInfo(string username);
        //Task<Status> UnbanIpAddress(string ipAddress);
    }
}
