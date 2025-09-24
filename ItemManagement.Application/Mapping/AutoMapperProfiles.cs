using AutoMapper;
using ItemManagement.Application.Common.DTO;
using ItemManagement.Domain.Entities;
 
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ItemManagement.Application.Mapping
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Item, ItemDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));

            CreateMap<ItemDto, Item>()
                .ForMember(dest => dest.Category, opt => opt.Ignore());  // Assuming Category is managed separately

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();

            CreateMap<Transaction, TransactionDto>();
            CreateMap<TransactionDto, Transaction>();
        }
    }
}
