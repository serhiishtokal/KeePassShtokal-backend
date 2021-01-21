namespace KeePassShtokal.AppCore.DTOs.Auth
{
    public class BaseAuthDto
    {
        
        public string Username { get; set; }
      
        public string Password { get; set; }

        public bool IsReadMode { get; set; }
    }
}
