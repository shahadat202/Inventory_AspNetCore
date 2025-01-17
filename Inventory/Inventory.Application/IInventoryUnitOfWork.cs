using Inventory.Domain;
using Inventory.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Inventory.Application
{
    public interface IInventoryUnitOfWork : IUnitOfWork
    {
        public IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
    }
}