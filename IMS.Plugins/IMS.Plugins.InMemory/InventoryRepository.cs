using IMS.CoreBusiness;
using IMS.UseCases.Plugininterfaces;

namespace IMS.Plugins.InMemory
{
    public class InventoryRepository : IInventoryRepository
    {

        private List<Inventory> _inventories;

        public InventoryRepository() 
        {
            _inventories = new List<Inventory>()
            {
                new Inventory{InventoryId = 1, InventoryName = "Bike Seat", Quantity = 10, Price = 2},
                new Inventory{InventoryId = 2, InventoryName = "Bike Body", Quantity = 10, Price = 15},
                new Inventory{InventoryId = 3, InventoryName = "Bike Wheels", Quantity = 20, Price = 8},
                new Inventory{InventoryId = 4, InventoryName = "Bike Pedals", Quantity = 20, Price = 1},
            };
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                return await Task.FromResult(_inventories);    ///will show everything
            }
            else
            {
                return _inventories.Where(x => x.InventoryName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        public Task AddInventoryAsync(Inventory inventory)
        {
            if (_inventories.Any(x => x.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;    //wont do any inserting as there is already a duplicate
            }
            else
            {
                var maxId = _inventories.Max(x => x.InventoryId);    //assigning new Id to added inventory item
                inventory.InventoryId = maxId + 1;

                _inventories.Add(inventory);
                return Task.CompletedTask;
            }
             
        }

        public Task UpdateInventoryAsync(Inventory inventory)
        {
            //this is there for when you change the name of the first inventory, to the same name as the second inventory for example, then we should not allow this to happen, otherwise you will have duplicate names with different Ids 
            if (_inventories.Any(x => x.InventoryId != inventory.InventoryId && x.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;
            }

            var inv = _inventories.FirstOrDefault(x => x.InventoryId == inventory.InventoryId);


            if (inv != null)   //if we found something
            {
                inv.InventoryName = inventory.InventoryName;
                inv.Price = inventory.Price;
                inv.Quantity = inventory.Quantity;
            }
            return Task.CompletedTask;
        }

        public async Task<Inventory> GetInventoryByIdAsync(int inventoryId)
        {
           var inv = _inventories.First(x => x.InventoryId == inventoryId);
            var newInv = new Inventory
            {
                InventoryId = inv.InventoryId,
                InventoryName = inv.InventoryName,
                Price = inv.Price,
                Quantity = inv.Quantity,
            };

            return await Task.FromResult(newInv);
            
        }
    }
}