using GestionStock.Application.Abstractions;
using GestionStock.Application.Dashboard.Dtos;
using GestionStock.Application.Contracts.Persistence;

namespace GestionStock.Application.Dashboard.Queries;

public class GetDashboardHandler : IQueryHandler<GetDashboardQuery, DashboardDto>
{
    private readonly IProductRepository _products;
    private readonly ICategoryRepository _categories;
    private readonly IStockBalanceRepository _balances;
    private readonly IStockMovementRepository _movements;

    public GetDashboardHandler(
        IProductRepository products,
        ICategoryRepository categories,
        IStockBalanceRepository balances,
        IStockMovementRepository movements)
    {
        _products = products;
        _categories = categories;
        _balances = balances;
        _movements = movements;
    }

    public async Task<DashboardDto> Handle(GetDashboardQuery request, CancellationToken ct)
    {
        var tenantId = request.TenantId;

        var products = await _products.GetAllAsync(tenantId, null, null, ct);
        var categories = await _categories.GetAllAsync(tenantId, ct);
        var balances = await _balances.GetAllAsync(tenantId, ct);
        var movements = await _movements.GetAllAsync(tenantId, ct);

        // 🗺️ Map catégories
        var categoryMap = categories
            .GroupBy(c => c.Id)
            .ToDictionary(g => g.Key, g => g.First().Name);

        // 💰 Total Stock Value (باستعمال Product.Price)
        var totalStockValue = balances.Sum(b =>
        {
            var product = products.FirstOrDefault(p => p.Id == b.ProductId);
            return product != null ? b.Quantity * product.Price : 0;
        });

        return new DashboardDto
        {
            // 📊 KPIs
            TotalProducts = products.Count,
            TotalCategories = categories.Count,
            TotalTransactions = movements.Count,
            TotalStockValue = totalStockValue,

            // 📦 Stock Status
            StockNormal = balances.Count(b => b.Quantity > 2),
            StockLow = balances.Count(b => b.Quantity > 0 && b.Quantity <= 2),
            StockOut = balances.Count(b => b.Quantity == 0),

            // 📈 Top catégories
            TopCategories = products
                .GroupBy(p => p.CategoryId)
                .Select(g => new CategoryStatsDto
                {
                    Name = categoryMap.ContainsKey(g.Key) ? categoryMap[g.Key] : "Unknown",
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList(),

            // 🔥 Produits critiques
            CriticalProducts = balances
                .Where(b => b.Quantity <= 2)
                .Select(b =>
                {
                    var product = products.FirstOrDefault(p => p.Id == b.ProductId);

                    return new ProductCriticalDto
                    {
                        Id = b.ProductId,
                        Name = product?.Name ?? "Unknown",
                        ImageUrl = product?.ImageUrl,
                        Quantity = b.Quantity
                    };
                })
                .ToList()
        };
    }
}