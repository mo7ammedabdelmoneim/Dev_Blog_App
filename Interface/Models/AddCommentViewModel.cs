using System.ComponentModel.DataAnnotations;

namespace Interface.Models
{
    public class AddCommentViewModel
    {
        [Required]
        public string CommentText { get; set; }
        public Guid PostId { get; set; }
        public string PostSlug { get; set; }
        public string UserId { get; set; }

    }
}
