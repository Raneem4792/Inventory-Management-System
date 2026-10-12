using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestWebApplication.Models;
using TestWebApplication.Models.ViewModels;
using TestWebApplication.Repository.Base;


namespace TestWebApplication.Controllers
{
    [Authorize]
    public class StockTransactionsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StockTransactionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var transactions = _unitOfWork.stockTransactions.FindAll("Item");
            return View(transactions);
        }

        public IActionResult New(int? itemId)
        {
            ViewBag.items = new SelectList(_unitOfWork.items.FindAll(), "Id", "Name", itemId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New(StockTransactionsVM model)
        {
            if (ModelState.IsValid)
            {
                var item = _unitOfWork.items.FindById(model.ItemId);
                if (item == null)
                {
                    ModelState.AddModelError("ItemId", "Invalid item selected.");
                    ViewBag.items = new SelectList(_unitOfWork.items.FindAll(), "Id", "Name");
                    return View(model);
                }
                if (model.Type=="IN")
                {
                    item.Quantity += model.Quantity;
                }
                else if (model.Type == "OUT")
                {
                    if (item.Quantity < model.Quantity)
                    {
                        ModelState.AddModelError("Quantity", "Not enough stock available.");
                        ViewBag.items = new SelectList(_unitOfWork.items.FindAll(), "Id", "Name");
                        return View(model);
                    }
                    item.Quantity -= model.Quantity;
                }
                else
                {
                    ModelState.AddModelError("Type", "Invalid transaction type.");
                    ViewBag.items = new SelectList(_unitOfWork.items.FindAll(), "Id", "Name");
                    return View(model);
                }
                var transaction = new StockTransaction
                {
                    ItemId = model.ItemId,
                    Quantity = model.Quantity,
                    Type = model.Type
                };
                _unitOfWork.items.UpdateOne(item);
                _unitOfWork.stockTransactions.AddOne(transaction);

                TempData["successData"] = "Transaction has been completed successfully";

                return RedirectToAction("Index");
            }

            ViewBag.items = new SelectList(_unitOfWork.items.FindAll(), "Id", "Name");
            return View(model);
        }
    }
}
