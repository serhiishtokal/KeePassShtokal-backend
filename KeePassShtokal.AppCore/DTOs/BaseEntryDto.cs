using System;
using System.Collections.Generic;
using System.Text;

namespace KeePassShtokal.AppCore.DTOs
{
    public class BaseEntryDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordDecrypted { get; set; }
        public string WebAddress { get; set; }
        public string Description { get; set; }
    }
}
