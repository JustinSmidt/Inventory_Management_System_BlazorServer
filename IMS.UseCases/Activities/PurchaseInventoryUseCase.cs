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
    public class PurchaseInventoryUseCase : IPurchaseInventoryUseCase
    {
        private readonly IInventoryTransactionRepository inventoryTransactionRepository;
        private readonly IInventoryRepository inventoryRepository;     //we are injecting it in because we need the update function for inventory, as we will need to update the inventory quantity

        public PurchaseInventoryUseCase(IInventoryTransactionRepository inventoryTransactionRepository, IInventoryRepository inventoryRepository)
        {
            this.inventoryTransactionRepository = inventoryTransactionRepository;
            this.inventoryRepository = inventoryRepository;
        }

        public async Task ExecuteAsync(string poNumber, Inventory inventory, int quantity, string doneBy)
        {
            //Insert a record in the transaction table
            await inventoryTransactionRepository.PurchaseAsync(poNumber, inventory, quantity, doneBy, inventory.Price);   //we provide price because prices change, and what has been ordered yesterday can have a different price today

            //we will need to update inventory by increasing quantity
            inventory.Quantity += quantity;
            await this.inventoryRepository.UpdateInventoryAsync(inventory);   //update the quantity


        }
    }
}
