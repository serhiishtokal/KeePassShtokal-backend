using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KeePassShtokal.Infrastructure.Entities
{
    public class EntryState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EntryStateId { get; set; }
        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        public string Username { get; set; }
        public string Email { get; set; }
        [Required]
        public string PasswordE { get; set; }
        public string WebAddress { get; set; }
        public string Description { get; set; }
    }
}
