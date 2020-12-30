using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KeePassShtokal.Infrastructure.Entities
{
    public class UsersEntries
    {
        public UsersEntries()
        {
            CreationDateTime = DateTime.UtcNow;
        }
        [Required]
        public DateTime CreationDateTime { get; set; }
        [Required]
        public bool IsUserOwner { get; set; }
        [Required]
        public int EntryId { get; set; }
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(EntryId))]
        public virtual Entry Entry { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
