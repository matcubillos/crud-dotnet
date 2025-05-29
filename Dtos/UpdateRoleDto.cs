using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos
{
    public class UpdateRoleDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
        
    }
}
