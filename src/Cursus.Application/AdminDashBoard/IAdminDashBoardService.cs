using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cursus.Application.AdminDashBoard
{
    public interface IAdminDashBoardService
    {
        Cursus.Domain.Models.AdminDashBoard GetData();
    }
}