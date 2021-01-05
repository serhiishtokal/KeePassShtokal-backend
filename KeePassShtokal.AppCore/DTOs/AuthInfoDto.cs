using System;
using System.Collections.Generic;
using System.Text;

namespace KeePassShtokal.AppCore.DTOs
{
    public class AuthInfoDto
    {
        public string Username { get; set; }
        public DateTime SuccessfulSignIn { get; set; }
        public DateTime UnSuccessfulSignIn { get; set; }
    }
}
