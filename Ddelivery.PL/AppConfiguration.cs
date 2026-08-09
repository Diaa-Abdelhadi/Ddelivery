using Ddelivery.BLL.Service;
using Ddelivery.DAL.Repository;
using Ddelivery.DAL.Utils;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Ddelivery.PL
{
    public class AppConfiguration
    {

        public static void Config(IServiceCollection Services)
        {
            Services.AddScoped<ISeedData, RoleSeedData>();
            Services.AddScoped<ISeedData, UserSeedData>();

            Services.AddScoped<IAuthenticationService, AuthenticationService>();
            Services.AddTransient<IEmailSender, EmailSender>();
            Services.AddScoped<ITokenService, TokenService>();

            Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            Services.AddScoped<IMenuCategoryRepository, MenuCategoryRepository>();
            Services.AddScoped<IMealRepository, MealRepository>();
            Services.AddScoped<ICartRepository, CartRepository>();
            Services.AddScoped<IOrderRepository, OrderRepository>();
            Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            Services.AddScoped<IFileService, FileService>();
            Services.AddScoped<IRestaurantService, RestaurantService>();
            Services.AddScoped<IMenuCategoryService, MenuCategoryService>();
            Services.AddScoped<IMealService, MealService>();
            Services.AddScoped<ICartService, CartService>();
            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<IRestaurantEarningsRepository, RestaurantEarningsRepository>();
            Services.AddScoped<IDriverEarningsRepository, DriverEarningsRepository>();
            Services.AddScoped<IEarningsService, EarningsService>();
            Services.AddScoped<IReviewRepository, ReviewRepository>();
            Services.AddScoped<IReviewService, ReviewService>();
            Services.AddScoped<IMangerUserService, ManageUserService>();

        }

    }
}
