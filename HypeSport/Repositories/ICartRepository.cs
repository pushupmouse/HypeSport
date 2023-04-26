﻿using HypeSport.Models;

namespace HypeSport.Repositories
{
    public interface ICartRepository
    {
        Task<int> AddItem(int productId, int qty);
        Task<int> RemoveItem(int productId);
        Task<Cart> GetUserCart();
        Task<Cart> GetCart(string userId);
        Task<int> GetCartItemCount(string userId = "");
        Task<bool> DoCheckout();
    }
}
