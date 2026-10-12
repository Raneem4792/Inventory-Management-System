using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TestWebApplication.Models;
using TestWebApplication.Repository.Base;
using TestWebApplication.Models.ViewModels;

namespace TestWebApplication.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {

            var model = new DashboardVM();
            var items = _unitOfWork.items.FindAll();
            model.TotalItems = items.Count();
            model.TotalCategories = _unitOfWork.categories.FindAll().Count();
            model.TotalEmployees = _unitOfWork.employees.FindAll().Count();
            model.LowStackList = items.Where(x => x.Quantity <= 5).ToList();
            model.LowStockItems = model.LowStackList.Count();
            model.LatestTransactions = _unitOfWork.stockTransactions.FindAll().OrderByDescending(t => t.CreatedDate).Take(5);
            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
