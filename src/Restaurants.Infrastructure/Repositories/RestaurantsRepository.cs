using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using System.Linq.Expressions;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantsRepository(RestaurantsDbContext dbContext)
    : IRestaurantsRepository
{
    public async Task Delete(Restaurant entity)
    {
        dbContext.Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task<int> Create(Restaurant entity)
    {
        dbContext.Restaurants.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        var restaurants = await dbContext.Restaurants.ToListAsync();
        return restaurants;
    }

    public async Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber,
        string? sortBy, SortDirection sortDirection)
    {
        var searchPhraseLower = searchPhrase?.ToLower();

        var baseQuery = dbContext.Restaurants.Where(x => searchPhraseLower == null || (x.Name.ToLower().Contains(searchPhraseLower)
            || x.Description.ToLower().Contains(searchPhraseLower)));

        var totalCount = await baseQuery.CountAsync();

        if (sortBy != null)
        {
            var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
            {
                { nameof(Restaurant.Name), x => x.Name },
                { nameof(Restaurant.Description), x => x.Description },
                { nameof(Restaurant.Category), x => x.Category },
            };

            var selectedColumn = columnsSelector[sortBy];

            baseQuery = sortDirection == SortDirection.Ascending
               ? baseQuery.OrderBy(selectedColumn)
               : baseQuery.OrderByDescending(selectedColumn);
        }

        var restaurants = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (restaurants, totalCount);
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        var restaurant = await dbContext.Restaurants.
            Include(x => x.Dishes).
            FirstOrDefaultAsync(x => x.Id == id);
        return restaurant;
    }

    public Task SaveChanges()
        => dbContext.SaveChangesAsync();
}