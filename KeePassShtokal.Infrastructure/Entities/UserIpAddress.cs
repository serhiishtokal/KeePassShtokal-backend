using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KeePassShtokal.Infrastructure.Entities
{
    public class UserIpAddress
    {
        [Required]
        public int IpAddressId { get; set; }

        [ForeignKey(nameof(IpAddressId))]
        public IpAddress IpAddress { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public DateTime? BlockedTo { get; set; }
        [Required]
        public int IncorrectLoginCount { get; set; }
    }
}
