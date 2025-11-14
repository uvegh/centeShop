using AutoMapper;
using Catalog.API.DTOs;


namespace Catalog.API.Configuration;

public class MapperConfig:Profile
{

    public MapperConfig()
    {
        CreateMap<ProductDto, Product>().ReverseMap();
        CreateMap<CreateProductDto, Product>().ReverseMap();

    }
}
