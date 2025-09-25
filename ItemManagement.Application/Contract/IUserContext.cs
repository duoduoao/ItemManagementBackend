using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemManagement.Application.Contract
{
    public interface IUserContext
    {
        bool IsAuthenticated { get; }
        string UserId { get; }
        string UserName { get; }  // Optionally, or other user info as needed
    }
}
