using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeePassShtokal.Infrastructure.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public string Salt { get; set; }
        [Required]
        public bool IsPasswordKeptAsSha { get; set; }
        [Required]
        public int IncorrectLoginCount { get; set; }
        public DateTime? BlockedTo { get; set; }
        public virtual ICollection<UsersEntries> UserEntries { get; set; }
    }
}
