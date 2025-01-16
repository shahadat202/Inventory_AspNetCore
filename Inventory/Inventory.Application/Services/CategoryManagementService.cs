using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services
{
    public class CategoryManagementService : ICategoryManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public CategoryManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        public IList<Category> GetCategories()
        {
            return _inventoryUnitOfWork.CategoryRepository.GetAll();
        }

        public Category GetCategory(Guid categoryId)
        {
            return _inventoryUnitOfWork.CategoryRepository.GetById(categoryId);
        }
    }
}