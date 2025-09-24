using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemManagement.Application.Common.DTO
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }

      
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsBeingDeleted { get; set; }
        //public List<ItemDto> Items { get; set; } = new List<ItemDto>();
    }
}
