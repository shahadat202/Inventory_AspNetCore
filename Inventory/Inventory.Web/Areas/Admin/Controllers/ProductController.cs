using Inventory.Domain.Entities;
using Inventory.Application.Services;
using Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Web;

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

        public JsonResult GetProductJsonResult(ProductListModel model)
        {
            var result = _productManagementService.GetProducts(model.PageIndex, model.PageSize, 
                model.Search, model.FormatSortExpression("Name"));

            var productJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from record in result.data
                        select new string[]
                        {
                                HttpUtility.HtmlEncode(record.Name),
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
            return Json(productJsonData);
        }

        public IActionResult Insert()
        {
            var model = new ProductInsertModel();
            return View(model);
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