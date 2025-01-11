using Inventory.Domain.Entities;

namespace Inventory.Application.Services
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public ProductManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        public void InsertProduct(Product product)
        {
            _inventoryUnitOfWork.ProductRepository.Add(product);
            _inventoryUnitOfWork.Save();
        }
    }
}