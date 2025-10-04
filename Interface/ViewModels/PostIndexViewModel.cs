using Source.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Interface.ViewModels
{
    public class PostIndexViewModel
    {
        public List<PostViewModel>? FeaturedPosts { get; set; }

        public List<PostViewModel>? LatestPosts { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }
    }
}
