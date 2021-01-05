using System.ComponentModel.DataAnnotations;

namespace KeePassShtokal.AppCore.DTOs
{
    public class BaseAuthDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
