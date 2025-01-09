using Inventory.Domain.Entities;
using Inventory.Application.Services;
using Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductManagementService _productManagementService;
        public ProductController(IProductManagementService productManagementService)
        {
            _productManagementService = productManagementService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Insert()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Insert(ProductInsertModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Category = model.Category,
                    MeasurementUnit = model.MeasurementUnit,
                    StockQuantity = model.StockQuantity,
                    BuyingPrice = model.BuyingPrice,
                    SellingPrice = model.SellingPrice,
                    Tax = model.Tax,
                    SellingWithTax = model.SellingWithTax,
                    Barcode = model.Barcode,
                    Description = model.Description,
                    Status = model.Status,
                    //ImageUrl = model.ImageUrl,
                };
                _productManagementService.InsertProduct(product);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}