using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cursus.Application.AdminDashBoard
{
    public interface IAdminDashBoardRepository
    {
        Cursus.Domain.Models.AdminDashBoard GetData();       
    }
}