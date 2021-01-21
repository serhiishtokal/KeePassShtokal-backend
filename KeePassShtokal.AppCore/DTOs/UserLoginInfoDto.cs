using System;
using System.Collections.Generic;
using System.Text;

namespace KeePassShtokal.AppCore.DTOs
{
    public class UserLoginInfoDto
    {
       public DateTime? LastSuccessfulLoginDateTime { get; set; }
       public string LastSuccessfulLoginIpAddress { get; set; }
       public DateTime? LastUnsuccessfulLoginDateTime { get; set; }
       public string LastUnsuccessfulLoginIpAddress { get; set; }
    }
}
