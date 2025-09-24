using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemManagement.Application.Common.DTO
{
    public class ItemDto
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }  // flattened for easy display
        public int Quantity { get; set; }
        public double Price { get; set; }
       
        public string ImageUrl { get; set; }

    
    }

}
