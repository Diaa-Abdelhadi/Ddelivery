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
    public class MenuCategoryService : IMenuCategoryService
    {
        private readonly IMenuCategoryRepository _menuCategoryRepository;
        private readonly IRestaurantRepository _restaurantRepository;

        public MenuCategoryService(IMenuCategoryRepository menuCategoryRepository, IRestaurantRepository restaurantRepository)
        {
            _menuCategoryRepository = menuCategoryRepository;
            _restaurantRepository = restaurantRepository;
        }

        public async Task<MenuCategoryResponse?> CreateMenuCategory(MenuCategoryRequest request, int restaurantId, string ownerId)
        {
            var restaurant = await _restaurantRepository.FindByIdAsync(restaurantId);
            if (restaurant is null || restaurant.OwnerId != ownerId)
            {
                return null;
            }

            var category = request.Adapt<MenuCategory>();
            category.RestaurantId = restaurantId;

            await _menuCategoryRepository.CreateAsync(category);
            return category.Adapt<MenuCategoryResponse>();
        }

        public async Task<List<MenuCategoryResponse>?> GetMenuCategoriesForOwner(int restaurantId, string ownerId)
        {
            var restaurant = await _restaurantRepository.FindByIdAsync(restaurantId);
            if (restaurant is null || restaurant.OwnerId != ownerId)
            {
                return null;
            }

            var categories = await _menuCategoryRepository.GetByRestaurantIdAsync(restaurantId);
            return categories.Adapt<List<MenuCategoryResponse>>();
        }

        public async Task<List<MenuCategoryUserResponse>> GetMenuCategoriesForUser(int restaurantId, string lang = "en")
        {
            var categories = await _menuCategoryRepository.GetByRestaurantIdAsync(restaurantId);
            var activeCategories = categories.Where(c => c.status == Status.Active).ToList();
            return activeCategories.BuildAdapter().AddParameters("lang", lang).AdaptToType<List<MenuCategoryUserResponse>>();
        }

        public async Task<BaseResponse> UpdateMenuCategoryAsync(int id, MenuCategoryRequest request, string ownerId)
        {
            try
            {
                var category = await _menuCategoryRepository.FindByIdAsync(id);
                if (category is null)
                {
                    return new BaseResponse { Success = false, Message = "Menu category not found" };
                }

                var restaurant = await _restaurantRepository.FindByIdAsync(category.RestaurantId);
                if (restaurant is null || restaurant.OwnerId != ownerId)
                {
                    return new BaseResponse { Success = false, Message = "You are not authorized to update this menu category" };
                }

                if (request.Translations != null)
                {
                    foreach (var translation in request.Translations)
                    {
                        var existingTranslation = category.Translations.FirstOrDefault(t => t.Language == translation.Language);
                        if (existingTranslation != null)
                        {
                            existingTranslation.Name = translation.Name;
                        }
                        else
                        {
                            return new BaseResponse { Success = false, Message = $"Translation for language '{translation.Language}' not supported." };
                        }
                    }
                }

                await _menuCategoryRepository.UpdateAsync(category);
                return new BaseResponse { Success = true, Message = "Menu category updated successfully" };
            }
            catch (Exception ex)
            {
                return new BaseResponse { Success = false, Message = "An error occurred while updating the menu category", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<BaseResponse> DeleteMenuCategoryAsync(int id, string ownerId)
        {
            try
            {
                var category = await _menuCategoryRepository.FindByIdAsync(id);
                if (category is null)
                {
                    return new BaseResponse { Success = false, Message = "Menu category not found" };
                }

                var restaurant = await _restaurantRepository.FindByIdAsync(category.RestaurantId);
                if (restaurant is null || restaurant.OwnerId != ownerId)
                {
                    return new BaseResponse { Success = false, Message = "You are not authorized to delete this menu category" };
                }

                await _menuCategoryRepository.DeleteAsync(category);
                return new BaseResponse { Success = true, Message = "Menu category deleted successfully" };
            }
            catch (Exception ex)
            {
                return new BaseResponse { Success = false, Message = "An error occurred while deleting the menu category", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<BaseResponse> ToggleStatus(int id, string ownerId)
        {
            try
            {
                var category = await _menuCategoryRepository.FindByIdAsync(id);
                if (category is null)
                {
                    return new BaseResponse { Success = false, Message = "Menu category not found" };
                }

                var restaurant = await _restaurantRepository.FindByIdAsync(category.RestaurantId);
                if (restaurant is null || restaurant.OwnerId != ownerId)
                {
                    return new BaseResponse { Success = false, Message = "You are not authorized to toggle this menu category's status" };
                }

                category.status = category.status == Status.Active ? Status.InActive : Status.Active;
                await _menuCategoryRepository.UpdateAsync(category);
                return new BaseResponse { Success = true, Message = "Menu category status toggled successfully" };
            }
            catch (Exception ex)
            {
                return new BaseResponse { Success = false, Message = "An error occurred while toggling the menu category status", Errors = new List<string> { ex.Message } };
            }
        }
    }
}
