namespace Interface.ViewModels
{
    public class AdminUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CurrentRole { get; set; }
    }
}
