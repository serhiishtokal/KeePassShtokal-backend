namespace KeePassShtokal.AppCore.DTOs.Entry
{
    public class GetEntryDto
    {
        public int EntryId { get; set; }
        public bool IsOwner { get; set; }
        public bool IsDeleted { get; set; }
        public string UserOwnerUsername { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordEncrypted { get; set; }
        public string WebAddress { get; set; }
        public string Description { get; set; }

    }
}
