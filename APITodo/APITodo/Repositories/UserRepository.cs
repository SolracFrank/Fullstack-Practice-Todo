using APITodo.Data;
using APITodo.Models.Dtos;
using APITodo.Repositories.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APITodo.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private string secretKey;

        public UserRepository(ApplicationDBContext db, IConfiguration config, UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            secretKey = config.GetValue<string>("ApiSettings:Secret");
        }

        public IdentityUser GetById(string id)
        {
            return _db.Users.FirstOrDefault(u => u.Id == id);
        }

        public ICollection<IdentityUser> GetUsers()
        {
            return _db.Users.OrderBy(u => u.Id).ToList() ?? new List<IdentityUser> { };
        }

        public bool isUniqueUser(string name)
        {
            var exist = _db.Users.Any(u => u.UserName == name || u.Email == name);
            return exist;
        }

        public async Task<ResponseUserLoginDto> Login(LoginUserDto loginUserDto)
        {
            var user = _db.Users.FirstOrDefault(
                u => u.UserName.ToLower() == loginUserDto.UserName.ToLower());
            bool isValide = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);
            if (user == null || !isValide)
            {
                return new ResponseUserLoginDto()
                {
                    Token = "",
                    User = null
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())  // Añade esta línea

                }),
                Expires = DateTime.UtcNow.AddDays(15),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescription);

            ResponseUserLoginDto responseUserLoginDto = new ResponseUserLoginDto()
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<DataUserDto>(user),
            };

           return responseUserLoginDto;
        }

        public async Task<DataUserDto> Register(RegisterUserDto registerUserDto)
        {
            IdentityUser identityUser = new IdentityUser()
            {
                UserName = registerUserDto.UserName,
                Email =  registerUserDto.Email,
            };
            var result = await _userManager.CreateAsync(identityUser, registerUserDto.Password);
            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("usuario"));
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                }
                await _userManager.AddToRoleAsync(identityUser, "admin");
                var returnUser = _db.Users.FirstOrDefault(u =>
                u.UserName == registerUserDto.UserName);

                return _mapper.Map<DataUserDto>(returnUser);
            }
            return new DataUserDto();
        }

        public bool Saved()
        {
            throw new NotImplementedException();
        }
    }
}






