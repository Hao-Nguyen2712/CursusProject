using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cursus.Application.Category;
using Cursus.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Cursus.MVC.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        public CategoryViewComponent(ICategoryService _categoryService, IMapper _mapper)
        {
            this.categoryService = _categoryService;
            mapper = _mapper;
        }
        public IViewComponentResult Invoke()
        {

            var list = categoryService.GetAllCategories();



            List<CategoryViewModel> categories = mapper.Map<List<CategoryViewModel>>(list);
            return View("CategoryList", categories);
        }
    }
}
