using AutoMapper;
using Net6SampleProject.Core.DTOs;
using Net6SampleProject.Core.Models;

namespace Net6SampleProject.Service.Mappings
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            // Product
            CreateMap<Product, ProductDto>().ReverseMap();

            // Category
            CreateMap<Category, CategoryDto>().ReverseMap();

            // Category with Products
            CreateMap<Category, CategoryWithProductsDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

            // Product with Category
            CreateMap<Product, ProductsWithCategoryDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

            // ProductFeature varsa entity'si
            CreateMap<ProductFeature, ProductFeatureDto>().ReverseMap();
        }
    }
}
