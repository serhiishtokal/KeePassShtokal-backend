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
        public string UserOwnerUsername { get; set; }
        [Required]
        public int CurrentEntryStateId { get; set; }
        [ForeignKey(nameof(CurrentEntryStateId))]
        public EntryState CurrentEntryState { get; set; }
    }
}
