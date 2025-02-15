using Inventory.Application.ServiceInterface;
using Inventory.Domain.Entities;
using Inventory.Domain.RepositoryInterface;
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
        public async Task<Category> GetCategory(Guid categoryId)
        {
            return await _inventoryUnitOfWork.CategoryRepository.GetByIdAsync(categoryId);
        }

        public async Task<IList<Category>> GetCategories()
        {
            return await _inventoryUnitOfWork.CategoryRepository.GetAllAsync();
        }

        public void InsertCategory(Category category)
        {
            if (!_inventoryUnitOfWork.CategoryRepository.IsTitleDuplicate(category.Name))
            {
                _inventoryUnitOfWork.CategoryRepository.Add(category);
                _inventoryUnitOfWork.Save();
            }
        }

        public void UpdateCategory(Category existingCategory)
        {
            if (!_inventoryUnitOfWork.CategoryRepository.IsTitleDuplicate(existingCategory.Name, existingCategory.Id))
            {
                _inventoryUnitOfWork.CategoryRepository.Edit(existingCategory);
                _inventoryUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Category name should be unique.");
        }

        public void DeleteCategory(Guid id)
        {
            _inventoryUnitOfWork.CategoryRepository.Remove(id);
            _inventoryUnitOfWork.Save();
        }
    }
}