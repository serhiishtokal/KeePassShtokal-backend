using System;
using System.Collections.Generic;
using System.Text;

namespace KeePassShtokal.AppCore.DTOs
{
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public bool IsPasswordKeptAsHash { get; set; }
    }
}
