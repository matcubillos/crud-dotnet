using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "The email address format is invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Role Id is required")]
        public int RoleId { get; set; }
    }
}
