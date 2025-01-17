using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.RepositoryContracts
{
    public interface ICategoryRepository : IRepositoryBase<Category, Guid>
    {
        bool IsTitleDuplicate(string title, Guid? id = null);
    }
}
