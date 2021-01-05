﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KeePassShtokal.AppCore.DTOs;
using KeePassShtokal.AppCore.Helpers;

namespace KeePassShtokal.AppCore.Services
{
    public interface IAuthService
    {
        Task<Status> Register(RegisterDto registerDto);
        Task<Status> Login(LoginDto loginDto);

        //Task<Status> ChangePassword(ChangePasswordDto changePasswordDto);
        //string PreapreHashPassword(string password, string salt, bool isKeptAsHash);
        //Task<AuthInfoDto> GetAuthInfo(string username);
        //Task<Status> UnbanIpAddress(string ipAddress);
    }
}