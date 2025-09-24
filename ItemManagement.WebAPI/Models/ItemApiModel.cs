using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace ItemManagement.WebAPI.Models
{
    public class ItemApiModel
    {
        public int ItemId { get; set; }
        [Required(ErrorMessage = "Name is required")] public string?  Name { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
        public string? CategoryName { get; set; }
        public int? CategoryId { get; set; }
        public List<SelectListItem>? Categories { get; set; }  // Populate in controller for dropdown
 

    }
}
