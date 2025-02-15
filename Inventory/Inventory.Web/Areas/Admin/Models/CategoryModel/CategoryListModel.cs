using Inventory.Domain;
using Inventory.Domain.Entities;

namespace Inventory.Web.Areas.Admin.Models.CategoryModel
{
    public class CategoryListModel : DataTables
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}
