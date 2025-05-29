using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos
{
    public class CreateRoleDto
    {
        [Required(ErrorMessage = "Role is required")]
        public string Name { get; set; }
    }
}
