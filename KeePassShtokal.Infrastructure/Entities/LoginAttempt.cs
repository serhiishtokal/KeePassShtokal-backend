using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KeePassShtokal.Infrastructure.Entities
{
    //todo 
    public class LoginAttempt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoginTrialId { get; set; }
        [Required]
        public int? UserId { get; set; }
        [Required]
        public int IpId { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public bool IsSuccessful { get; set; }
        [ForeignKey(nameof(IpId))]
        public IpAddress IpAddress { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}