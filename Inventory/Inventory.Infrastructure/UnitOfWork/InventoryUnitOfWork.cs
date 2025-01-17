using Inventory.Application;
using Inventory.Domain;
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
    }
}