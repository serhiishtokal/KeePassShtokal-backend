namespace KeePassShtokal.AppCore.DTOs.Entry
{
    public class BaseEntryDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordDecrypted { get; set; }
        public string WebAddress { get; set; }
        public string Description { get; set; }
    }
}
