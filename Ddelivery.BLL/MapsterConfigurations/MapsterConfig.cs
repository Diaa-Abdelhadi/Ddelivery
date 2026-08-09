using Ddelivery.DAL.DTO.Response;
using Ddelivery.DAL.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.BLL.MapsterConfigurations
{
    public static class MapsterConfig
    {
        public static void MapsterConfRegister(string imageBaseUrl)
        {
            TypeAdapterConfig<Restaurant, RestaurantResponse>.NewConfig()
                .Map(dest => dest.MainImage, source => $"{imageBaseUrl}/images/{source.MainImage}");

            TypeAdapterConfig<Restaurant, RestaurantUserResponse>.NewConfig()
                .Map(dest => dest.Name, source => source.Translations
                    .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                    .Select(t => t.Name).FirstOrDefault())
                .Map(dest => dest.MainImage, source => $"{imageBaseUrl}/images/{source.MainImage}");

            TypeAdapterConfig<Restaurant, RestaurantUserDetails>.NewConfig()
                .Map(dest => dest.Name, source => source.Translations
                    .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                    .Select(t => t.Name).FirstOrDefault())
                .Map(dest => dest.Description, source => source.Translations
                    .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                    .Select(t => t.Description).FirstOrDefault())
                .Map(dest => dest.MainImage, source => $"{imageBaseUrl}/images/{source.MainImage}");

            TypeAdapterConfig<MenuCategory, MenuCategoryUserResponse>.NewConfig()
                .Map(dest => dest.Name, source => source.Translations
                 .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                        .Select(t => t.Name).FirstOrDefault());

            TypeAdapterConfig<Meal, MealResponse>.NewConfig()
                .Map(dest => dest.MainImage, source => $"{imageBaseUrl}/images/{source.MainImage}")
                .Map(dest => dest.SubImages, source => source.SubImages != null
                    ? source.SubImages.Select(img => $"{imageBaseUrl}/images/{img.ImageName}").ToList()
                    : new List<string>());

            TypeAdapterConfig<Meal, MealUserResponse>.NewConfig()
                .Map(dest => dest.Name, source => source.Translations
                    .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                    .Select(t => t.Name).FirstOrDefault())
                .Map(dest => dest.MainImage, source => $"{imageBaseUrl}/images/{source.MainImage}");

            TypeAdapterConfig<Meal, MealUserDetails>.NewConfig()
                .Map(dest => dest.Name, source => source.Translations
                    .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                    .Select(t => t.Name).FirstOrDefault())
                .Map(dest => dest.Description, source => source.Translations
                    .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                    .Select(t => t.Description).FirstOrDefault())
                .Map(dest => dest.MainImage, source => $"{imageBaseUrl}/images/{source.MainImage}")
                .Map(dest => dest.SubImages, source => source.SubImages != null
                    ? source.SubImages.Select(img => $"{imageBaseUrl}/images/{img.ImageName}").ToList()
                    : new List<string>());

            TypeAdapterConfig<OrderItem, OrderItemResponse>.NewConfig()
                .Map(dest => dest.MealName, source => source.Meal.Translations
                    .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                    .Select(t => t.Name).FirstOrDefault());

            TypeAdapterConfig<Order, OrderResponse>.NewConfig()
                .Map(dest => dest.Items, source => source.OrderItems);

            TypeAdapterConfig<Review, ReviewResponse>.NewConfig()
                .Map(dest => dest.UserName, source => source.User.UserName);

            TypeAdapterConfig<ApplicationUser, UserListResponse>.NewConfig()
                .Map(dest => dest.IsBlocked, source => source.LockoutEnd.HasValue && source.LockoutEnd.Value > DateTimeOffset.UtcNow);
        }
    }
}
