namespace Interface.ViewModels
{
    public class GrowthItem
    {
        public string Month { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class CategoryItem
    {
        public string Name { get; set; } = string.Empty;
        public int PostCount { get; set; }
    }

    public class RoleItem
    {
        public string RoleName { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class AdminStatsViewModel
    {
        public IEnumerable<GrowthItem> MonthlyUserGrowth { get; set; } = new List<GrowthItem>();
        public IEnumerable<GrowthItem> MonthlyPostGrowth { get; set; } = new List<GrowthItem>();

        public double AvgCommentsPerPost { get; set; }
        public double AvgReactsPerPost { get; set; }

        public IEnumerable<CategoryItem> TopCategories { get; set; } = new List<CategoryItem>();
        public IEnumerable<RoleItem> RoleCounts { get; set; } = new List<RoleItem>();
    }
}
