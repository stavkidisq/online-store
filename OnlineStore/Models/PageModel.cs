using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class PageModel
    {
        public int Id { get; set; }
        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        public string Title { get; set; } = null!;
        public string? Slug { get; set; }
        [Required, MinLength(2, ErrorMessage = "Minimum length is 4")]
        public string Content { get; set; } = null!;
        public int Sorting { get; set; }
    }
}
