using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WizLib_Model.Models
{
    public class Category
    {
        [Key]
        public int Category_Id { get; set; } //naming convention of entity framework takes "Id" to set this column as the id column
        public string Name { get; set; }
    }
}
