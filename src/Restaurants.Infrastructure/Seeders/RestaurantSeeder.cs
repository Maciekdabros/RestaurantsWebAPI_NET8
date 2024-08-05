using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Constants;

namespace Restaurants.Infrastructure.Seeders;

internal class RestaurantSeeder(RestaurantsDbContext dbContext) : IRestaurantSeeder
{
    public async Task Seed()
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Restaurants.Any())
            {
                var restaurants = GetRestaurants();
                dbContext.Restaurants.AddRange(restaurants);
                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.Roles.Any())
            {
                var roles = GetRoles();
                dbContext.Roles.AddRange(roles);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private IEnumerable<IdentityRole> GetRoles()
    {
        List<IdentityRole> roles = [
            new(UserRoles.User)
            {
                NormalizedName=UserRoles.User.ToUpper()
            },
            new(UserRoles.Owner)
            {
                NormalizedName = UserRoles.User.ToUpper()
            },
            new(UserRoles.Admin)
            {
                NormalizedName=UserRoles.User.ToUpper()
            },
        ];
        return roles;
    }

    private IEnumerable<Restaurant> GetRestaurants()

    {
        List<Restaurant> restaurants = [
            new()
            {
                Name="KFC",
                Category="Fast Food",
                Description="KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Louisville, Kentucky, that specializes in fried chicken.",
                ContactEmail="contact@kfc.com",
                HasDelivery=true,
                Dishes=
                [
                    new()
                    {
                        Name="Chicken Wings",
                        Description="Chicken wings, often simply referred to as wings, are chicken wing sections that have been deep fried, unbreaded and coated in vinegar-based hot sauce.",
                        Price=5.99m
                    },
                    new()
                    {
                        Name="Chicken Drumsticks",
                        Description="A chicken drumstick is the lower part of the chicken leg, also known as the calf.",
                        Price=4.99m
                    },
                ],
                Address=new()
                {
                    City="London",
                    Street="Cork St 5",
                    PostalCode="WC2N 5DU"
                }
            },
            new Restaurant()
            {
                Name="McDonald's",
                Category="Fast Food",
                Description="McDonald's Corporation is an American fast food company, founded in 1940 as a restaurant operated by Richard and Maurice McDonald, in San Bernardino, California, United States.",
                ContactEmail="contact@mcdonald.com",
                HasDelivery=true,
                Address=new()
                {
                    City="London",
                    Street="Boots 193",
                    PostalCode="W1F 8SR"
                }
            }
        ];
        return restaurants;
    }
}