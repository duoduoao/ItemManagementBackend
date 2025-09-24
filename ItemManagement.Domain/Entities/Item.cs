using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemManagement.Domain.Entities
{
    public class Item
    {
        public int ItemId { get; set; }
 
        public int? CategoryId { get; set; }
     
        public string? Name { get; set; }
     
        public int? Quantity { get; set; }
      
        public double? Price { get; set; }

        // navigation property for ef core
        public Category? Category { get; set; } 

   
        //public IFormFile? Image { get; set; }   // not saved to DB, used for upload binding

       
        public string? ImageUrl { get; set; }   // stored in DB as image reference (URL/blob name)
    }
}
