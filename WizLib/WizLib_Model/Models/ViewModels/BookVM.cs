using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace WizLib_Model.Models.ViewModels
{
    //this view model is necessary to set up the many (books) to one (publisher) relationship
    //this view is also necessary to set up the book page for Upset
    public class BookVM
    {
        public Book Book { get; set; }
        public IEnumerable<SelectListItem> PublisherList { get; set; }
    }
}
