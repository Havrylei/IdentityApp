using System.ComponentModel.DataAnnotations;

namespace IdentityApp.DTO
{
    public class EditUserDto
    {
        [Required]
        public string ID { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
