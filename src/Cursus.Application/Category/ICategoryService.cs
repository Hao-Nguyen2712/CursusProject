using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cursus.Application.Category
{
    public interface ICategoryService
    {
        public List<Cursus.Domain.Models.Category> GetAllCategories();
        public bool AddCategory(Cursus.Domain.Models.Category category);
        public bool UpdateCategory(Cursus.Domain.Models.Category category);
        public bool DeleteCategory(int id);
        public Cursus.Domain.Models.Category FindCategoryById(int id);
        List<Domain.Models.Course> GetCategory(int categoryId);


    }
}