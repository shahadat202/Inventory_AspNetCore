using Inventory.Domain;
using Inventory.Domain.Entities;
using Inventory.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product, Guid>, IProductRepository
    {
        public ProductRepository(InventoryDbContext context) : base(context)
        {

        }

        public (IList<Product> data, int total, int totalDisplay) GetPagedProducts(int pageIndex, int pageSize, DataTablesSearch search, string order)
        {
            return GetDynamic(x => x.Name == search.Value, order, null, pageIndex, pageSize, true);
        }
    }
}
