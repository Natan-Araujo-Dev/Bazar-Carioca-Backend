using AutoMapper;
using BazarCarioca.WebAPI.DTOs.Entities;
using BazarCarioca.WebAPI.Models;

namespace BazarCarioca.WebAPI.DTOs.Mapper
{
    public class ShopkeeperMappingProfile : Profile
    {
        public ShopkeeperMappingProfile()
        {
            CreateMap<Shopkeeper, ShopkeeperDTO>()
                .ReverseMap();
        }
    }
}
