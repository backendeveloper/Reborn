using AutoMapper;
using Reborn.Common.Dto;
using Reborn.Domain.Model;

namespace Reborn.Service
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {            
            CreateMap<Category, CategoryDto>();            
        }
    }
}
