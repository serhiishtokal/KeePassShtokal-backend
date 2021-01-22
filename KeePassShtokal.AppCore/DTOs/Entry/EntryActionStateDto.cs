using System;
using System.Collections.Generic;
using System.Text;
using KeePassShtokal.Infrastructure.DefaultData;

namespace KeePassShtokal.AppCore.DTOs.Entry
{
    public class EntryActionStateDto
    {
        public int ActionId { get; set; }
        public int EntryId { get; set; }
        public int UserId { get; set; }
        public DateTime DateTime { get; set; }

        public string ActionTypeString { get; set; }
        public ActionTypesEnum ActionType { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordE { get; set; }
        public string WebAddress { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get;set; }
    }
}
