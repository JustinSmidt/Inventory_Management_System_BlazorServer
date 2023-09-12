using IMS.CoreBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Reports.Interfaces
{
    public interface ISearchInventoryTransactionUseCase
    {
        Task<IEnumerable<InventoryTransaction>> ExecuteAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo, InventoryTransactionType? transactionType);
    
            
    }
}
