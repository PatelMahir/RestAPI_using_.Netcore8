using RestAPI_using_.Netcore8.Models;
using RestAPI_using_.Netcore8.Models.DTOs.UserDto;
using RestAPI_using_.Netcore8.Models.DTOs.CategoryDto;
using AutoMapper;
namespace RestAPI_using_.Netcore8.AutoMapper
{
    public class ApplicationMapper:Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category,CreateCategoryDto>().ReverseMap();
            CreateMap<Category,UserDto>().ReverseMap();
        }
    }
}