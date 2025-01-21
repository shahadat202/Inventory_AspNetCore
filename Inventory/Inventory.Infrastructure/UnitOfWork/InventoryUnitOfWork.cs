using Inventory.Application;
using Inventory.Domain;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using Inventory.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Inventory.Infrastructure.UnitOfWorks
{
    public class InventoryUnitOfWork : UnitOfWork, IInventoryUnitOfWork
    {
        public IProductRepository ProductRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public InventoryUnitOfWork(InventoryDbContext dbContext,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository) : base(dbContext)
        {
            ProductRepository = productRepository;
            CategoryRepository = categoryRepository;
        }

        public async Task<(IList<ProductDto> data, int total,
            int totalDisplay)> GetPagedProductsUsingSPAsync(int pageIndex, 
            int pageSize, DataTablesSearch search, string? order)
        {
            var procedureName = "GetProducts";
            var result = await SqlUtility.QueryWithStoredProcedureAsync<ProductDto>(procedureName,
                new Dictionary<string, object>
                {
                    { "PageIndex", pageIndex },
                    { "PageSize", pageSize },
                    { "OrderBy", order },
                    { "Name", search.Value },
                },
                new Dictionary<string, Type>
                {
                    { "Total", typeof(int) },
                    { "TotalDisplay", typeof(int) },
                });
            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }
    }
}