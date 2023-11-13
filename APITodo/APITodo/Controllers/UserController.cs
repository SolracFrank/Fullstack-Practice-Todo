using APITodo.Models;
using APITodo.Models.Dtos;
using APITodo.Repositories.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace APITodo.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _usRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        protected APIResponse _apiResponse;

        public UserController(IUserRepository usRepo, IMapper mapper, ILogger<UserController> logger)
        {
            _usRepo = usRepo;
            _mapper = mapper;
            _logger = logger;
            this._apiResponse = new APIResponse();
        }
        // ENDPOINT de Acción para obtene USUARIOS, 
        [Authorize(Roles = "admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUsers()
        {
            _logger.LogInformation("Called GetUsers.");  // Log al inicio del método

            var usersList = _usRepo.GetUsers();
            var usersListDto = new List<UserDto>();

            foreach (var user in usersList)
            {
                var userDto = _mapper.Map<UserDto>(user);
                usersListDto.Add(userDto);
            }

            return Ok(usersListDto);
        }
        //End Point para usuario por ID
        [Authorize(Roles = "admin")]
        [HttpGet("{userId}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUser(string userId)
        {
            _logger.LogInformation("Called GetUser by ID");
            var userItem = _usRepo.GetById(userId);
            if (userItem == null)
            {
                return NotFound();
            }

            var userDtoItem = _mapper.Map<UserDto>(userItem);

            return Ok(userDtoItem);
        }
        //End Point para crear user
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            _logger.LogInformation("Called Register.");
            var uniqueUserName = _usRepo.isUniqueUser(registerUserDto.UserName);
            if (uniqueUserName)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add("El usuario ya existe");
                return BadRequest(_apiResponse);
            }
            var usuario = await _usRepo.Register(registerUserDto);
            if (usuario == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add("Error en el registro");
                return BadRequest(_apiResponse);
            }

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
        //End Point para log
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            _logger.LogInformation("Called Login.");
            var loginResponse = await _usRepo.Login(loginUserDto);

            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add("El nombre de usuario o password son incorrectos");
                return BadRequest(_apiResponse);
            }
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            _apiResponse.Result = loginResponse;

            return Ok(_apiResponse);
        }

    }
}

