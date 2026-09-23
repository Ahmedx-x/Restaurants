using AutoMapper;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dtos.Dishes;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Mappers.Dishes
{
    public class DishesProfle : Profile
    {
        public DishesProfle()
        {
            CreateMap<Dish, DishDto>();
            CreateMap<CreateDishCommand, Dish>();
        }
    }
}
