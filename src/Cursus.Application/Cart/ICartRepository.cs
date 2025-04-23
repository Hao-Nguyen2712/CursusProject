using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cursus.Domain.Models;

namespace Cursus.Application.Cart
{
    public interface ICartRepository
    {
        public void AddToCart(Domain.Models.Cart cart);
        List<Domain.Models.Cart> GetCartByAccountId(int accountId);
        public void RemoveFromCart(int cartId);
        public bool Checkout(int accountId);
        bool IsCoursePurchased(int accountId, int courseId);
        public bool BuyNow(int accountId, int courseId);
    }
}