using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
  public class Category_model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int parent_id { get; set; }
        [MaxLength(100, ErrorMessage = "[Code] must be a string with a maximum length of '100'")]
        public string code { get; set; }
        [MaxLength(100, ErrorMessage = "[Code] must be a string with a maximum length of '100'")]
        public string name { get; set; }
        public string description { get; }
        public DateTime? created_datetime { get; }
        public int? created_by { get; }
        public DateTime? updated_datetime { get; }
        public int? updated_by { get; }
    }
    public class DisplayCategory_model
    {
        public int id { get; }
        public string parent_name { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int parent_id { get; set; }
    }
    public class CategoryParent_model
    {
        public int id { get; set; }
        public int parent_id { get; set; }
        public string name { get; set; }
    }
    public class SearchCategory_model
    {
        public string code { get; set; }
        public string name { get; set; }
        public int? parent_id { get; set; }
    }
}
