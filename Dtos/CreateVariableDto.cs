using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos
{
    public class CreateVariableDto
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;

        [Required(ErrorMessage = "Type is required")]
        [RegularExpression("^(text|numeric|boolean)$", ErrorMessage = "Type must be 'text', 'numeric', or 'boolean'")]
        public string Type { get; set; } = string.Empty;
    }
}
