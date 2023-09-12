using IMS.CoreBusiness;
using IMS.Plugins.EFCoreSqlServer;
using IMS.UseCases.Plugininterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.EFCoreSqlServer
{
    public class ProductEFCoreRepository : IProductRepository
    {
        private readonly IDbContextFactory<IMSContext> contextFactory;

        public ProductEFCoreRepository(IDbContextFactory<IMSContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task AddProductAsync(Product product)
        {
            using var db = this.contextFactory.CreateDbContext();

            db.Products.Add(product);

            FlagInventoryUnchanged(product, db);

            await db.SaveChangesAsync();
        }

        

        public async Task<IEnumerable<Product>> GetProductsByNameUseCase(string name)
        {
            using var db = this.contextFactory.CreateDbContext();

            return await db.Products.Where(
               x => x.ProductName.ToLower().IndexOf(name.ToLower()) >= 0).ToListAsync();
        }


        public async Task<Product?> GetProductByIdAsync(int productId) //we are adding ? because if we provide an iD that does not exist, then we should allow it to return a null product
        {
            using var db = this.contextFactory.CreateDbContext();

            return await db.Products.Include(x => x.ProductInventories).ThenInclude(x => x.Inventory).FirstOrDefaultAsync(x => x.ProductId == productId);
        }

        public async Task UpdateProductAsync(Product product)
        {
            using var db = this.contextFactory.CreateDbContext();

            var prod = await db.Products
                .Include(x => x.ProductInventories)
                .FirstOrDefaultAsync(x => x.ProductId == product.ProductId);

            if(prod != null)
            {
                prod.ProductName = product.ProductName;
                prod.Price = product.Price;
                prod.Quantity = product.Quantity;
                prod.ProductInventories = product.ProductInventories;

                FlagInventoryUnchanged(product, db);   

                await db.SaveChangesAsync();
            }
        }

        //when we add a new product or update, each product contains a list of inventories, but it 
        //will add those inventories as NEW inventories, which is wrong, and therefore we create
        //this method to rectify it. We are telling hte system that the inventories are not new
        //inventories but exist instead
        private void FlagInventoryUnchanged(Product product, IMSContext db)
        {
           
            if (product?.ProductInventories != null && product.ProductInventories.Count > 0)
            {
                foreach (var prodInv in product.ProductInventories)
                {
                    if (prodInv.Inventory != null)
                    {
                        db.Entry(prodInv.Inventory).State = EntityState.Unchanged;
                    }
                }
            }
        }


    }
}
