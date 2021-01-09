namespace KeePassShtokal.AppCore.DTOs
{
    public class RegistrationDto : BaseAuthDto
    {
        public bool IsPasswordKeptAsHash { get; set; }
    }
}
