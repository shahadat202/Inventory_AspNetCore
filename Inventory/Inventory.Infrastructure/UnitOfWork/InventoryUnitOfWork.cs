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
    public class InventoryUnitOfWork : UnitOfWork
    {
        public IProductRepository ProductRepository { get; private set; }
        public InventoryUnitOfWork(InventoryDbContext dbContext,
            IProductRepository productRepository) : base(dbContext)
        {
            ProductRepository = productRepository;
        }
    }
}