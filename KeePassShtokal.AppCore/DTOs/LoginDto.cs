using System;
using System.Collections.Generic;
using System.Text;

namespace KeePassShtokal.AppCore.DTOs
{
    public class LoginDto : BaseAuthDto
    {
        public string IpAddress { get; set; }
    }
}
