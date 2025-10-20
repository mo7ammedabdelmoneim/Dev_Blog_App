using Source.UtilitClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.Models
{
    public class Post
    {
        public Guid Id { get; set; }

        [Required, StringLength(200)]
        private string title;
        public string Title
        {
            get => title;
            set
            {
                title = value;
                Slug = SlugHelper.GenerateSlug(value);
            }
        }

        public string? CoverUrl { get; set; } = "/uploads/posts/Default.jpeg";

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public string Slug { get; set; }

        public List<Tag>? Tags { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public List<PostReacts> PostReacts { get; set; } = new List<PostReacts>();
    }
}