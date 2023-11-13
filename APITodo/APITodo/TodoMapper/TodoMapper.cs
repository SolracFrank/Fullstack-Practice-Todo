using APITodo.Models;
using APITodo.Models.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace APITodo.TodoMapper
{
    public class TodoMapper : Profile
    {
        public TodoMapper()
        {
            //Users Mapper
            CreateMap<Todo, TodoDto>().ReverseMap();
            CreateMap<Todo, CreateTodoDto>().ReverseMap();

            //Todo Mapper
            CreateMap<IdentityUser, DataUserDto>().ReverseMap();
            CreateMap<IdentityUser, LoginUserDto>().ReverseMap();
            CreateMap<IdentityUser, RegisterUserDto>().ReverseMap();
            CreateMap<IdentityUser, ResponseUserLoginDto>().ReverseMap();
            CreateMap<IdentityUser, UserDto>().ReverseMap();
        }
    }
}
