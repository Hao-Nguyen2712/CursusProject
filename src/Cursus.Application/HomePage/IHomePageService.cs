using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.ViewModels;


namespace Cursus.Application
{
    public interface IHomePageService
    {
        HomePageView GetData(int accountId, string id);
        public HomePageView GetDataUnauthenticated();

	}
}