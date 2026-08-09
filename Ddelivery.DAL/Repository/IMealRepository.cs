using Ddelivery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Repository
{
    public interface IMealRepository
    {
        Task<Meal> AddAsync(Meal request);
        Task<Meal?> FindByIdAsync(int id);
        Task<bool> DecreaseQuantitesAsync(List<(int mealId, int quantity)> meals);
        IQueryable<Meal> Query();
        Task<Meal?> UpdateAsync(Meal meal);
        Task IncreaseQuantitiesAsync(List<(int mealId, int quantity)> meals);

    }
}
