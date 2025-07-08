﻿using AutoMapper;
using BazarCarioca.WebAPI.DTOs.Entities;
using BazarCarioca.WebAPI.Models;

namespace BazarCarioca.WebAPI.DTOs.Mapper
{
    public class ProductTypeMappingProfile : Profile
    {
        public ProductTypeMappingProfile() 
        {
            CreateMap<ProductType, ProductTypeDTO>()
                .ReverseMap();
        }
    }
}
