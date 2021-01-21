namespace KeePassShtokal.AppCore.DTOs.Auth
{
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public bool IsPasswordKeptAsHash { get; set; }
    }
}
