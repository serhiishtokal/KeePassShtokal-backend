using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using KeePassShtokal.Infrastructure.DefaultData;

namespace KeePassShtokal.Infrastructure.Entities
{
    public class EntryAction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActionId { get; set; }

        [Required]
        public ActionTypesEnum ActionType { get; set; }


        [Required]
        public int EntryStateId { get; set; }
        [ForeignKey(nameof(EntryStateId))]
        public EntryState EntryState { get; set; }


        [Required]
       
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [Required]
        public int EntryId { get; set; }
        [ForeignKey(nameof(EntryId))]
        public Entry Entry { get; set; }


        [Required]
        public bool IsRestorable { get; set; }

        [Required]
        public DateTime DateTime { get; set; }
    }
}
