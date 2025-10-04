namespace Interface.ViewModels
{
    public class PostByCatViewModel
    {
        public string CategoryName { get; set; }
        public List<PostViewModel> Posts { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
