using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Http;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using Cursus.Application.Cart;
using Cursus.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Cursus.Infrastructure.Cart
{
    public class CartRepository : ICartRepository
    {
        private readonly CursusDBContext _db;

        public CartRepository(CursusDBContext db)
        {
            _db = db;
        }

        public void AddToCart(Domain.Models.Cart cart)
        {
            if (cart == null)
            {
                return;
            }
            _db.Carts.Add(cart);
            _db.SaveChanges();
        }

        public bool BuyNow(int accountId, int courseId)
        {
            // Retrieve the course and instructor details
            var course = _db.Courses
            .Include(c => c.Account)  // Eager loading for instructor
            .FirstOrDefault(c => c.CourseId == courseId);

            if (course == null)
            {
                return false; // Course not found
            }

            // check if user already purchased the course
            var userEnrolledCourses = _db.Enrolls.Where(c => c.AccountId == accountId && c.CourseId == courseId).Any();
            if (userEnrolledCourses)
            {
                return false; // course already purchased
            }

            // Validate user's wallet balance
            var userWallet = _db.Accounts.Find(accountId); // Find user's wallet
            if (userWallet == null)
            {
                throw new InvalidOperationException("User wallet not found.");
            }

            decimal? totalAmount = course.CourseMoney * (1 - course.Discount / 100);
            if (userWallet.Money < totalAmount)
            {
                throw new InvalidOperationException("Insufficient funds, checkout fails"); // Insufficient funds, buy now fails
            }

            // Update wallet balances (using transactions for data integrity)
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    userWallet.Money -= totalAmount; // Deduct from user's wallet
                    _db.Accounts.Update(userWallet);

                    var instructorWallet = _db.Accounts.Find(course.Account.AccountId); // Find instructor's wallet
                    if (instructorWallet == null)
                    {
                        throw new InvalidOperationException("Instructor wallet not found.");
                    }

                    instructorWallet.Money += totalAmount;
                    _db.Accounts.Update(instructorWallet);

                    // Enroll user in purchased course
                    var enroll = new Domain.Models.Enroll
                    {
                        AccountId = accountId,
                        CourseId = courseId,
                        EnrollDate = DateTime.Now,
                        EnrollStatus = "Purchased",
                        EnrollBlock = "false",
                        EnrollFinish = "Not finish"
                    };

                    _db.Enrolls.Add(enroll);

                    _db.SaveChanges();
                    transaction.Commit(); // Commit changes if successful
                }
                catch (Exception)
                {
                    transaction.Rollback(); // Rollback on errors
                    throw;
                }
            }

            return true;
        }


        // checkout 
        public bool Checkout(int accountId)
        {
            // Fetch the user's cart and instructor details
            var cartItems = _db.Carts
                .Where(c => c.AccountId == accountId)
                .Include(c => c.Course)
                .ThenInclude(c => c.Account)
                .ToList();

            if (cartItems.Count == 0)
            {
                return false; // No items in cart, checkout fails
            }

            var userEnrolledCourses = _db.Enrolls.Where(e => e.AccountId == accountId).Select(e => e.CourseId).ToList();

            foreach (var cartItem in cartItems)
            {
                if (userEnrolledCourses.Contains(cartItem.CourseId))
                {
                    // Course already purchased, handle this case
                    return false;
                }
            }

            var userAccount = cartItems.First().Course.Account; // Instructor of first course

            // Validate user's wallet balance
            var userWallet = _db.Accounts.Find(accountId);
            if (userWallet == null)
            {
                throw new InvalidOperationException("User wallet not found.");
            }

            decimal? totalAmount = cartItems.Sum(c => c.Course.CourseMoney * (1 - c.Course.Discount / 100)); // Calculate total cost
            if (userWallet.Money < totalAmount)
            {
                throw new InvalidOperationException("Insufficient funds, checkout fails"); // Insufficient funds, checkout fails
            }

            // Update wallet balances (using transactions for data integrity)
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    userWallet.Money -= totalAmount; // Deduct from user's wallet
                    _db.Accounts.Update(userWallet);

                    var instructorWallet = _db.Accounts.Find(userAccount.AccountId); // Find instructor's wallet
                    if (instructorWallet == null)
                    {
                        throw new InvalidOperationException("Instructor wallet not found.");
                    }

                    instructorWallet.Money += totalAmount;
                    _db.Accounts.Update(instructorWallet);

                    _db.SaveChanges();
                    transaction.Commit(); // Commit changes if successful
                }
                catch (Exception)
                {
                    transaction.Rollback(); // Rollback on errors
                    throw;
                }
            }
            // Enroll user in purchased courses (within the transaction)
            foreach (var cartItem in cartItems)
            {
                var enroll = new Domain.Models.Enroll
                {
                    AccountId = accountId,
                    CourseId = cartItem.CourseId,
                    EnrollDate = DateTime.Now,
                    EnrollStatus = "Purchased",
                    EnrollBlock = "false",
                    EnrollFinish = "Not finish"
                };

                _db.Enrolls.Add(enroll);
            }

            _db.SaveChanges();

            // Remove items from cart after successful checkout
            cartItems.ForEach(c => _db.Carts.Remove(c));
            _db.SaveChanges();

            return true;
        }

        public List<Domain.Models.Cart> GetCartByAccountId(int accountId)
        {
            return _db.Carts.Where(c => c.AccountId == accountId)
                    .Include(c => c.Course).ThenInclude(c => c.Account)
                    .ToList();
        }

        public bool IsCoursePurchased(int accountId, int courseId)
        {
            return _db.Enrolls.Any(e => e.AccountId == accountId && e.CourseId == courseId);
        }

        public void RemoveFromCart(int cartId)
        {
            var cartItem = _db.Carts.Find(cartId);
            if (cartItem != null)
            {
                _db.Carts.Remove(cartItem);
                _db.SaveChanges();
            }
        }

    }
}
