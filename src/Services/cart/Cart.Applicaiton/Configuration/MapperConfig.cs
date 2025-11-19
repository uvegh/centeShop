using AutoMapper;
using Cart.Domain.Entities;
using Catalog.API.DTOs;
using Shared.Library.Events.Integrations;



namespace Catalog.API.Configuration;

public class MapperConfig:Profile
{

    public MapperConfig()
    {
        CreateMap<CartDto, CartEntity>().ReverseMap();
        CreateMap<CartItemDto, CartItem>().ReverseMap();

    }
}
