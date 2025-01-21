using Inventory.Domain;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventory.Web.Areas.Admin.Models
{
    public class ProductListModel : DataTables
    {
        public ProductSearchDto SearchItem { get; set; }
        public IList<SelectListItem> Categories { get; private set; }

        //public void SetCategoryValues(IList<Category> categories)
        //{
        //    Categories = RazorUtility.ConvertCategories(categories);
        //}


        public void SetCategoryValues(IList<Category> categories)
        {
            Categories = (from c in categories
                          select new SelectListItem(c.Name, c.Id.ToString()))
                          .ToList();
            Categories.Insert(0, new SelectListItem("All", Guid.Empty.ToString()));
        }
    }
}
