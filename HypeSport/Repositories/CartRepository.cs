﻿using HypeSport.Data;
using HypeSport.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HypeSport.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> AddItem(int productId, int quantity)
        {
            string userId = GetUserId();
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged in");
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new Cart
                    {
                        UserId = userId
                    };
                    _db.Carts.Add(cart);
                }
                _db.SaveChanges();
                // cart detail section
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(a => a.CartId == cart.Id && a.ProductId == productId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += quantity;
                }
                else
                {
                    var book = _db.Products.Find(productId);
                    cartItem = new CartDetail
                    {
                        ProductId = productId,
                        CartId = cart.Id,
                        Quantity = quantity,
                    };
                    _db.CartDetails.Add(cartItem);
                }
                _db.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception("Error");
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<int> RemoveItem(int productId)
        {
            //using var transaction = _db.Database.BeginTransaction();
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                // cart detail section
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(a => a.CartId == cart.Id && a.ProductId == productId);
                if (cartItem is null)
                    throw new Exception("No products in cart");
                else if (cartItem.Quantity == 1)
                    _db.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error");
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<Cart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid userid");
            var cart = await _db.Carts
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Product)
                                  .ThenInclude(a => a.Category)
                                  .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return cart;
        }

        public async Task<Cart> GetCart(string userId)
        {
            var cart = await _db.Carts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

        public async Task<int> GetCartItemCount(string userId="")
        {
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _db.Carts
                              join cartDetail in _db.CartDetails
                              on cart.Id equals cartDetail.CartId
                              select new { cartDetail.Id }
                        ).ToListAsync();
            return data.Count;
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }

        //public async Task<int> GetCartItemCount(string userId)
        //{
        //    return 0;
        //}
    }
}
