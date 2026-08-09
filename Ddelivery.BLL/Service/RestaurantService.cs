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
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IFileService _fileService;

        public RestaurantService(IRestaurantRepository restaurantRepository, IFileService fileService)
        {
            _restaurantRepository = restaurantRepository;
            _fileService = fileService;
        }

        public async Task<RestaurantResponse> CreateRestaurant(RestaurantRequest request, string ownerId)
        {
            var restaurant = request.Adapt<Restaurant>();
            restaurant.OwnerId = ownerId;

            if (request.MainImage != null)
            {
                var imagePath = await _fileService.UploadFileAsync(request.MainImage);
                restaurant.MainImage = imagePath;
            }

            await _restaurantRepository.CreateAsync(restaurant);
            return restaurant.Adapt<RestaurantResponse>();
        }

        public async Task<List<RestaurantResponse>> GetAllRestaurantsForAdmin()
        {
            var restaurants = await _restaurantRepository.Query().ToListAsync();
            return restaurants.Adapt<List<RestaurantResponse>>();
        }

        public async Task<List<RestaurantResponse>> GetMyRestaurants(string ownerId)
        {
            var restaurants = await _restaurantRepository.Query().Where(r => r.OwnerId == ownerId).ToListAsync();
            return restaurants.Adapt<List<RestaurantResponse>>();
        }

        public async Task<PagintedResponse<RestaurantUserResponse>> GetAllRestaurantsForUserAsync(
            string lang = "en", int page = 1, int limit = 10, string? search = null, string? sortBy = null, bool asc = true)
        {
            var query = _restaurantRepository.Query().Where(r => r.status == Status.Active);

            if (search is not null)
            {
                query = query.Where(r => r.Translations.Any(t => t.Name.Contains(search)));
            }

            var totalCount = await query.CountAsync();

            if (sortBy is not null)
            {
                if (sortBy == "rate")
                {
                    query = asc ? query.OrderBy(r => r.Rate) : query.OrderByDescending(r => r.Rate);
                }
                if (sortBy == "name")
                {
                    query = asc
                        ? query.OrderBy(r => r.Translations.FirstOrDefault(t => t.Language == lang).Name)
                        : query.OrderByDescending(r => r.Translations.FirstOrDefault(t => t.Language == lang).Name);
                }
            }

            var restaurants = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();
            var response = restaurants.BuildAdapter().AddParameters("lang", lang).AdaptToType<List<RestaurantUserResponse>>();

            return new PagintedResponse<RestaurantUserResponse>
            {
                TotalCount = totalCount,
                Page = page,
                Limit = limit,
                Data = response
            };
        }

        public async Task<RestaurantUserDetails> GetRestaurantDetailsForUser(int id, string lang = "en")
        {
            var restaurant = await _restaurantRepository.FindByIdAsync(id);
            return restaurant.BuildAdapter().AddParameters("lang", lang).AdaptToType<RestaurantUserDetails>();
        }

        public async Task<BaseResponse> UpdateRestaurantAsync(int id, RestaurantRequest request, string ownerId)
        {
            try
            {
                var restaurant = await _restaurantRepository.FindByIdAsync(id);
                if (restaurant is null)
                {
                    return new BaseResponse { Success = false, Message = "Restaurant not found" };
                }
                if (restaurant.OwnerId != ownerId)
                {
                    return new BaseResponse { Success = false, Message = "You are not authorized to update this restaurant" };
                }

                restaurant.Address = request.Address;
                restaurant.Latitude = request.Latitude;
                restaurant.Longitude = request.Longitude;
                restaurant.PhoneNumber = request.PhoneNumber;

                if (request.MainImage != null)
                {
                    var imagePath = await _fileService.UploadFileAsync(request.MainImage);
                    restaurant.MainImage = imagePath;
                }

                if (request.Translations != null)
                {
                    foreach (var translation in request.Translations)
                    {
                        var existingTranslation = restaurant.Translations.FirstOrDefault(t => t.Language == translation.Language);
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

                await _restaurantRepository.UpdateAsync(restaurant);
                return new BaseResponse { Success = true, Message = "Restaurant updated successfully" };
            }
            catch (Exception ex)
            {
                return new BaseResponse { Success = false, Message = "An error occurred while updating the restaurant", Errors = new List<string> { ex.Message } };
            }
        }

        public async Task<BaseResponse> ToggleStatus(int id)
        {
            try
            {
                var restaurant = await _restaurantRepository.FindByIdAsync(id);
                if (restaurant is null)
                {
                    return new BaseResponse { Success = false, Message = "Restaurant not found" };
                }
                restaurant.status = restaurant.status == Status.Active ? Status.InActive : Status.Active;
                await _restaurantRepository.UpdateAsync(restaurant);
                return new BaseResponse { Success = true, Message = "Restaurant status toggled successfully" };
            }
            catch (Exception ex)
            {
                return new BaseResponse { Success = false, Message = "An error occurred while toggling the restaurant status", Errors = new List<string> { ex.Message } };
            }
        }
    }
   }
