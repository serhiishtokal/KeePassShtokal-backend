using System;

namespace KeePassShtokal.AppCore.DTOs.Auth
{
    public class BlockedIpDto
    {
        public int AddressId { get; set; }
        public string IpAddressString { get; set; }
        public DateTime? BlockedTo { get; set; }
        public int IncorrectLoginCount { get; set; }
    }
}
