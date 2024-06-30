using AutoMapper;
using GBLesson3GraphQL.DTO;
using GBLesson3GraphQL.Models;

namespace GBLesson3GraphQL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap(); // связываем объекты. ReverseMap - мапится и в обратную сторону
            CreateMap<ProductGroup, ProductGroupViewModel>().ReverseMap();

        }
    }
}
