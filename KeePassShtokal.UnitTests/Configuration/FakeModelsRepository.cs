using System.Linq;
using System;
using System.Collections.Generic;
using KeePassShtokal.AppCore.Helpers;
using KeePassShtokal.Infrastructure.Entities;

namespace KeePassShtokal.UnitTests.Configuration
{
    public static class FakeModelsRepository
    {
        public const string UserExistLogin = "Login";
        public const string UserNotExistLogin = "Login1";

        public static IQueryable<User> GetFakeUsers(
            int inCorrectLoginCount = 0,
            DateTime successfulLogin = new DateTime(),
            DateTime unSuccessfulLogin = new DateTime(),
            DateTime? blockLoginTo = null)
        {
            var fixture = new Fixture();
            return new List<User>
            {
                fixture.Build<User>()
                    .With(u => u.Login, UserExistLogin)
                    .With(u => u.PasswordHash, HashHelper.Sha512("zdRpf^%f65V(0" + "testSALT" + "Password" ))
                    .With(u => u.Salt, "testSALT")
                    .With(u => u.InCorrectLoginCount, inCorrectLoginCount)
                    .With(u => u.SuccessfulLogin, successfulLogin)
                    .With(u => u.UnSuccessfulLogin, unSuccessfulLogin)
                    .With(u => u.BlockLoginTo, blockLoginTo)
                    .Create(),
            }.AsQueryable();
        }

        public static IQueryable<Password> GetFakePasswords()
        {
            var fixture = new Fixture();
            return new List<Password>
            {
                fixture.Build<Password>()
                    .With(u => u.Login, UserExistLogin)
                    .With(u => u.PasswordValue, HashHelper.SHA512("zdRpf^%f65V(0" + "testSALT" + "Password" ))
                    .With(u => u.UserId, new Guid())
                    .Create(),
            }.AsQueryable();
        }

        public static IQueryable<IpAddress> GetFakeIpAddress(
            string ipAddress = "10.10.10.10",
            int incorrectSignInCount = 0,
            bool isPermanentlyBlocked = false
            )
        {
            var fixture = new Fixture();
            return new List<IpAddress>
            {
                fixture.Build<IpAddress>()
                    .With(u => u.FromIpAddress, ipAddress)
                    .With(u => u.IncorrectSignInCount, incorrectSignInCount)
                    .With(u => u.IsPermanentlyBlocked, isPermanentlyBlocked)
                    .Create(),
            }.AsQueryable();
        }

        public static RegisterModel GetFakeRegisterModel(string login = UserExistLogin)
        {
            return new RegisterModel
            {
                Login = login,
                IsPasswordKeptAsHash = true,
                Password = "Password"
            };
        }

        public static LoginModel GetFakeLoginModel(string login = UserExistLogin, string password = "Password", string ipAddress = "10.10.10.10")
        {
            return new LoginModel
            {
                Login = login,
                Password = password,
                IpAddress = ipAddress
            };
        }

        public static ChangePasswordModel GetFakeChangePasswordModel(string login = UserExistLogin, string oldPassword = "Password", string newPassword = "PasswordNew")
        {
            return new ChangePasswordModel
            {
                Login = login,
                OldPassword = oldPassword,
                NewPassword = newPassword,
                IsPasswordKeptAsHash = true
            };
        }

        public static AddPasswordModel GetFakeAddNewPasswordModel(
            string login = UserExistLogin, 
            string pasword = "Password", 
            string description = "Description", 
            string webPage = "webpage.pl", 
            Guid userId = new Guid())
        {
            return new AddPasswordModel
            {
                Login = login,
                Password = pasword,
                Description = description,
                WebPage = webPage,
                UserId = userId,
            };
        }

        public static User GetFakeUser(
            int inCorrectLoginCount = 0,
            DateTime successfulLogin = new DateTime(),
            DateTime unSuccessfulLogin = new DateTime(),
            DateTime? blockLoginTo = null,
            Guid userId = new Guid(),
            string login = UserExistLogin)
        {
            return new User
            {
                Id = userId,
                Login = login,
                InCorrectLoginCount = inCorrectLoginCount,
                SuccessfulLogin = successfulLogin,
                UnSuccessfulLogin = unSuccessfulLogin,
                BlockLoginTo = blockLoginTo
            };
        }

        public static IpAddress GetFakeIpAdderess(string ipAddress = "10.10.10.10", int inCorrectLoginCount = 0, bool isBlock = false)
        {
            return new IpAddress
            {
                FromIpAddress = ipAddress,
                IncorrectSignInCount = inCorrectLoginCount,
                IsPermanentlyBlocked = isBlock
            };
        }
    }
}
