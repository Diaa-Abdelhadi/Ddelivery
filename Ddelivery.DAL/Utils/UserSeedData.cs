using Ddelivery.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Utils
{
    public class UserSeedData : ISeedData
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserSeedData(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task DataSeed()
        {
            if (!await _userManager.Users.AnyAsync())
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin1",
                    Email = "admin@ddelivery.com",
                    FullName = "Admin User",
                    EmailConfirmed = true,
                };
                var owner = new ApplicationUser
                {
                    UserName = "owner1",
                    Email = "owner@ddelivery.com",
                    FullName = "Restaurant Owner",
                    EmailConfirmed = true,
                };
                var driver = new ApplicationUser
                {
                    UserName = "driver1",
                    Email = "driver@ddelivery.com",
                    FullName = "Driver User",
                    EmailConfirmed = true,
                };
                var customer = new ApplicationUser
                {
                    UserName = "customer1",
                    Email = "customer@ddelivery.com",
                    FullName = "Customer User",
                    EmailConfirmed = true,
                };
                await _userManager.CreateAsync(admin, "P@ssw0rd123");
                await _userManager.CreateAsync(owner, "P@ssw0rd123");
                await _userManager.CreateAsync(driver, "P@ssw0rd123");
                await _userManager.CreateAsync(customer, "P@ssw0rd123");

                await _userManager.AddToRoleAsync(admin, "Admin");
                await _userManager.AddToRoleAsync(owner, "RestaurantOwner");
                await _userManager.AddToRoleAsync(driver, "Driver");
                await _userManager.AddToRoleAsync(customer, "Customer");

            }
        }
    }
}

