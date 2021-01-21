using System;

namespace KeePassShtokal.AppCore.DTOs.Auth
{
    public class UserLoginInfoDto
    {
       public DateTime? LastSuccessfulLoginDateTime { get; set; }
       public string LastSuccessfulLoginIpAddress { get; set; }
       public DateTime? LastUnsuccessfulLoginDateTime { get; set; }
       public string LastUnsuccessfulLoginIpAddress { get; set; }
    }
}
