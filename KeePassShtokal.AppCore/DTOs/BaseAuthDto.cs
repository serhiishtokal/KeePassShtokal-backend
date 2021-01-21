using System.ComponentModel.DataAnnotations;

namespace KeePassShtokal.AppCore.DTOs
{
    public class BaseAuthDto
    {
        
        public string Username { get; set; }
      
        public string Password { get; set; }

        public bool IsReadMode { get; set; }
    }
}
