using System.ComponentModel.DataAnnotations;

namespace IdentityApp.DTO
{
    public class LoginUserDto
    {
        [Required]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
