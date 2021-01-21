using System;

namespace KeePassShtokal.AppCore.DTOs.Auth
{
    public class AuthInfoDto
    {
        public string Username { get; set; }
        public DateTime SuccessfulSignIn { get; set; }
        public DateTime UnSuccessfulSignIn { get; set; }
    }
}
