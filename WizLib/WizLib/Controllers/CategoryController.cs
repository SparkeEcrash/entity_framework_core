using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WizLib_DataAccess;
using WizLib_Model.Models;
using Microsoft.EntityFrameworkCore;

namespace WizLib.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            //No tracking queries are useful when the results are used in a read-only scenario.
            //They're quicker to execute because there's no need to set up the change tracking information
            List<Category> objList = _db.Categories.AsNoTracking().ToList();
            return View(objList);
        }
        public IActionResult Upsert(int? id) //? makes this parameter nullable
        {
            Category obj = new Category();
            if(id == null)
            {
                return View(obj);
            }
            //this for edit
            obj = _db.Categories.FirstOrDefault(u => u.Category_Id == id);
            if(obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Category_Id == 0)
                {
                    _db.Categories.Add(obj);
                }
                else
                {
                    _db.Categories.Update(obj);
                }
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public IActionResult Delete(int id)
        {
            var objFromDb = _db.Categories.FirstOrDefault(u => u.Category_Id == id);
            _db.Categories.Remove(objFromDb);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult CreateMultiple2()
        {
            List<Category> catList = new List<Category>();
            for(int i = 1; i <= 2; i++)
            {
                catList.Add(new Category { Name = Guid.NewGuid().ToString() });
                //_db.Categories.Add(new Category { Name = Guid.NewGuid().ToString() });
            }
            _db.Categories.AddRange(catList);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult CreateMultiple5()
        {
            for (int i = 1; i <= 5; i++)
            {
                _db.Categories.Add(new Category { Name = Guid.NewGuid().ToString() });
            }
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult RemoveMultiple2()
        {
            IEnumerable<Category> catList = _db.Categories.OrderByDescending(u => u.Category_Id).Take(2).ToList();

            _db.Categories.RemoveRange(catList);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult RemoveMultiple5()
        {
            IEnumerable<Category> catList = _db.Categories.OrderByDescending(u => u.Category_Id).Take(5).ToList();

            _db.Categories.RemoveRange(catList);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}