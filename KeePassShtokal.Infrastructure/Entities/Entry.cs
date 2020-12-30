using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KeePassShtokal.Infrastructure.Entities
{
    public class Entry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EntryId { get; set; }

        [Required]
        public string Login { get; set; }
        public string Email { get; set; }
        [Required]
        public string PasswordE { get; set; }
        public string WebAddress { get; set; }
        public string Description { get; set; }

        public virtual ICollection<UsersEntries> EntryUsers { get; set; }
    }
}
