using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.ViewModels;

namespace Cursus.Application.Profile
{
    public interface IProfileService
    {
        ProFileView GetData(int accountId, string id);
    }
}