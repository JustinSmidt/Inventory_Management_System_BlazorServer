using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.CoreBusiness
{
    public class ProductInventory
    {
        public int ProductId { get; set; }
        
        public Product? Product { get; set; }   //navigation property    EF Core will know how to create DB schema based on this

        public int InventoryId { get; set; }

        public Inventory? Inventory { get; set; }  //navigation

        public int InventoryQuantity { get; set; }
    }
}
