using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dtos;

namespace MagicVilla_VillaAPI
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa,VillaDto>().ReverseMap();
            CreateMap<Villa,VillaCreateDTO>().ReverseMap();
            CreateMap<Villa,VillaUpdateDTO>().ReverseMap();    
            
            CreateMap<VillaNumber,VillaNumberDTO>().ReverseMap();
            CreateMap<VillaNumberCreateDTO,VillaNumber>().ReverseMap();
            CreateMap<VillaNumberUpdateDTO,VillaNumber>().ReverseMap();
        }

    }
}
