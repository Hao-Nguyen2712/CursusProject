using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Cursus.Application.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository _categoryRepository)
        {
            categoryRepository = _categoryRepository;
        }

        public bool AddCategory(Domain.Models.Category category)
        {
            return categoryRepository.AddCategory(category);
        }

        public bool DeleteCategory(int id)
        {
            return categoryRepository.DeleteCategory(id);
        }

        public Domain.Models.Category FindCategoryById(int id)
        {
            return categoryRepository.FindCategoryById(id);
        }

        public List<Domain.Models.Category> GetAllCategories()
        {
            return categoryRepository.GetAllCategories();
        }



        public bool UpdateCategory(Domain.Models.Category category)
        {
            return categoryRepository.UpdateCategory(category);
        }

        public List<Domain.Models.Course> GetCategory(int categoryId)
        {
        return categoryRepository.GetCategory(categoryId);
        }
    }
}