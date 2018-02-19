using Microsoft.AspNetCore.Identity;

namespace IdentityApp.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
    }
}
