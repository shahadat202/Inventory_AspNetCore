using Inventory.Domain;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services
{
    public interface IProductManagementService
    {
        Task<Product> GetProductByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<(IList<ProductDto> data, int total, int totalDisplay)> GetProductsSP(int pageIndex, 
            int pageSize, ProductSearchDto search, string? order);
        void InsertProduct(Product product);
        void UpdateProduct(Product product);
        bool ProductExists(string name, Guid id);
        void DeleteProduct(Guid id);

        // Dashboard part
        Task<int> GetTotalItems();
        Task<decimal> GetTotalValue();
    }
}