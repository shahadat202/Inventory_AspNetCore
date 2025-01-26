using Inventory.Domain;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using Inventory.Domain.RepositoryContracts;
using System.Reflection.Metadata;

namespace Inventory.Application.Services
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public ProductManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        public async Task<Product> GetProductAsync(Guid id)
        {
            return await _inventoryUnitOfWork.ProductRepository.GetProductAsync(id);
        }

        public async Task<(IList<ProductDto> data, int total, int totalDisplay)> GetProductsSP(int pageIndex, 
            int pageSize, ProductSearchDto search, string? order)
        {
            return await _inventoryUnitOfWork.GetPagedProductsUsingSPAsync(pageIndex, pageSize, search, order);  
        }

        public void InsertProduct(Product product)
        {
            if (!_inventoryUnitOfWork.ProductRepository.IsTitleDuplicate(product.Name))
            {
                _inventoryUnitOfWork.ProductRepository.Add(product);
                _inventoryUnitOfWork.Save();
            }
        }
        
        public void UpdateProduct(Product product)
        {
            if (!_inventoryUnitOfWork.ProductRepository.IsTitleDuplicate(product.Name, product.Id))
            {
                _inventoryUnitOfWork.ProductRepository.Edit(product);
                _inventoryUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Title should be unique.");
        }
        public bool ProductExists(string name, Guid id)
        {
            return _inventoryUnitOfWork.ProductRepository.IsProductExist(name, id);      
        }

        public void DeleteProduct(Guid id)
        {
            _inventoryUnitOfWork.ProductRepository.Remove(id);
            _inventoryUnitOfWork.Save();
        }

    }
}