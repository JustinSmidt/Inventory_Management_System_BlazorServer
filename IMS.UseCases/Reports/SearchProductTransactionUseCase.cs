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
    public class SearchProductTransactionUseCase : ISearchProductTransactionUseCase
    {
        private readonly IProductTransactionRepository productTransactionRepository;

        public SearchProductTransactionUseCase(IProductTransactionRepository productTransactionRepository)
        {
            this.productTransactionRepository = productTransactionRepository;
        }


        public async Task<IEnumerable<ProductTransaction>> ExecuteAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? transactionType)
        {
            //we are doing it here because it is business logic function
            if (dateTo.HasValue)
            {
                dateTo = dateTo.Value.AddDays(1);     //changing it to next days date in order to retrieve everything that occurred until midnight today at 0:00, when it reaches next day
            }                                         //Because if dateTo is 5pm today, i dont want to just retrieve everything up to 5pm, i want to retreive everything happing in the entire day, untill 0:00

            return await this.productTransactionRepository.GetProductTransactionsAsync(productName, dateFrom, dateTo, transactionType);
        }
    }
}
