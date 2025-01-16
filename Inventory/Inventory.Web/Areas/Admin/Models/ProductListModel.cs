using Inventory.Domain;
using Inventory.Domain.Entities;
using Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventory.Web.Areas.Admin.Models
{
    public class ProductListModel : DataTables
    {
        public IList<SelectListItem> Categories { get; private set; }

        public void SetCategoryValues(IList<Category> categories)
        {
            Categories = RazorUtility.ConvertCategories(categories);
        }
    }
}
