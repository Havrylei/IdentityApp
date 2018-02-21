using System.ComponentModel.DataAnnotations;

namespace IdentityApp.DTO
{
    public class ChangePasswordDto
    {
        [Required]
        public string ID { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords don't match!")]
        public string PasswordConfirm { get; set; }
    }
}
