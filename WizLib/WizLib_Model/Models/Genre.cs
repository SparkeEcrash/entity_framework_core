using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WizLib_Model.Models
{
    [Table("tb_Genre")]
    public class Genre
    {
        public int GenreId { get; set; } //naming convention of entity framework takes "Id" to set this column as the id column
        [Column("Name")]
        public string GenreName { get; set; }
        //public int DisplayOrder { get; set; }
    }
}
