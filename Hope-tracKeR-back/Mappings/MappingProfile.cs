using AutoMapper;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;
using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Mappings;

public class MappingProfile : Profile 
{
    public MappingProfile()
    {
        CreateMap<DeviceRequest, Device>()
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Address, opt => opt.Ignore())
            .ForMember(dest => dest.Brand, opt => opt.Ignore())
            .ForMember(dest => dest.Employee, opt => opt.Ignore())
            .ForMember(dest => dest.Attributes,
                opt => opt.MapFrom(src => src.Attributes.Select(a => new ItemAttribute{
                    Name = a.Key,
                    Value = a.Value
                }).ToList()));

        CreateMap<Device, DeviceResponse>()
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Name))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.Branch}, {src.Address.Building}, {src.Address.Floor}, {src.Address.Room}"))
            .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FullName : null))
            .ForMember(dest => dest.Attributes, opt => opt.MapFrom(src => src.Attributes.ToDictionary(a => a.Name, a => a.Value)));

        CreateMap<StartRepairRequest, Repair>()
            .ForMember(dest => dest.Address, opt => opt.Ignore())
            .ForMember(dest => dest.Item, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.Diagnosis, opt => opt.Ignore())
            .ForMember(dest => dest.AddressId, opt => opt.MapFrom(src => src.CurrentAddressId))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DescriptionFailure));
    }
}