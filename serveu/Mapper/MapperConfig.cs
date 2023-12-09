using AutoMapper;
using serveu.Dtos;
using serveu.Models;

namespace serveu.Mapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<MenuCategoryEntities, MenuCategoryDTO>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MenuCategoryId))
             .ReverseMap();
            CreateMap<MenuCategoryEntities,
               UpdateMenuCategoryRequest>().ReverseMap();
            CreateMap<MenuCategoryEntities,
              PaginatedMenuCategoryResponseDTO>().ReverseMap();


            CreateMap<MenuItemEntities, MenuItemDTO>()
                            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MenuItemId))
                            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                            .ReverseMap();

            CreateMap<FileEntities, ImageDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<ApplicationUser, ApplicationUserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember(dest => dest.MenuItems, opt => opt.MapFrom(src => src.MenuItems))
                .ReverseMap();
        }
    }
}
