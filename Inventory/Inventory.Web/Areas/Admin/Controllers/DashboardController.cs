using AutoMapper;
using Inventory.Application.Services;
using Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IProductManagementService _productManagementService;
        private readonly ICategoryManagementService _categoryManagementService;
        private readonly IMapper _mapper;
        public DashboardController(ILogger<DashboardController> logger,
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
            var model = new DashboardViewModel()
            {
                TotalItems = await _productManagementService.GetTotalItems(),
                TotalValue = await _productManagementService.GetTotalValue(),
            };
            return View(model);
        }
    }
}
