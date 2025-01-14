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
                model.Search, model.FormatSortExpression("Name", "Barcode", "Category", "Tax", "SellingWithTax", "StockQuantity", "Status", "Id"));

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
                                record.Id.ToString(),
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

        public IActionResult Update(Guid id)
        {
            var model = new ProductUpdateModel();
            Product product = _productManagementService.GetProduct(id);

            model.Id = product.Id;
            model.Name = product.Name;
            model.Category = product.Category;
            model.Barcode = product.Barcode;
            model.MeasurementUnit = product.MeasurementUnit;
            model.StockQuantity = product.StockQuantity;
            model.BuyingPrice = product.BuyingPrice;
            model.SellingPrice = product.SellingPrice;
            model.Tax = product.Tax;
            model.SellingWithTax = product.SellingWithTax;
            model.Status = product.Status;
            model.Description = product.Description;

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(ProductUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Category = model.Category,
                    Barcode = model.Barcode,
                    MeasurementUnit = model.MeasurementUnit,
                    BuyingPrice = model.BuyingPrice,
                    SellingPrice = model.SellingPrice,
                    Tax = model.Tax,
                    SellingWithTax = model.SellingWithTax,
                    Status = model.Status,
                    Description = model.Description,
                };
                //if (_productManagementService.ProductExists(model.Name, model.Id))
                //{
                //    TempData.Put("ResponseMessage", new ResponseModel
                //    {
                //        Message = "Product name should be unique",
                //        Type = ResponseTypes.Danger
                //    });
                //    return View(model);
                //}
                try
                {
                    _productManagementService.UpdateProduct(product);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product updated successfuly",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product update failed",
                        Type = ResponseTypes.Danger
                    });
                    _logger.LogError(ex, "Product update failed");
                }
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _productManagementService.DeleteProduct(id);
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Product deleted successfuly",
                    Type = ResponseTypes.Success
                });
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Product delete failed",
                    Type = ResponseTypes.Danger
                });
                _logger.LogError(ex, "Product delete failed");
            }
            return View();
        }
    }
}