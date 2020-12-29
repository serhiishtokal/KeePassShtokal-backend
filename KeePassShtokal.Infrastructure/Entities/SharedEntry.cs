using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KeePassShtokal.Infrastructure.Entities
{
    public class SharedEntry
    {
        public SharedEntry()
        {
            CreationDateTime = DateTime.UtcNow;
        }
        [Required]
        public DateTime CreationDateTime { get; set; }
        [Required]
        public int EntryId { get; set; }
        [Required]
        public int UserId { get; set; }


        public virtual Entry Entry { get; set; }
        public virtual User User { get; set; }
    }
}
