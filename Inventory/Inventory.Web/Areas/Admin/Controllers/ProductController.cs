using Inventory.Domain.Entities;
using Inventory.Domain.Dtos;
using Inventory.Application.Services;
using Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Inventory.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductManagementService _productManagementService;
        private readonly ICategoryManagementService _categoryManagementService;
        private readonly IMapper _mapper;
        public ProductController(ILogger<ProductController> logger,
            IProductManagementService productManagementService,
            ICategoryManagementService categoryManagementService,
            IMapper mapper)
        {
            _logger = logger;
            _productManagementService = productManagementService;
            _categoryManagementService = categoryManagementService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var model = new ProductListModel();
            model.SetCategoryValues(await _categoryManagementService.GetCategories());    
            return View(model);
        }

        public async Task<IActionResult> IndexUI()
        {
            var products = await _productManagementService.GetAllProductsAsync();
            var productViewModels = _mapper.Map<List<ViewProductModel>>(products);
            return View(productViewModels);
        }

        [HttpPost]
        public async Task<JsonResult> GetProductJsonDataSP([FromBody] ProductListModel model)
        {
            var result = await _productManagementService.GetProductsSP(model.PageIndex, model.PageSize,
                model.SearchItem, model.FormatSortExpression("Name", "Barcode", "CategoryName", "Tax", "SellingWithTax", "StockQuantity", "Status", "Id"));

            var productJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from record in result.data
                        select new string[]
                        {
                                HttpUtility.HtmlEncode(record.Name),
                                HttpUtility.HtmlEncode(record.Barcode),
                                HttpUtility.HtmlEncode(record.CategoryName),
                                HttpUtility.HtmlEncode(record.Tax),
                                HttpUtility.HtmlEncode(record.SellingWithTax),
                                HttpUtility.HtmlEncode(record.StockQuantity),
                                HttpUtility.HtmlEncode(record.Status),
                                record.Id.ToString(),
                                //record.InsertDate.ToString("yyyy-MM-dd"),
                        }
                    ).ToArray()
            };
            return Json(productJsonData);
        }

        public async Task<IActionResult> Insert()
        {
            var model = new ProductInsertModel();
            model.SetCategoryValues(await _categoryManagementService.GetCategories());
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Insert(ProductInsertModel model)
        {
            if (ModelState.IsValid)
            {
                var product = _mapper.Map<Product>(model);
                product.Id = Guid.NewGuid();
                product.Category = await _categoryManagementService.GetCategory(model.CategoryId);

                // Image upload logic
                product.Image = await UploadImage(model.Image);
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
            model.SetCategoryValues(await _categoryManagementService.GetCategories());
            return View();
        }

        public async Task<IActionResult> Update(Guid id)
        {
            Product product = await _productManagementService.GetProductByIdAsync(id);
            var model = _mapper.Map<ProductUpdateModel>(product);   
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _productManagementService.GetProductByIdAsync(model.Id);
                    existingProduct = _mapper.Map(model, existingProduct);
                    if (existingProduct == null)
                    {
                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                            Message = "Blog post not found.",
                            Type = ResponseTypes.Danger
                        });
                        return RedirectToAction("Index");
                    }

                    _productManagementService.UpdateProduct(existingProduct);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Blog post updated successfully",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Blog post update failed",
                        Type = ResponseTypes.Danger
                    });
                    _logger.LogError(ex, "Blog post update failed");
                }
            }
            //model.SetCategoryValues(_categoryManagementService.GetCategories());
            //return View(model);
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
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

        public async Task<IActionResult> ViewProduct(Guid id)
        {
            var product = await _productManagementService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            
            var productViewModel = _mapper.Map<ViewProductModel>(product);  
            return View(productViewModel);
        }

        // Image upload logic 
        private async Task<string> UploadImage(IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                var uploadsFolder = Path.Combine("wwwroot", "uploadedImages");
                Directory.CreateDirectory(uploadsFolder);

                // Generate a unique filename to avoid collisions
                var uniqueFileName = $"{Guid.NewGuid()}_{image.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream); 
                }
                return $"/uploadedImages/{uniqueFileName}";
            }
            return null; 
        }

    }
}