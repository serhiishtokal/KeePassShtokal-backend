using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KeePassShtokal.AppCore.DTOs
{
    public class BlockedIpDto
    {
        public int AddressId { get; set; }
        public string IpAddressString { get; set; }
        public DateTime? BlockedTo { get; set; }
        public int IncorrectLoginCount { get; set; }
    }
}
