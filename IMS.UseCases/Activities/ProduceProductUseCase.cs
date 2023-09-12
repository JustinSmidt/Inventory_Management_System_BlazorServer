using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.CoreBusiness;
using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.Plugininterfaces;

namespace IMS.UseCases.Activities
{
    public class ProduceProductUseCase : IProduceProductUseCase
    {
        private readonly IProductRepository productRepository;
        private readonly IProductTransactionRepository productTransactionRepository;
        

        public ProduceProductUseCase(IProductRepository productRepository, IProductTransactionRepository productTransactionRepository)
        {
            this.productRepository = productRepository;
            this.productTransactionRepository = productTransactionRepository;
            
        }


        public async Task ExecuteAsync(string productionNumber, Product product, int quantity, string doneBy)
        {
            //add a transaction record
            await this.productTransactionRepository.ProduceAsync(productionNumber, product, quantity, doneBy);  
           
            //update the quantity of product
            product.Quantity += quantity;        
            await this.productRepository.UpdateProductAsync(product);
        }
    }
}
