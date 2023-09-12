using IMS.CoreBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Plugininterfaces
{
    public interface IInventoryTransactionRepository
    {
        Task PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, double price);

        Task ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy);  /*double price*/
        Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo, InventoryTransactionType? transactionType);
    }
}
