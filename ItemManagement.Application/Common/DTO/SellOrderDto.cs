using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemManagement.Application.Common.DTO
{
    public class SellOrderDto
    {
        public string CashierName { get; set; }
        public int ItemId { get; set; }
        public int SellQty { get; set; }
    }
}
