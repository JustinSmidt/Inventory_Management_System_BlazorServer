using IMS.CoreBusiness;
using IMS.UseCases.Plugininterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.InMemory
{
    public class ProductRepository : IProductRepository
    {

        private List<Product> _products;

        public ProductRepository()
        {
            _products = new List<Product>()
            {
                new Product{ProductId = 1, ProductName = "Bike", Quantity = 10, Price = 150},
                new Product{ProductId = 2, ProductName = "Car", Quantity = 5, Price = 25000},             
            };
        }

        public Task AddProductAsync(Product product)
        {
            if (_products.Any(x => x.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;    //wont do any inserting as there is already a duplicate
            }
            else
            {
                var maxId = _products.Max(x => x.ProductId);    //assigning new Id to added inventory item
                product.ProductId = maxId + 1;

                _products.Add(product);
                return Task.CompletedTask;
            }

        }

        

        public async Task<IEnumerable<Product>> GetProductsByNameUseCase(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await Task.FromResult(_products);    ///will show everything
            }
            else
            {
                return _products.Where(x => x.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }


        public async Task<Product?> GetProductByIdAsync(int productId) //we are adding ? because if we provide an iD that does not exist, then we should allow it to return a null product
        {
           var prod = _products.First(x => x.ProductId == productId);
            var newProd = new Product();
            if(prod != null)
            {
                newProd.ProductId = productId;
                newProd.ProductName = prod.ProductName;
                newProd.Price = prod.Price;
                newProd.Quantity= prod.Quantity;
                newProd.ProductInventories = new List<ProductInventory>();
                if(prod.ProductInventories != null && prod.ProductInventories.Count > 0)
                {
                    foreach(var prodInv in prod.ProductInventories)   //copying inventories to new product
                    {
                        var newProdInv = new ProductInventory
                        {
                            InventoryId = prodInv.InventoryId,
                            ProductId = prodInv.ProductId,
                            Product = prod,
                            Inventory = new Inventory(),
                            InventoryQuantity = prodInv.InventoryQuantity
                        };
                        if(prodInv.Inventory != null)
                        {
                            newProdInv.Inventory.InventoryId = prodInv.InventoryId;
                            newProdInv.Inventory.InventoryName = prodInv.Inventory.InventoryName;
                            newProdInv.Inventory.Price = prodInv.Inventory.Price;
                            newProdInv.Inventory.Quantity = prodInv.Inventory.Quantity;
                                
                        }
                        newProd.ProductInventories.Add(newProdInv);
                    }
                }
            }

            return await Task.FromResult(newProd);
            
        }

        public Task UpdateProductAsync(Product product)
        {
            //prevent different products from having the same name
            if(_products.Any(x => x.ProductId != product.ProductId && x.ProductName.ToLower() == product.ProductName.ToLower()))
            {
                return Task.CompletedTask;
            }

            var prod = _products.FirstOrDefault(x => x.ProductId == product.ProductId);

            if(prod != null)
            {
                prod.ProductName = product.ProductName;
                prod.Price = product.Price;
                prod.Quantity = product.Quantity;
                prod.ProductInventories = product.ProductInventories;
            }
            return Task.CompletedTask;
        }
    }
}
