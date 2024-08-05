using MediatR;
using Microsoft.Extensions.Configuration.UserSecrets;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQuery(int id) : IRequest<RestaurantDto>
{
    public int Id { get; } = id;
}