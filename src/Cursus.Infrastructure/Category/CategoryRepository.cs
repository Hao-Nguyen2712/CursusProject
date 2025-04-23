using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Formats.Asn1;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Application.Category;
using Cursus.Domain.Models;

namespace Cursus.Infrastructure.Category
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CursusDBContext dBContext;
        public CategoryRepository(CursusDBContext _dBContext)
        {
            this.dBContext = _dBContext;
        }

        public bool AddCategory(Domain.Models.Category category)
        {
            var categoryName = dBContext.Categories.Where(c => c.CategoryName == category.CategoryName).ToList();
            if (!categoryName.Any())
            {
                category.CategoryStatus = "Active";
                dBContext.Categories.Add(category);
                dBContext.SaveChanges();
                return true;

            }

            else
            {
                int flag = 0;
                for (int i = 0; i < categoryName.Count; i++)
                {
                    if (categoryName[i].CategoryStatus == "Active")
                    {
                        flag++;
                        break;
                    }
                }
                if (flag == 0)
                {
                    category.CategoryStatus = "Active";
                    dBContext.Categories.Add(category);
                    dBContext.SaveChanges();
                    return true;

                }
            }

            return false;
        }

        public bool DeleteCategory(int id)
        {
            var categories = FindCategoryById(id);
            if (categories != null)
            {
                categories.CategoryStatus = "InActive";
                dBContext.Update(categories);
                dBContext.SaveChanges();
                return true;
            }
            return false;
        }

        public Domain.Models.Category FindCategoryById(int id)
        {
            var category = dBContext.Categories.FirstOrDefault(c => c.CategoryId == id);
            return category;
        }

        public List<Domain.Models.Category> GetAllCategories()
        {
            var categories = dBContext.Categories.ToList();
            return categories;
        }

        public bool UpdateCategory(Cursus.Domain.Models.Category category)
        {

            if (category != null)
            {
                var categoryName = dBContext.Categories.Where(c => c.CategoryName == category.CategoryName).ToList();

                if (!categoryName.Any())
                {
                    dBContext.Categories.Update(category);
                    dBContext.SaveChanges();
                    return true;
                }
                else
                {
                    int flag = 0;
                    for (int i = 0; i < categoryName.Count; i++)
                    {
                        if (categoryName[i].CategoryStatus == "Active")
                        {
                            flag++;
                            break;
                        }
                    }
                    if (flag == 0)
                    {
                        dBContext.Categories.Update(category);
                        dBContext.SaveChanges();
                        return true;

                    }
                }

            }
            return false;
        }

        public List<Domain.Models.Course> GetCategory(int categoryId)
        {
            return dBContext.Courses.Where(c => c.CategoryId == categoryId).ToList();
        }
    }
}
