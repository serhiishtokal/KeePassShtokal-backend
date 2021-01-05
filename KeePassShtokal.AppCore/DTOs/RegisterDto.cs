namespace KeePassShtokal.AppCore.DTOs
{
    public class RegisterDto : BaseAuthDto
    {
        public bool IsPasswordKeptAsHash { get; set; }
    }
}
