using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ItemManagement.Domain.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
 
        public string? Name { get; set; }
        public string? Description { get; set; }

        // navigation property for ef core
        public List<Item> Items { get; set; } = new List<Item>();//must be initialized to avoid null reference exceptions when accessed or iterated.
    }
}
