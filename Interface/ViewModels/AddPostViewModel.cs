using Source.Models;
using System.ComponentModel.DataAnnotations;

namespace Interface.ViewModels
{
    public class AddPostViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 150 characters.")]
        public string Title { get; set; }

        [Display(Name = "Cover Image")]
        [DataType(DataType.Upload)]
        public IFormFile? Cover { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid category.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [MinLength(20, ErrorMessage = "Content must be at least 20 characters long.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Please select at least one tag.")]
        [MinLength(1, ErrorMessage = "Please select at least one tag.")]
        public List<Guid>? Tags { get; set; }

        public string? ExistingCoverUrl { get; set; }


    }
}
