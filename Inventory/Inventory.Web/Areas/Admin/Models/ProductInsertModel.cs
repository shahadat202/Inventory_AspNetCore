using Inventory.Domain.Entities;
using Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventory.Web.Areas.Admin.Models
{
    public class ProductInsertModel
    {
        public string Name { get; set; }
        public string MeasurementUnit { get; set; }
        public int StockQuantity { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal Tax { get; set; }
        public decimal SellingWithTax { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

        public Guid CategoryId { get; set; }
        public IList<SelectListItem>? Categories { get; private set; }

        public void SetCategoryValues(IList<Category> categories)
        {
            Categories = RazorUtility.ConvertCategories(categories);
        }

        //public string ImageUrl { get; set; }
    }
}