using Inventory.Domain;
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
        public void InsertProduct(Product product);
        (IList<Product> data, int total, int totalDisplay) GetProducts(int pageIndex, 
            int pageSize, DataTablesSearch search, string? order);
    }
}