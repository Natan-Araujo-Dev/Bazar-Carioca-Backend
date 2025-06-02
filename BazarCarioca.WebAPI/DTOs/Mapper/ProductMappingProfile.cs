using AutoMapper;
using BazarCarioca.WebAPI.DTOs;
using BazarCarioca.WebAPI.Models;

namespace BazarCarioca.WebAPI.DTOs.Mapper
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ReverseMap();

            CreateMap<ProductCreateDTO, Product>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ProductCreateDTO, ProductDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ProductUpdateDTO, Product>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
