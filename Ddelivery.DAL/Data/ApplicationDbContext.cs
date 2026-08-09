using Ddelivery.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ddelivery.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantTranslation> RestaurantTranslations { get; set; }
        public DbSet<MenuCategory> MenuCategories { get; set; }
        public DbSet<MenuCategoryTranslation> MenuCategoryTranslations { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealTranslation> MealTranslations { get; set; }
        public DbSet<MealImage> MealImages { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<RestaurantEarnings> RestaurantEarnings { get; set; }
        public DbSet<DriverEarnings> DriverEarnings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            builder.Entity<Restaurant>()
                .HasOne(r => r.Owner)
                .WithMany()
                .HasForeignKey(r => r.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Restaurant>()
                .HasOne(r => r.user)
                .WithMany()
                .HasForeignKey(r => r.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MenuCategory>()
                .HasOne(c => c.user)
                .WithMany()
                .HasForeignKey(c => c.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Meal>()
               .HasOne(m => m.user)
               .WithMany()
               .HasForeignKey(m => m.CreatedBy)
               .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
               .HasOne(o => o.User)
               .WithMany()
               .HasForeignKey(o => o.UserId)
               .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Order>()
               .HasOne(o => o.Driver)
               .WithMany()
               .HasForeignKey(o => o.DriverId)
               .OnDelete(DeleteBehavior.NoAction);

                 builder.Entity<Meal>()
                .HasOne(m => m.Restaurant)
                .WithMany(r => r.Meals)
                     .HasForeignKey(m => m.RestaurantId)
                         .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Meal>()
               .HasOne(m => m.MenuCategory)
               .WithMany(c => c.Meals)
               .HasForeignKey(m => m.MenuCategoryId)
               .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<RestaurantEarnings>()
                .HasIndex(e => new { e.RestaurantId, e.Date })
                .IsUnique();
            builder.Entity<RestaurantEarnings>()
                .HasOne(e => e.Restaurant)
                .WithMany()
                .HasForeignKey(e => e.RestaurantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<DriverEarnings>()
                .HasIndex(e => new { e.DriverId, e.Date })
                .IsUnique();
            builder.Entity<DriverEarnings>()
                .HasOne(e => e.Driver)
                .WithMany()
                .HasForeignKey(e => e.DriverId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Review>()
                .HasOne(r => r.Meal)
                .WithMany(m => m.Reviews)
                .HasForeignKey(r => r.MealId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseModel>();

            if (_httpContextAccessor.HttpContext != null)
            {
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                foreach (var entityEntry in entries)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        entityEntry.Property(x => x.CreatedBy).CurrentValue = currentUserId;
                        entityEntry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;

                    }
                    else if (entityEntry.State == EntityState.Modified)
                    {
                        entityEntry.Property(x => x.UpdatedBy).CurrentValue = currentUserId;
                        entityEntry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;

                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<BaseModel>();
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property(x => x.CreatedBy).CurrentValue = currentUserId;
                    entityEntry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;

                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property(x => x.UpdatedBy).CurrentValue = currentUserId;
                    entityEntry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;

                }
            }
            return base.SaveChanges();
        }
    }
}