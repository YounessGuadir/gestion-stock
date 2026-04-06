using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Dashboard.Dtos
{
    public class DashboardDto
    {
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public int TotalTransactions { get; set; }
        public decimal TotalStockValue { get; set; }

        public int StockNormal { get; set; }
        public int StockLow { get; set; }
        public int StockOut { get; set; }

        public List<CategoryStatsDto> TopCategories { get; set; } = new();
        public List<ProductCriticalDto> CriticalProducts { get; set; } = new();
    }

    public class CategoryStatsDto
    {
        public string Name { get; set; } = "";
        public int Count { get; set; }
    }

    public class ProductCriticalDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string? ImageUrl { get; set; }
        public decimal Quantity { get; set; }
    }
}
