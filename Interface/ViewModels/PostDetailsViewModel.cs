using Source.Models;

namespace Interface.ViewModels
{
    public class PostDetailsViewModel
    {
        public PostViewModel Post { get; set; }
        public List<Comment>? Comments { get; set; }
    }
}
