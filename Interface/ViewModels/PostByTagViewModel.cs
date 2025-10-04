namespace Interface.ViewModels
{
    public class PostByTagViewModel
    {
        public string TagName { get; set; }
        public List<PostViewModel> Posts { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

}
