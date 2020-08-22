using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace WizLib_Model.Models.ViewModels
{
    //this view model is necessary to set up the many (books) to many (author) relationship
    //this view is also necessary to set up the book page for ManageAuthors
    public class BookAuthorVM
    {
        public BookAuthor BookAuthor { get; set; }
        public Book Book { get; set; }
        public IEnumerable<BookAuthor> BookAuthorList { get; set; }
        public IEnumerable<SelectListItem> AuthorList { get; set; }
    }
}
