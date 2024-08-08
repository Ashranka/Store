using AutoMapper;
using Store.DTOs;
using Store.Entities;

namespace Store.Mappers
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles() 
        {
            CreateMap<CreateProductDto, Product>().ReverseMap();      
            CreateMap<UpdateproductDto, Product>().ReverseMap();
        }
    }
}
