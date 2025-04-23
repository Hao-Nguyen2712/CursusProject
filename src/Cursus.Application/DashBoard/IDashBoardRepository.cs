using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cursus.Application.DashBoard
{
    public interface IDashBoardRepository
    {
        Cursus.Domain.ViewModels.DashBoard GetData(int accountId,string id);
        
        
    }
}