using Inventory.Domain.Entities;
using Inventory.Application.Services;
using Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Inventory.Infrastructure;

namespace Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductManagementService _productManagementService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(ILogger<ProductController> logger,
            IProductManagementService productManagementService)
        {
            _logger = logger;
            _productManagementService = productManagementService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetProductJsonData([FromBody] ProductListModel model)
        {
            var result = _productManagementService.GetProducts(model.PageIndex, model.PageSize, 
                model.Search, model.FormatSortExpression("Name", "Barcode", "Category", "Tax", "SellingWithTax", "StockQuantity", "Status"));

            var productJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from record in result.data
                        select new string[]
                        {
                                HttpUtility.HtmlEncode(record.Name),
                                HttpUtility.HtmlEncode(record.Barcode),
                                HttpUtility.HtmlEncode(record.Category),
                                HttpUtility.HtmlEncode(record.Tax),
                                HttpUtility.HtmlEncode(record.SellingWithTax),
                                HttpUtility.HtmlEncode(record.StockQuantity),
                                HttpUtility.HtmlEncode(record.Status),
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
                try
                {
                    _productManagementService.InsertProduct(product);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product inserted successfuly",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product insertion failed",
                        Type = ResponseTypes.Danger
                    });
                    _logger.LogError(ex, "Product insertion failed");
                }
            }
            return View();
        }
    
    }
}