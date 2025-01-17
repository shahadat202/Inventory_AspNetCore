﻿using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services
{
    public interface ICategoryManagementService
    {
        IList<Category> GetCategories();
        Category GetCategory(Guid categoryId);
        void InsertCategory(Category category);
        void UpdateCategory(Category existingCategory);
        void DeleteCategory(Guid id);
    }
}
