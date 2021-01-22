using Microsoft.Extensions.Caching.Memory;
using Moq;
using PocketWallet.Data;
using PocketWallet.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeePassShtokal.UnitTests.Configuration;

namespace PocketWallet.UnitTests.Configuration
{
    public static class MockInjectedServices
    {
        public static Mock<PasswordWalletContext> GetMockDbContext(IQueryable<User> users)
        {
            var usersMock = DbContextMock.CreateDbSetMock(users);
            var dbContextMock = new Mock<PasswordWalletContext>(DbContextMock.DummyOptions);
            dbContextMock.Setup(x => x.Users).Returns(usersMock.Object);

            return dbContextMock;
        }

        public static Mock<PasswordWalletContext> GetMockDbContext(IQueryable<User> users, IQueryable<Password> passwords)
        {
            var usersMock = DbContextMock.CreateDbSetMock(users);
            var passwordMock = DbContextMock.CreateDbSetMock(passwords);
            var dbContextMock = new Mock<PasswordWalletContext>(DbContextMock.DummyOptions);
            dbContextMock.Setup(x => x.Users).Returns(usersMock.Object);
            dbContextMock.Setup(x => x.Passwords).Returns(passwordMock.Object);

            return dbContextMock;
        }

        public static Mock<PasswordWalletContext> GetMockDbContext(IQueryable<User> users, IQueryable<IpAddress> ipAddress)
        {
            var usersMock = DbContextMock.CreateDbSetMock(users);
            var ipAddressMock = DbContextMock.CreateDbSetMock(ipAddress);
            var dbContextMock = new Mock<PasswordWalletContext>(DbContextMock.DummyOptions);
            dbContextMock.Setup(x => x.Users).Returns(usersMock.Object);
            dbContextMock.Setup(x => x.IpAddresses).Returns(ipAddressMock.Object);

            return dbContextMock;
        }

        public static Mock<IMemoryCache> GetMockmemoryCache()
        {
            var memoryCacheMock = new Mock<IMemoryCache>();
            memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

            return memoryCacheMock;
        }

        public static Mock<IMemoryCache> GetMockmemoryCache(object expectedValue)
        {
            var memoryCacheMock = new Mock<IMemoryCache>();
            memoryCacheMock
                .Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue))
                .Returns(true);

            return memoryCacheMock;
        }

    }
}
