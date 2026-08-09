using Ddelivery.DAL.DTO.Request;
using Ddelivery.DAL.DTO.Response;
using Ddelivery.DAL.Models;
using Ddelivery.DAL.Repository;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.BLL.Service
{
    public class MealService : IMealService
    {
        private readonly IMealRepository _mealRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMenuCategoryRepository _menuCategoryRepository;
        private readonly IFileService _fileService;

        public MealService(
            IMealRepository mealRepository,
            IRestaurantRepository restaurantRepository,
            IMenuCategoryRepository menuCategoryRepository,
            IFileService fileService)
        {
            _mealRepository = mealRepository;
            _restaurantRepository = restaurantRepository;
            _menuCategoryRepository = menuCategoryRepository;
            _fileService = fileService;
        }

        public async Task<MealResponse?> CreateMeal(MealRequest request, int restaurantId, string ownerId)
        {
            var restaurant = await _restaurantRepository.FindByIdAsync(restaurantId);
            if (restaurant is null || restaurant.OwnerId != ownerId)
            {
                return null;
            }

            var menuCategory = await _menuCategoryRepository.FindByIdAsync(request.MenuCategoryId);
            if (menuCategory is null || menuCategory.RestaurantId != restaurantId)
            {
                return null;
            }

            var meal = request.Adapt<Meal>();
            meal.RestaurantId = restaurantId;

            if (request.MainImage != null)
            {
                var imagePath = await _fileService.UploadFileAsync(request.MainImage);
                meal.MainImage = imagePath;
            }

            if (request.SubImages != null)
            {
                meal.SubImages = new List<MealImage>();
                foreach (var file in request.SubImages)
                {
                    var imagePath = await _fileService.UploadFileAsync(file);
                    meal.SubImages.Add(new MealImage { ImageName = imagePath });
                }
            }

            await _mealRepository.AddAsync(meal);
            return meal.Adapt<MealResponse>();
        }

        public async Task<PagintedResponse<MealResponse>?> GetMealsForOwner(int restaurantId, string ownerId, int page = 1, int limit = 10)
        {
            var restaurant = await _restaurantRepository.FindByIdAsync(restaurantId);
            if (restaurant is null || restaurant.OwnerId != ownerId)
            {
                return null;
            }

            var query = _mealRepository.Query().Where(m => m.RestaurantId == restaurantId);
            var totalCount = await query.CountAsync();
            var meals = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();

            return new PagintedResponse<MealResponse>
            {
                TotalCount = totalCount,
                Page = page,
                Limit = limit,
                Data = meals.Adapt<List<MealResponse>>()
            };
        }

        public async Task<PagintedResponse<MealUserResponse>> GetAllMealsForUserAsync(
            int restaurantId, string lang = "en", int page = 1, int limit = 10, string? search = null,
            int? menuCategoryId = null, string? sortBy = null, bool asc = true)
        {
            var query = _mealRepository.Query().Where(m => m.RestaurantId == restaurantId && m.status == Status.Active);

            if (menuCategoryId is not null)
            {
                query = query.Where(m => m.MenuCategoryId == menuCategoryId);
            }

            if (search is not null)
            {
                query = query.Where(m => m.Translations.Any(t => t.Name.Contains(search) || t.Description.Contains(search)));
            }

            var totalCount = await query.CountAsync();

            if (sortBy is not null)
            {
                if (sortBy == "price")
                {
                    query = asc ? query.OrderBy(m => m.Price) : query.OrderByDescending(m => m.Price);
                }
                if (sortBy == "name")
                {
                    query = asc
                        ? query.OrderBy(m => m.Translations.FirstOrDefault(t => t.Language == lang).Name)
                        : query.OrderByDescending(m => m.Translations.FirstOrDefault(t => t.Language == lang).Name);
                }
                if (sortBy == "rate")
                {
                    query = asc ? query.OrderBy(m => m.Rate) : query.OrderByDescending(m => m.Rate);
                }
            }

            var meals = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();
            var response = meals.BuildAdapter().AddParameters("lang", lang).AdaptToType<List<MealUserResponse>>();

            return new PagintedResponse<MealUserResponse>
            {
                TotalCount = totalCount,
                Page = page,
                Limit = limit,
                Data = response
            };
        }

        public async Task<MealUserDetails> GetMealDetailsForUser(int id, string lang = "en")
        {
            var meal = await _mealRepository.FindByIdAsync(id);
            return meal.BuildAdapter().AddParameters("lang", lang).AdaptToType<MealUserDetails>();
        }

        public async Task<BaseResponse> UpdateMealAsync(int id, UpdateMealRequest request, string ownerId)
        {
            try
            {
                var meal = await _mealRepository.FindByIdAsync(id);
                if (meal is null)
                {
                    return new BaseResponse { Success = false, Message = "Meal not found" };
                }

                var restaurant = await _restaurantRepository.FindByIdAsync(meal.RestaurantId);
                if (restaurant is null || restaurant.OwnerId != ownerId)
                {
                    return new BaseResponse { Success = false, Message = "You are not authorized to update this meal" };
                }

                if (request.MenuCategoryId != meal.MenuCategoryId)
                {
                    var menuCategory = await _menuCategoryRepository.FindByIdAsync(request.MenuCategoryId);
                    if (menuCategory is null || menuCategory.RestaurantId != meal.RestaurantId)
                    {
                        return new BaseResponse { Success = false, Message = "Invalid menu category for this restaurant" };
                    }
                    meal.MenuCategoryId = request.MenuCategoryId;
                }

                meal.Price = request.Price;
                meal.Discount = request.Discount;
                meal.Quantity = request.Quantity;

                if (request.MainImage != null)
                {
                    var imagePath = await _fileService.UploadFileAsync(request.MainImage);
                    meal.MainImage = imagePath;
                }

                if (request.SubImages != null)
                {
                    meal.SubImages.Clear();
                    foreach (var file in request.SubImages)
                    {
                        var imagePath = await _fileService.UploadFileAsync(file);
                        meal.SubImages.Add(new MealImage { ImageName = imagePath });
                    }
                }

                if (request.Translations != null)
                {
                    foreach (var translation in request.Translations)
                    {
                        var existingTranslation = meal.Translations.FirstOrDefault(t => t.Language == translation.Language);
                        if (existingTranslation != null)
                        {
                            existingTranslation.Name = translation.Name;
                            existingTranslation.Description = translation.Description;
                        }
                        else
                        {
                            return new BaseResponse { Success = false, Message = $"Translation for language '{translation.Language}' not supported." };
                        }
                    }
                }

                await _mealRepository.UpdateAsync(meal);
                return new BaseResponse { Success = true, Message = "Meal updated successfully" };
            }
            catch (Exception ex)
            {
                return new BaseResponse { Success = false, Message = "An error occurred while updating the meal", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<BaseResponse> ToggleStatus(int id, string ownerId)
        {
            try
            {
                var meal = await _mealRepository.FindByIdAsync(id);
                if (meal is null)
                {
                    return new BaseResponse { Success = false, Message = "Meal not found" };
                }

                var restaurant = await _restaurantRepository.FindByIdAsync(meal.RestaurantId);
                if (restaurant is null || restaurant.OwnerId != ownerId)
                {
                    return new BaseResponse { Success = false, Message = "You are not authorized to update this meal" };
                }

                meal.status = meal.status == Status.Active ? Status.InActive : Status.Active;
                await _mealRepository.UpdateAsync(meal);
                return new BaseResponse { Success = true, Message = "Meal status toggled successfully" };
            }
            catch (Exception ex)
            {
                return new BaseResponse { Success = false, Message = "An error occurred while toggling the meal status", Errors = new List<string> { ex.Message } };
            }
        }
    }
}