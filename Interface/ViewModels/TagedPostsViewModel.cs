namespace Interface.ViewModels
{
    public class TagedPostsViewModel
    {
        public List<PostViewModel>? Posts { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }
    }
}
