using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class Item_model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public int category_id { get; set; }
        public string category_name { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "[Code] must be a string with a maximum length of '100'")]
        public string code { get; set; }
        [MaxLength(100, ErrorMessage = "[Code] must be a string with a maximum length of '100'")]
        public string name { get; set; }
        public string specification { get; set; }
        public string description { get; set; }
        [Required]
        public bool dangerous { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? discontinued_datetime { get; set; }
        public int? inventory_measure_id { get; set; }
        public string inventory_measure_name { get; set; }
        public int? inventory_expired { get; set; }
        public double? inventory_standard_cost { get; set; }
        public double? inventory_list_price { get; set; }
        public double? manufacture_day { get; set; }
        public int? manufacture_make { get; set; }
        public int? manufacture_tool { get; set; }
        public int? manufacture_finished_goods { get; set; }
        public string manufacture_size { get; set; }
        public int? manufacture_size_measure_id { get; set; }
        public string manufacture_size_measure_name { get; set; }
        public string manufacture_weight { get; set; }
        public int? manufacture_weight_measure_id { get; set; }
        public string manufacture_weight_measure_name { get; set; }
        public string manufacture_style { get; set; }
        public string manufacture_class { get; set; }
        public string manufacture_color { get; set; }
        public DateTime? created_datetime { set; get; }
        public int? created_by { set; get; }
        public DateTime? updated_datetime { set; get; }
        public int? updated_by { set; get; }
    }
    public class DisplayItem_model
    {
        public int id { get; set; }
        public string category_name { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string specification { get; set; }
        public string description { get; set; }
        public int dangerous { get; set; }
    }
    public class ListItem_model
    {
        public int? id { get; set; }
        public int? category_id { get; set; }
        public string category_name { get; set; }
        public string code { get; set; }
        public string name { get; set; }
    }
    public class GetCatetory_model
    {
        public int? id { get; set; }

        public string name { get; set; }
    }
    public class GetMeasure_model
    {
        public int? id { get; set; }
        public string name { get; set; }
    }
}
