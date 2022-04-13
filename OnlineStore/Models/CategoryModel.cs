﻿using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters are allowed!")]
        public string Name { get; set; } = null!;
        [Required]
        public string Slug { get; set; } = null!;
        public int Sorting { get; set; }
    }
}
