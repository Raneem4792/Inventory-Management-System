using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestWebApplication.Repository.Base;
using TestWebApplication.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace TestWebApplication.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {

        private readonly IUnitOfWork unitOfWork;

        private readonly IHostingEnvironment _host;

        public ItemsController(IUnitOfWork unit, IHostingEnvironment host)
        {
            unitOfWork = unit;
            _host = host;
        }

        public async Task<IActionResult> Index( string search, int? categoryId)
        {
            var itemsList = await unitOfWork.items.FindAllAsync("Category");

            if (!string.IsNullOrEmpty(search))
            {
                itemsList = itemsList.Where(x => x.Name.Contains(search)).ToList();
            }
            if (categoryId.HasValue)
            {
                itemsList = itemsList.Where(x => x.CategoryId == categoryId.Value).ToList();
            }

            ViewBag.categories = unitOfWork.categories.FindAll();

            return View(itemsList);
        }

        [Authorize(Roles = "Admin")]
        //GET
        public IActionResult New()
        {
            createSelectList();
            return View();
        }

        //POST
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult New(Item item)
        {
            if (ModelState.IsValid)
            {
                string fileName=string.Empty;
                if (item.clientFile != null) 
                {
                    string myUpload = Path.Combine( _host.WebRootPath, "images");
                    fileName = item.clientFile.FileName;
                    string fullPath = Path.Combine( myUpload, fileName );
                    item.clientFile.CopyTo(new FileStream(fullPath, FileMode.Create));
                    item.imagePath = fileName;
                
                }

                unitOfWork.items.AddOne(item);
                TempData["successData"] = "Item has been added successfully";
                return RedirectToAction("Index");
            }
            else
                return View(item);
        }

        //GET
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var item = unitOfWork.items.FindById(Id.Value);
            if (item == null)
            {
                return NotFound();
            }

            createSelectList(item.CategoryId);
            return View(item);
        }

        public void createSelectList(int selectId=1)
        {
            //List<Category> categories = new List<Category>
            //{
            //    new Category(){Id=0, Name="Select Category"},
            //    new Category(){Id=1, Name="Computers"},
            //    new Category(){Id=2, Name="Mobiles"},
            //    new Category(){Id=3, Name="Electric Machines"},
            //};

            List<Category> categories = unitOfWork.categories.FindAll().ToList();
            SelectList listItems = new SelectList(categories, "Id", "Name", selectId);
            ViewBag.CategoryList = listItems;
        }

        //POST

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Item item)
        {
            if (!ModelState.IsValid)
            {
                createSelectList(item.CategoryId);
                return View(item);
            }

            var existingItem = unitOfWork.items.FindById(item.Id);

            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = item.Name;
            existingItem.Price = item.Price;
            existingItem.CategoryId = item.CategoryId;

            if (item.clientFile != null)
            {
                string uploadPath = Path.Combine(_host.WebRootPath, "images");

                Directory.CreateDirectory(uploadPath);

                string fileName = Guid.NewGuid().ToString()
                                  + Path.GetExtension(item.clientFile.FileName);

                string fullPath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    item.clientFile.CopyTo(stream);
                }

                existingItem.imagePath = fileName;
            }

            unitOfWork.items.UpdateOne(existingItem);

            TempData["successData"] = "Item has been edited successfully";

            return RedirectToAction("Index");
        }

        //GET
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var item = unitOfWork.items.FindById(Id.Value);
            if (item == null)
            {
                return NotFound();
            }

            createSelectList(item.CategoryId);
            return View(item);
        }

        //POST
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteItem(int? Id)
        {
            var item = unitOfWork.items.FindById(Id.Value);
            if (item == null)
            {
                return NotFound();
            }
            unitOfWork.items.DeleteOne(item);
            TempData["successData"] = "Item has been deleted successfully";
            return RedirectToAction("Index");

        }
    }
}

