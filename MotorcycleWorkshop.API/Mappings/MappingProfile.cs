using AutoMapper;
using MotorcycleWorkshop.API.DTOs;
using MotorcycleWorkshop.model;

namespace MotorcycleWorkshop.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Repair, RepairDto>()
            .ForMember(d => d.MechanicName, opt => opt.MapFrom(s => s.Mechanic.Name))
            .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.Customer.Name))
            .ForMember(d => d.TotalPartsPrice, opt => 
                opt.MapFrom(s => s.UsedParts.Sum(p => p.Price)));

        CreateMap<Repair, RepairDetailDto>();
        CreateMap<Mechanic, MechanicDto>();
        CreateMap<Customer, CustomerDto>();
        CreateMap<Part, PartDto>();
        CreateMap<Address, AddressDto>();
    }
}
