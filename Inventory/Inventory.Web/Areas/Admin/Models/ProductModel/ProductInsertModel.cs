using Inventory.Domain.Entities;
using Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Web.Areas.Admin.Models.ProductModel
{
    public class ProductInsertModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string MeasurementUnit { get; set; }
        [Required]
        public int StockQuantity { get; set; }
        [Required]
        public decimal BuyingPrice { get; set; }
        [Required]
        public decimal SellingPrice { get; set; }
        [Required]
        public decimal Tax { get; set; }
        [Required]
        public decimal SellingWithTax { get; set; }
        [Required]
        public string Barcode { get; set; }
        [Required]
        public string Description { get; set; }
        public bool Status { get; set; }

        [Display(Name = "Category"), Required]
        public Guid CategoryId { get; set; }
        public IList<SelectListItem>? Categories { get; private set; }

        public void SetCategoryValues(IList<Category> categories)
        {
            Categories = RazorUtility.ConvertCategories(categories);
        }
        [Display(Name = "Product Image"), Required]
        public IFormFile Image { get; set; }

    }
}