using IMS.CoreBusiness;
using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.Plugininterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Activities
{
    public class SellProductUseCase : ISellProductUseCase
    {
        private readonly IProductRepository productRepository;
        private readonly IProductTransactionRepository productTransactionRepository;

        public SellProductUseCase(IProductRepository productRepository, IProductTransactionRepository productTransactionRepository)
        {
            this.productRepository = productRepository;
            this.productTransactionRepository = productTransactionRepository;
        }

        public async Task ExecuteAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy)
        {
            await this.productTransactionRepository.SellProductAsync(salesOrderNumber, product, quantity, unitPrice, doneBy);

            //decrease product
            product.Quantity -= quantity;
            await this.productRepository.UpdateProductAsync(product);
        }


    }
}
