using System;
using KeePassShtokal.Infrastructure.Entities;

namespace KeePassShtokal.AppCore.Helpers.Extensions
{
    public static class IpAddressEntityExtension
    {
        public static void IncreaseIncorrectAttempts(this IpAddress ipAddress)
        {
            ipAddress.IncorrectLoginCount++;
            DateTime? blockToDate = ipAddress.IncorrectLoginCount switch
            {
                { } n when n >= 4 => DateTime.MaxValue,
                { } n when n >= 3 => DateTime.Now.AddSeconds(10),
                { } n when n >= 2 => DateTime.Now.AddSeconds(5),
                _ => null
            };
            ipAddress.BlockedTo = blockToDate;
        }

        public static void ResetIncorrectAttempts(this IpAddress ipAddress)
        {
            ipAddress.BlockedTo = null;
            ipAddress.IncorrectLoginCount = 0;
        }

        public static string GenerateBlockMessage(this IpAddress ipAddress)
        {
            var blockMessage = ipAddress.IncorrectLoginCount switch
            {
                { } n when n >= 4 => "Your Ip permanently blocked",
                { } n when n >= 3 => "Your Ip blocked for 10 seconds",
                { } n when n >= 2 => "Your Ip blocked for 5 seconds",
                _ => ""
            };
            return blockMessage;
        }
    }
}
