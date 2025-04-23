using Cursus.Domain.Models;
using Cursus.Infrastructure.Cart;
using Cursus.Infrastructure.Enroll;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cursus.Tests.CartRepositoryTest
{
    public class CartRepositoryTest
    {
        private CartRepository _cartRepository;
        private CursusDBContext _context { get; set; } = null!;
        private Cart _cart;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<CursusDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            _context = new CursusDBContext(options);
            _cartRepository = new CartRepository(_context);
            _cart = new Cart
            {
                CartId = 1,
                CourseId = 1,
                AccountId = 1,
                CartMoney = 2000
            };

            _context.Carts.Add(_cart);
            _context.SaveChanges();

        }

        [Test]
        public void TestAddToCart_Success()
        {
            var itemCart = new Cart
            {
                CartId = 2,
                CourseId = 2,
                AccountId = 1,  
                CartMoney = 100
            };
            _cartRepository.AddToCart(itemCart);

            var result = _context.Carts.FirstOrDefault(c => c.CartId == 2);
            
            Assert.IsNotNull(result, "Course added to cart successfully.");
            Assert.AreEqual(itemCart.CourseId, result.CourseId, "");
            Assert.AreEqual(itemCart.AccountId, itemCart.AccountId, "The account ID should match.");
        }

        [Test]
        public void TestAddToCart_NullCart()
        {
            Assert.DoesNotThrow(() => _cartRepository.AddToCart(null));
        }


        [Test]
        public void TestRemoveFromCart_Success()
        {
            _cartRepository.RemoveFromCart(_cart.CartId);

            var result = _context.Carts.FirstOrDefault(c => c.CartId == _cart.CartId);

            Assert.IsNull(result, "Course should be removed from cart.");
        }


        [Test]
        public void TestCourseIsPurchased_WhenCourseIsPurchased_Success()
        {
            _context.Enrolls.Add(new Enroll {
                AccountId = 1,
                CourseId = 1,
                EnrollDate = DateTime.UtcNow,
                EnrollBlock = "false",
                EnrollStatus = "Purchased"
            });
            _context.SaveChanges();

            var result = _cartRepository.IsCoursePurchased(1, 1);
            Assert.IsTrue(result, "The course should be as purchased successful.");
        }

        [Test]
        public void TestCourseIsPurchased_WhenCourseIsPurchased_NotSuccess()
        {
            var result = _cartRepository.IsCoursePurchased(1, 1);
            Assert.IsFalse(result, "The course should not be as purchased successful.");
        }


        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
