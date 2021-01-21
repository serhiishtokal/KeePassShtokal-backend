using System;
using KeePassShtokal.Infrastructure.Entities;

namespace KeePassShtokal.AppCore.Helpers.Extensions
{
    public static class UserIpAddressEntityExtension
    {
        public static void UpdateAttempts(this UserIpAddress userIpAddress, bool isLoginSuccessful)
        {
            if (isLoginSuccessful)
            {
                userIpAddress.ResetIncorrectAttempts();
            }
            else
            {
                userIpAddress.IncreaseIncorrectAttempts();
            }
        }
        private static void IncreaseIncorrectAttempts(this UserIpAddress userIpAddress)
        {
            userIpAddress.IncorrectLoginCount++;
            DateTime? blockToDate = userIpAddress.IncorrectLoginCount switch
            {
                { } n when n >= 4 => DateTime.MaxValue,
                { } n when n >= 3 => DateTime.Now.AddSeconds(10),
                { } n when n >= 2 => DateTime.Now.AddSeconds(5),
                _ => null
            };
            userIpAddress.BlockedTo = blockToDate;
        }

        private static void ResetIncorrectAttempts(this UserIpAddress userIpAddress)
        {
            userIpAddress.BlockedTo = null;
            userIpAddress.IncorrectLoginCount = 0;
        }

        public static void Unblock(this UserIpAddress userIpAddress)
        {
            userIpAddress.ResetIncorrectAttempts();
        }

        public static string GenerateBlockMessage(this UserIpAddress userIpAddress)
        {
            var blockMessage = userIpAddress.IncorrectLoginCount switch
            {
                { } n when n >= 4 => "Your Ip permanently blocked.",
                { } n when n >= 3 => "Your Ip blocked for 10 seconds.",
                { } n when n >= 2 => "Your Ip blocked for 5 seconds.",
                _ => ""
            };
            return blockMessage;
        }
    }
}
