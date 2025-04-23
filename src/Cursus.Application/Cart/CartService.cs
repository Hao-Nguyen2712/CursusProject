using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Application.Cart;
using Cursus.Domain.Models;

namespace Cursus.Application.Cart
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _db;
        public CartService(ICartRepository db)
        {
            _db = db;
        }

        public void AddToCart(Domain.Models.Cart cart)
        {
            _db.AddToCart(cart);
        }

        public bool BuyNow(int accountId, int courseId)
        {
            return _db.BuyNow(accountId, courseId);
        }

        public bool Checkout(int accountId)
        {
            return _db.Checkout(accountId);
        }

        public List<Domain.Models.Cart> GetCartByAccountId(int accountId)
        {
            return _db.GetCartByAccountId(accountId);
        }

        public bool IsCoursePurchased(int accountId, int courseId)
        {
            return _db.IsCoursePurchased(accountId, courseId);
        }

        public void RemoveFromCart(int cartId)
        {
            _db.RemoveFromCart(cartId);
        }
    }
}