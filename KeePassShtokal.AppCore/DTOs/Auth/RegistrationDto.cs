namespace KeePassShtokal.AppCore.DTOs.Auth
{
    public class RegistrationDto : BaseAuthDto
    {
        public bool IsPasswordKeptAsHash { get; set; }
    }
}
