using Source.Models;
using System.ComponentModel.DataAnnotations;

namespace Interface.ViewModels
{
    public class PostViewModel {
        public Guid PostId { get; set; }
        [Required]
        public string Title { get; set; }

        public string? Slug { get; set; }

        public string? CoverUrl { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public List<Tag>? Tags { get; set; }

        public int Reacts { get; set; }

        public string AuthorName { get; set; }

    }

}
