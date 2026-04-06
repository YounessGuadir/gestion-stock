using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Stock.DTOs
{
    public record StockBalanceDto(
      Guid Id,
      Guid TenantId,
      Guid ProductId,
      decimal Quantity,
      decimal LowStockThreshold,
      DateTime UpdatedAt
  );
}
