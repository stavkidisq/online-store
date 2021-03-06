using OnlineStore.Infrastructure;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        public string Name { get; set; } = null!;
        public string? Slug { get; set; }

        [Required, MinLength(10, ErrorMessage = "Minimum length is 10")]
        public string Description { get; set; } = null!;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "You must choose a category")]
        public int CategoryId { get; set; }
        public string? Image { get; set; }

        [ForeignKey("CategoryId")]
        public virtual CategoryModel? Category { get; set; }

        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }
    }
}
