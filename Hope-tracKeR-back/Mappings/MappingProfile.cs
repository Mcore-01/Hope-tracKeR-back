using AutoMapper;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Mappings;

public class MappingProfile : Profile 
{
    public MappingProfile()
    {
        CreateMap<DeviceModify, Device>();
    }
}