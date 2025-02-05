
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Product
    {
        public long Id { get; set; }
        [Required(ErrorMessage ="Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Size is required.")]
        public string Size { get; set; }
        [Required(ErrorMessage = "Color is required.")]
        public string Color { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Size is required.")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string Description { get; set; }
    }
}
