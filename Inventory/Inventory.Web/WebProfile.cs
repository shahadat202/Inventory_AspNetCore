using AutoMapper;
using Inventory.Domain.Entities;
using Inventory.Web.Areas.Admin.Models;

namespace Inventory.Web
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<ProductInsertModel, Product>().ReverseMap();
            CreateMap<ProductUpdateModel, Product>().ReverseMap();
            CreateMap<Product, ViewProductModel>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));
        }
    }
}
