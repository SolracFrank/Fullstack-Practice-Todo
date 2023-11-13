using APITodo.Models.Dtos;
using Microsoft.AspNetCore.Identity;

namespace APITodo.Repositories.IRepositories
{
    public interface IUserRepository
    {
        ICollection<IdentityUser> GetUsers();
        IdentityUser GetById(string id);
        bool isUniqueUser(string name);
        Task<ResponseUserLoginDto> Login(LoginUserDto loginUserDto);
        Task<DataUserDto> Register(RegisterUserDto registerUserDto);

        bool Saved();
        
    }
}
