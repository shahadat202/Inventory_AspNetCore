using Inventory.Domain;
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
        
        public (IList<Product> data, int total, int totalDisplay) GetProducts(int pageIndex, 
            int pageSize, DataTablesSearch search, string? order)
        {
            return _inventoryUnitOfWork.ProductRepository.GetPagedProducts(pageIndex, pageSize, search, order);  
        }
    }
}