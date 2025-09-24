using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemManagement.Application.Common.DTO
{
    public class ErrorResponseDto
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
