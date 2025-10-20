namespace Interface.ViewModels
{
    public class AdminStatsViewModel
    {
        public IEnumerable<object>? MonthlyUserGrowth { get; set; }
        public IEnumerable<object>? MonthlyPostGrowth { get; set; }

        public double AvgCommentsPerPost { get; set; }
        public double AvgReactsPerPost { get; set; }

        public IEnumerable<object>? TopCategories { get; set; }
        public IEnumerable<object>? RoleCounts { get; set; }
    }
}
