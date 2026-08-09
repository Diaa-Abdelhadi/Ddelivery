using Ddelivery.DAL.DTO.Request;
using Ddelivery.DAL.DTO.Response;
using Ddelivery.DAL.Models;
using Ddelivery.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.BLL.Service
{
    public class CartService : ICartService
    {
        private readonly IMealRepository _mealRepository;
        private readonly ICartRepository _cartRepository;

        public CartService(IMealRepository mealRepository, ICartRepository cartRepository)
        {
            _mealRepository = mealRepository;
            _cartRepository = cartRepository;
        }

        public async Task<BaseResponse> AddToCartAsync(string userId, AddToCartRequest request)
        {
            var meal = await _mealRepository.FindByIdAsync(request.MealId);
            if (meal is null)
            {
                return new BaseResponse { Success = false, Message = "Meal not found" };
            }

            var cartItem = await _cartRepository.GetCartItemAsync(userId, request.MealId);
            var existingCount = cartItem?.count ?? 0;

            if (meal.Quantity < request.count + existingCount)
            {
                return new BaseResponse { Success = false, Message = "Not enough quantity in stock" };
            }

            if (cartItem is not null)
            {
                cartItem.count += request.count;
                await _cartRepository.UpdateAsync(cartItem);
            }
            else
            {
                var cart = request.Adapt<Cart>();
                cart.UserId = userId;
                await _cartRepository.Createasync(cart);
            }

            return new BaseResponse { Success = true, Message = "Meal added to cart" };
        }

        public async Task<CartSummaryResponse> GetUserCartAsync(string userId, string lang = "en")
        {
            var cart = await _cartRepository.GetUserCartAsync(userId);
            var items = cart.Select(c => new CartResponse
            {
                MealId = c.MealId,
                MealName = c.Meal.Translations.FirstOrDefault(t => t.Language == lang)?.Name,
                Count = c.count,
                Price = c.Meal.Price
            }).ToList();

            return new CartSummaryResponse { Items = items };
        }

        public async Task<BaseResponse> UpdateQuantityAsync(string userId, int mealId, int count)
        {
            var cartItem = await _cartRepository.GetCartItemAsync(userId, mealId);
            if (cartItem is null)
            {
                return new BaseResponse { Success = false, Message = "Item not found in cart" };
            }

            if (count == 0)
            {
                await _cartRepository.DeleteAsync(cartItem);
                return new BaseResponse { Success = true, Message = "Item removed from cart" };
            }

            if (count < 0)
            {
                return new BaseResponse { Success = false, Message = "Quantity cannot be negative" };
            }

            if (cartItem.Meal.Quantity < count)
            {
                return new BaseResponse { Success = false, Message = "Not enough quantity in stock" };
            }

            cartItem.count = count;
            await _cartRepository.UpdateAsync(cartItem);
            return new BaseResponse { Success = true, Message = "Quantity updated successfully" };
        }

        public async Task<BaseResponse> ClearCartAsync(string userId)
        {
            await _cartRepository.ClearCartAsync(userId);
            return new BaseResponse { Success = true, Message = "Cart cleared" };
        }

        public async Task<BaseResponse> RemoveFromCartAsync(string userId, int mealId)
        {
            var cartItem = await _cartRepository.GetCartItemAsync(userId, mealId);
            if (cartItem is null)
            {
                return new BaseResponse { Success = false, Message = "Item not found in cart" };
            }

            await _cartRepository.DeleteAsync(cartItem);
            return new BaseResponse { Success = true, Message = "Item removed from cart" };
        }
    }
}
