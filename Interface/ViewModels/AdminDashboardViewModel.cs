namespace Interface.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; } = 0;
        public int TotalPosts { get; set; } = 0;
        public int TotalCategories { get; set; } = 0;
        public int TotalComments { get; set; } = 0;

        public List<LatestPostsDashboardViewModel>? LatestPosts {  get; set; } 
        public List<NewUsersDashboardViewModel>? LatestUsers {  get; set; } 

        public double AvgPostsPerUser { get; set; }
        public string MostActiveCategory { get; set; }
        public string TopPostTitle { get; set; }

    }

    public class LatestPostsDashboardViewModel
    {
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
    }
    
    public class NewUsersDashboardViewModel
    {
        public string Title { get; set; }
        public string Email {  get; set; } 
        public DateTime CreationDate { get; set; }
    }
}
