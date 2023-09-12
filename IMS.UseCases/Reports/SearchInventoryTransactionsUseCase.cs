using IMS.CoreBusiness;
using IMS.UseCases.Plugininterfaces;
using IMS.UseCases.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Reports
{
    public class SearchInventoryTransactionsUseCase : ISearchInventoryTransactionUseCase
    {
        private readonly IInventoryTransactionRepository inventoryTransactionRepository;


        public SearchInventoryTransactionsUseCase(IInventoryTransactionRepository inventoryTransactionRepository)
        {
            this.inventoryTransactionRepository = inventoryTransactionRepository;
        }


        public async Task<IEnumerable<InventoryTransaction>> ExecuteAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo, InventoryTransactionType? transactionType)
        {
            //we are doing it here because it is business logic function
            if(dateTo.HasValue)
            {
                dateTo = dateTo.Value.AddDays(1);     //changing it to next days date in order to retrieve everything that occurred until midnight today at 0:00, when it reaches next day
            }                                         //Because if dateTo is 5pm today, i dont want to just retrieve everything up to 5pm, i want to retreive everything happing in the entire day, untill 0:00

            return await this.inventoryTransactionRepository.GetInventoryTransactionsAsync(inventoryName, dateFrom, dateTo, transactionType);
        }

    }
}
