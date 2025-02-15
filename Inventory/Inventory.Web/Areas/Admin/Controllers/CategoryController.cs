using Inventory.Domain.Entities;
using Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Inventory.Infrastructure;
using Inventory.Application.ServiceInterface;
using Inventory.Web.Areas.Admin.Models.CategoryModel;

namespace Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryManagementService _categoryManagementService;
        public CategoryController(ILogger<CategoryController> logger,
            ICategoryManagementService categoryManagementService)
        {
            _logger = logger;
            _categoryManagementService = categoryManagementService;
        }
        public async Task<IActionResult> Index()
        {
            var model = new CategoryListModel
            {
                Categories = await _categoryManagementService.GetCategories()
            };
            return View(model);
        }

        public IActionResult Insert()
        {
            return View(new CategoryInsertModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Insert(CategoryInsertModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name
                };

                try
                {
                    _categoryManagementService.InsertCategory(category);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Category inserted successfully",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Category insertion failed",
                        Type = ResponseTypes.Danger
                    });
                    _logger.LogError(ex, "Category insertion failed");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var category = await _categoryManagementService.GetCategory(id);
            var model = new CategoryUpdateModel()
            {
                Id = category.Id,
                Name = category.Name,
            };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingCategory = await _categoryManagementService.GetCategory(model.Id);
                    if (existingCategory == null)
                    {
                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                            Message = "Category not found.",
                            Type = ResponseTypes.Danger
                        });
                        return RedirectToAction("Index");
                    }
                    existingCategory.Name = model.Name;

                    _categoryManagementService.UpdateCategory(existingCategory);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Category updated successfully",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Category update failed",
                        Type = ResponseTypes.Danger
                    });
                    _logger.LogError(ex, "Category update failed");
                }
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                _categoryManagementService.DeleteCategory(id);
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Category deleted successfuly",
                    Type = ResponseTypes.Success
                });
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Category delete failed",
                    Type = ResponseTypes.Danger
                });
                _logger.LogError(ex, "Category deleted failed");
            }
            return View();
        }
    }
}
