using Cursus.Application.Category;
using Cursus.Domain.Models;
using Cursus.Infrastructure.Category;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Tests.TestCategory
{
    public class CategoryRepositoryTest
    {
        private CategoryRepository _repository;
        private CursusDBContext context;
        private Category category;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CursusDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            context = new CursusDBContext(options);
            _repository =  new CategoryRepository(context);
            var categories = new List<Category>
            {
                new Category {CategoryId = 1 , CategoryName = "Math" , CategoryStatus = "Action"},
                new Category {CategoryId = 2 ,CategoryName = "History" , CategoryStatus= "Action"},
                new Category {CategoryId = 3 ,CategoryName = "Physics" , CategoryStatus= "Action"},
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();
        }

        [Test]
        public void GetAllCategories()
        {

            var result = _repository.GetAllCategories();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Math", result[0].CategoryName);
            Assert.AreEqual("History", result[1].CategoryName);
            Assert.AreEqual("Physics", result[2].CategoryName);
        }
        [Test]
        public void AddCategory()
        {
            // Arrange
            var newCategory = new Category {CategoryId = 4, CategoryName = "Science", CategoryStatus="Active" };

            // Act
            var result = _repository.AddCategory(newCategory);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(4, context.Categories.Count());

        }

        [Test]
        public void RemoveCategory()
        {
            var result = _repository.DeleteCategory(1);

            var test = context.Categories.FirstOrDefault(category => category.CategoryId == 1);
            Assert.IsNotNull(test, "Not found");
            Assert.AreEqual(3, context.Categories.Count());
        }
        [Test]
        public void UpdateCategory()
        {
            var test = context.Categories.FirstOrDefault(category => category.CategoryId == 1);
            test.CategoryName = "Physical";

            var update = _repository.UpdateCategory(test);

            
            Assert.AreEqual("Physical", test.CategoryName);
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
