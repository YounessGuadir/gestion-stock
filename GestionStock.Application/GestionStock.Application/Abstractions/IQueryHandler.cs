using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Abstractions
{
    public interface IQueryHandler<TQuery, TResult>
    {
        Task<TResult> Handle(TQuery query, CancellationToken ct);
    }
}
