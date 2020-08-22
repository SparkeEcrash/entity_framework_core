using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WizLib_DataAccess;
using WizLib_Model.Models;
using WizLib_Model.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace WizLib.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;
        
        public BookController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            /*ONE WAY: THIS IS THE STEP WITHOUT GETING MANY PULLING COLLECTION OF AUTHORS*/
            //BELOW IS USING EAGER LOADING
            //List<Book> objList = _db.Books.Include(u => u.Publisher).ToList();

            /*It is best to avoid the below snippet and use .Include (using Microsoft.EntityFrameWorkCore necessary) in order to use a JOIN statement when executing SQL queries
             * 
             * 
            List<Book> objList = _db.Books.ToList();
            foreach(var obj in objList)
            {
                //Least Efficient
                //obj.Publisher = _db.Publishers.FirstOrDefault(u => u.Publisher_Id == obj.Publisher_Id);
                
                //Explicit Loading More Efficient
                _db.Entry(obj).Reference(u => u.Publisher).Load();
                //the above is for bulk querying so that multiple SQL queries is not used
            }
            */

            /*SECOND WAY: THIS IS THE STEP IF YOU NEED TO PULL A COLLECTION OF AUTHORS ALONG WITH THE BOOK ASSOCIATED*/
            //BELOW IS USING EAGER LOADING
            List<Book> objList = _db.Books.Include(u => u.Publisher).Include(u => u.BookAuthors).ThenInclude(u=>u.Author).ToList();

            /*It is best to avoid the below snippet and use .Include (using Microsoft.EntityFrameWorkCore necessary) in order to use a JOIN statement when executing SQL queries
             * 
             * 
            List<Book> objList = _db.Books.ToList();
            foreach(var obj in objList)
            {
                _db.Entry(obj).Reference(u => u.Publisher).Load();
                _db.Entry(obj).Collection(u => u.BookAuthors).Load();
                foreach(var bookAuth in obj.BookAuthors)
                {
                    _db.Entry(bookAuth).Reference(u => u.Author).Load();
                }
            }
            */
            return View(objList);
        }

        public IActionResult Upsert(int? id)
        {
            BookVM obj = new BookVM();
            obj.PublisherList = _db.Publishers.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Publisher_Id.ToString()
            });
            if (id == null)
            {
                return View(obj);
            }
            //this for edit
            obj.Book = _db.Books.FirstOrDefault(u => u.Book_Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BookVM obj)
        {
                if (obj.Book.Book_Id == 0)
                {
                    _db.Books.Add(obj.Book);
                }
                else
                {
                    _db.Books.Update(obj.Book);
                }
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            BookVM obj = new BookVM();

            if (id == null)
            {
                return View(obj);
            }
            //this for edit
            obj.Book = _db.Books.Include(u => u.BookDetail).FirstOrDefault(u => u.Book_Id == id);
            /*It is best to avoid the below snippet and use .Include (using Microsoft.EntityFrameWorkCore necessary) in order to use a JOIN statement when executing SQL queries
             * 
             * 
            obj.Book = _db.Books.FirstOrDefault(u => u.Book_Id == id);
            obj.Book.BookDetail = _db.BookDetails.FirstOrDefault(u => u.BookDetail_Id == obj.Book.BookDetail_Id);
            */
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(BookVM obj)
        {
            if (obj.Book.BookDetail.BookDetail_Id == 0)
            {
                //this is create
                _db.BookDetails.Add(obj.Book.BookDetail);
                _db.SaveChanges();

                var BookFromDb = _db.Books.FirstOrDefault(u => u.Book_Id == obj.Book.Book_Id);
                BookFromDb.BookDetail_Id = obj.Book.BookDetail.BookDetail_Id;
                _db.SaveChanges();
            }
            else
            {
                //this is an update
                _db.BookDetails.Update(obj.Book.BookDetail);
                _db.SaveChanges();
            }
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var objFromDb = _db.Books.FirstOrDefault(u => u.Book_Id == id);
            _db.Books.Remove(objFromDb);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ManageAuthors(int id)
        {
            BookAuthorVM obj = new BookAuthorVM
            {
                BookAuthorList = _db.BookAuthors.Include(u => u.Author).Include(u => u.Book).Where(u => u.Book_Id == id).ToList(),
                BookAuthor = new BookAuthor()
                {
                    Book_Id = id
                },
                Book = _db.Books.FirstOrDefault(u => u.Book_Id == id)
            };
            List<int> tempListOfAssignedAuthors = obj.BookAuthorList.Select(u => u.Author_Id).ToList();
            var tempList = _db.Authors.Where(u => !tempListOfAssignedAuthors.Contains(u.Author_Id)).ToList();

            obj.AuthorList = tempList.Select(i => new SelectListItem
            {
                Text = i.FullName,
                Value = i.Author_Id.ToString()
            });

            return View(obj);
        }
        [HttpPost]
        public IActionResult ManageAuthors(BookAuthorVM bookAuthorVM)
        {
            if(bookAuthorVM.BookAuthor.Book_Id!=0 && bookAuthorVM.BookAuthor.Author_Id != 0)
            {
                _db.BookAuthors.Add(bookAuthorVM.BookAuthor);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(ManageAuthors), new { @id = bookAuthorVM.BookAuthor.Book_Id });
        }
        [HttpPost]
        public IActionResult RemoveAuthors(int authorId, BookAuthorVM bookAuthorVM)
        {
            int bookId = bookAuthorVM.Book.Book_Id;
            BookAuthor bookAuthor = _db.BookAuthors.FirstOrDefault(
                u => u.Author_Id == authorId && u.Book_Id == bookId);

            _db.BookAuthors.Remove(bookAuthor);
            _db.SaveChanges();
            return RedirectToAction(nameof(ManageAuthors), new { @id = bookId });
        }
        public IActionResult PlayGround()
        {
            var bookTemp = _db.Books.FirstOrDefault();
            bookTemp.Price = 100;

            var bookCollection = _db.Books;
            double totalPrice = 0;

            foreach (var book in bookCollection)
            {
                totalPrice += book.Price;
            }

            var bookList = _db.Books.ToList();
            foreach (var book in bookList)
            {
                totalPrice += book.Price;
            }

            var bookCollection2 = _db.Books;
            var bookCount1 = bookCollection2.Count();

            var bookCount2 = _db.Books.Count();

            //SQL QUERY DOES NOT HAVE A WHERE CLAUSE AND FILTERING IS DONE IN SYSTEM MEMORY
            IEnumerable<Book> BookList1 = _db.Books;
            var filteredBook1 = BookList1.Where(b => b.Price > 500).ToList();

            //SQL QUERY DOES HAVE A WHERE CLAUSE AND THERE IS NO ADDITIONAL FILTERING DONE BY THE SYSTEM
            IQueryable<Book> BookList2 = _db.Books;
            var filteredBook2 = BookList2.Where(b => b.Price > 500).ToList();

            //You can manually change the entity state so that _db.SaveChanges() can run
            //var category = _db.Categories.FirstOrDefault();
            //_db.Entry(category).State = EntityState.Modified;
            //_db.SaveChanges();



            //Updating Related Data
            var bookTemp1 = _db.Books.Include(b => b.BookDetail).FirstOrDefault(b => b.Book_Id == 4);
            bookTemp1.BookDetail.NumberOfChapters = 2222;
            //Books.Update saves ALL the properties for the Book model all over again even if some have not changed
            _db.Books.Update(bookTemp1);
            _db.SaveChanges();

            var bookTemp2 = _db.Books.Include(b => b.BookDetail).FirstOrDefault(b => b.Book_Id == 4);
            bookTemp2.BookDetail.Weight = 3333;
            //Books.Attach saves ONLY the properties that have changed and do not set properties that have not changed
            _db.Books.Attach(bookTemp2);
            _db.SaveChanges();


            /*EXAMPLES FOR PULLING DATA FROM VIEWS AND USING MYSQL QUERIES AND MYSQL PROCEDURES*/

            //VIEWS
            //VIEWS DO NOT HAVE PRIMARY KEYS SO ENTITY CORE WILL NEVER TRACK CHANGES IN THESE DATA
            //USING VIEWS TO PULL DATA IS FOR READ PURPOSES ONLY
            var ViewList = _db.BookDetailsFromViews.ToList();
            var ViewList1 = _db.BookDetailsFromViews.FirstOrDefault();
            var ViewList2 = _db.BookDetailsFromViews.Where(u=>u.Price>500);

            //RAW SQL
            var bookRaw = _db.Books.FromSqlRaw("Select * from dbo.books").ToList();
            //you can only pull all the columns * and not specific columns

            //protected from SQL Injection attack
            int id = 1;
            var bookTempSql1 = _db.Books.FromSqlInterpolated($"Select * from dbo.books where Book_Id={id}").ToList();

            var bookSproc = _db.Books.FromSqlInterpolated($" EXEC dbo.getAllBookDetails {id}").ToList();

            //.NET 5 only
            var BookFilter1 = _db.Books.Include(e => e.BookAuthors.Where(p => p.Author_Id == 5)).ToList();
            var BookFilter2 = _db.Books.Include(e => e.BookAuthors.OrderByDescending(p => p.Author_Id).Take(2)).ToList();

            return RedirectToAction(nameof(Index));
        }
    }
}