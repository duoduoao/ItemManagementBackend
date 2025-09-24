using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemManagement.Application.Common
{
    public enum DeleteCategoryResult
    {
        Success,
        NotFound,
        HasLinkedItems,
        UnknownError

    }
}
