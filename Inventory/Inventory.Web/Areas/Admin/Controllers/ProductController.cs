﻿using Inventory.Domain.Entities;
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
        public IActionResult Index()
        {
            var model = new ProductListModel();
            model.SetCategoryValues(_categoryManagementService.GetCategories());    
            return View(model);
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

        public IActionResult Insert()
        {
            var model = new ProductInsertModel();
            model.SetCategoryValues(_categoryManagementService.GetCategories());
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Insert(ProductInsertModel model)
        {
            if (ModelState.IsValid)
            {
                var product = _mapper.Map<Product>(model);
                product.Id = Guid.NewGuid();
                product.Category = _categoryManagementService.GetCategory(model.CategoryId);  
                //var product = new Product()
                //{
                //    Id = Guid.NewGuid(),
                //    Name = model.Name,
                //    //InsertDate = DateTime.UtcNow,
                //    MeasurementUnit = model.MeasurementUnit,
                //    StockQuantity = model.StockQuantity,
                //    BuyingPrice = model.BuyingPrice,
                //    SellingPrice = model.SellingPrice,
                //    Tax = model.Tax,
                //    SellingWithTax = model.SellingWithTax,
                //    Barcode = model.Barcode,
                //    Description = model.Description,
                //    Status = model.Status,
                //    Category = _categoryManagementService.GetCategory(model.CategoryId),
                //    //ImageUrl = model.ImageUrl,
                //};
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
            model.SetCategoryValues(_categoryManagementService.GetCategories());
            return View();
        }

        public async Task<IActionResult> Update(Guid id)
        {
            Product product = await _productManagementService.GetProductAsync(id);
            var model = _mapper.Map<ProductUpdateModel>(product);   

            //model.Id = product.Id;
            //model.Name = product.Name;
            //model.Barcode = product.Barcode;
            //model.MeasurementUnit = product.MeasurementUnit;
            //model.StockQuantity = product.StockQuantity;
            //model.BuyingPrice = product.BuyingPrice;
            //model.SellingPrice = product.SellingPrice;
            //model.Tax = product.Tax;
            //model.SellingWithTax = product.SellingWithTax;
            //model.Status = product.Status;
            //model.Description = product.Description;

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the existing BlogPost from the database
                    var existingProduct = await _productManagementService.GetProductAsync(model.Id);
                    existingProduct = _mapper.Map(model, existingProduct);
                    //existingProduct.Category = _categoryManagementService.GetCategory(model.CategoryId);    
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