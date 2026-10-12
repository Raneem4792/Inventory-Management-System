namespace TestWebApplication.Models.ViewModels
{
    public class DashboardVM
    {
        public int TotalCategories { get; set; }
        public int TotalItems { get; set; }
        public int TotalEmployees { get; set; }
        public int LowStockItems { get; set; }

        public IEnumerable<Item> LowStackList { get; set; }
        public IEnumerable<StockTransaction> LatestTransactions { get; set; }
    }
}
