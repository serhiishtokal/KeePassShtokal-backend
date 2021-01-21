using System;
using KeePassShtokal.Infrastructure.Entities;

namespace KeePassShtokal.AppCore.Helpers.Extensions
{
    public static class UserEntityExtension
    {
        public static void UpdateAttempts(this User user, bool isLoginSuccessful)
        {
            if (isLoginSuccessful)
            {
                user.ResetIncorrectAttempts();
            }
            else
            {
                user.IncreaseIncorrectAttempts();
            }
        }
        private static void IncreaseIncorrectAttempts(this User user)
        {
            user.IncorrectLoginCount++;
            DateTime? blockToDate = user.IncorrectLoginCount switch
            {
                { } n when n >= 4 => DateTime.Now.AddMinutes(2),
                { } n when n >= 3 => DateTime.Now.AddSeconds(10),
                { } n when n >= 2 => DateTime.Now.AddSeconds(5),
                _ => null
            };
            user.BlockedTo = blockToDate;
        }

        private static void ResetIncorrectAttempts(this User user)
        {
            user.BlockedTo = null;
            user.IncorrectLoginCount = 0;
        }

        public static string GenerateBlockMessage(this User user)
        {
            var blockMessage = user.IncorrectLoginCount switch
            {
                { } n when n >= 4 => "Current user blocked for 2 minutes.",
                { } n when n >= 3 => "Current user blocked for 10 seconds.",
                { } n when n >= 2 => "Current user blocked for 5 seconds.",
                _ => ""
            };
            return blockMessage;
        }

    }
}
