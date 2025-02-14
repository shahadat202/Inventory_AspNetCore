using Inventory.Domain;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using Inventory.Domain.RepositoryContracts;
using System.Linq;
using System.Reflection.Metadata;

namespace Inventory.Application.Services
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        private readonly IProductRepository _productRepository;
        public ProductManagementService(IInventoryUnitOfWork inventoryUnitOfWork,
            IProductRepository productRepository)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
            _productRepository = productRepository;
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            return await _inventoryUnitOfWork.ProductRepository.GetProductAsync(id);
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _inventoryUnitOfWork.ProductRepository.GetAllAsync();  
        }

        public async Task<(IList<ProductDto> data, int total, int totalDisplay)> GetProductsSP(int pageIndex, 
            int pageSize, ProductSearchDto search, string? order)
        {
            //order ??= "Name ASC";
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

        // Dashboard part
        public async Task<int> GetTotalItems()
        {
            var products = await _inventoryUnitOfWork.ProductRepository.GetAllAsync();
            return products.Count();
        }

        public async Task<decimal> GetTotalBuyingValue()
        {
            var values = await _inventoryUnitOfWork.ProductRepository.GetAllAsync();
            return values.Sum(x => x.BuyingPrice * x.StockQuantity);
        }

        public async Task<int> GetTotalRegistration()
        {
            return await _productRepository.GetTotalRegistrationAsync();
        }
    }
}