using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using ItemManagement.WebAPI.Models;
using ItemManagement.Application.Common.DTO;

namespace ItemManagement.WebAPI.Mappings
{
    public class ViewModelMappingProfile : Profile
    {
        public ViewModelMappingProfile()
        {
            CreateMap<ItemDto, ItemApiModel>()
                .ForMember(dest => dest.Categories, opt => opt.Ignore());  // Categories populated in controller
            CreateMap<ItemApiModel, ItemDto>();  // Allow reverse mapping

            CreateMap<CategoryDto, CategoryApiModel>();
            CreateMap<CategoryApiModel, CategoryDto>();

            CreateMap<TransactionDto, TransactionApiModel>();
            CreateMap<TransactionApiModel, TransactionDto>();
            CreateMap<SellOrderDto, SellOrderApiModel>();
            CreateMap<SellOrderApiModel, SellOrderDto>();






        }
    }
}
