using AutoMapper;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Mappings;

public class MappingProfile : Profile 
{
    public MappingProfile()
    {
        CreateMap<DeviceModify, Device>()
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Address, opt => opt.Ignore())
            .ForMember(dest => dest.Brand, opt => opt.Ignore())
            .ForMember(dest => dest.Attributes,
                opt => opt.MapFrom(src => src.Attributes.Select(a => new ItemAttribute
                {
                    Name = a.Key,
                    Value = a.Value
                }).ToList()));
    }
}