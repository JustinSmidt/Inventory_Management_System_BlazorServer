using IMS.CoreBusiness;
using IMS.UseCases.Plugininterfaces;
using IMS.UseCases.Inventories.Interfaces;

namespace IMS.UseCases.Inventories
{
    public class ViewInventoriesByNameUseCase : IViewInventoriesByNameUseCase
    {
        private readonly IInventoryRepository inventoryRepository;

        public ViewInventoriesByNameUseCase(IInventoryRepository inventoryRepository)  //plugging in interface repository
        {
            this.inventoryRepository = inventoryRepository;
        }

        public async Task<IEnumerable<Inventory>> ExecuteAsync(string name = "") //returns a list of inventories
        {
            return await inventoryRepository.GetInventoriesByNameAsync(name);
        }
    }
}