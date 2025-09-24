using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemManagement.WebAPI.Models
{
    public class CategoryApiModel
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
