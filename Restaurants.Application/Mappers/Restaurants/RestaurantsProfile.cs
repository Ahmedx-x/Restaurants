using AutoMapper;
using Restaurants.Application.Dtos.Restaurants;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Mappers.Restaurants
{
    public class RestaurantsProfile : Profile
    {
        public RestaurantsProfile()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(dst => dst.City, opt =>
                    opt.MapFrom(src => src.Address == null ? null : src.Address.City))
                .ForMember(dst => dst.Street, opt =>
                    opt.MapFrom(src => src.Address == null ? null : src.Address.Street))
                .ForMember(dst => dst.PostalCode, opt =>
                    opt.MapFrom(src => src.Address == null ? null : src.Address.PostalCode));


            CreateMap<CreateRestaurantDto, Restaurant>()
                .ForMember(d => d.Address, opt => opt.MapFrom(
                    src => new Address
                    {
                        City = src.City,
                        Street = src.Street,
                        PostalCode = src.PostalCode
                    }
                    ));
        }
    }
}
