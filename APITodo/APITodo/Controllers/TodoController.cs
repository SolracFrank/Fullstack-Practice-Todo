using APITodo.Models;
using APITodo.Models.Dtos;
using APITodo.Repositories.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APITodo.Controllers
{
    [Route("api/todos")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _tdRepo;
        private readonly IMapper _mapper;

        public TodoController(ITodoRepository tdRepo, IMapper mapper)
        {
            _tdRepo = tdRepo;
            _mapper = mapper;
        }

        //Get Todos Endpoint
        [Authorize(Roles ="admin, usuario")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetTodos()
        {
            try
            {
                //Get Auth User 
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 

                if (string.IsNullOrEmpty(userId))
                {
                    return NotFound("Usuario no encontrado.");
                }

                var todoList = _tdRepo.GetTodos(userId) ?? new List<Todo>();

                var todoListDto = todoList.Select(_mapper.Map<TodoDto>).ToList();

                return Ok(todoListDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        //Set Todo Endpoint
        [Authorize(Roles = "admin, usuario")]
        [HttpPost]

        [ProducesResponseType(201, Type = typeof(TodoDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult CreateTodo([FromBody] CreateTodoDto createTodoDto)
        {
            //Get Auth User 
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Acceso no autorizado.");
            }
            if (!ModelState.IsValid || createTodoDto == null)
            {
                return BadRequest(ModelState);
            }

            var todo = _mapper.Map<Todo>(createTodoDto);
            if (!_tdRepo.CreateTodo(todo,userId))
            {
                ModelState.AddModelError("", $"Something went wrong creating TODO.");
                return StatusCode(500, ModelState);
            }
            return CreatedAtAction(nameof(GetTodos), null, _mapper.Map<TodoDto>(todo));
        }

        //Patch Todo EndPoint
        [Authorize(Roles = "admin, usuario")]
        [HttpPatch("{todoId:int}", Name = "UpdatePatchTodo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ActualizarPatchCategoria(int todoId, [FromBody] TodoDto todoDto)
        {
            if (!ModelState.IsValid || todoDto == null || todoId != todoDto.Id)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Acceso no autorizado.");
            }

            var todo = _mapper.Map<Todo>(todoDto);
            if (!_tdRepo.UpdateTodo(todo))
            {
                ModelState.AddModelError("", $"Something went wrong at Updating TODO");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        //Delete Todo Endpoint
        [Authorize(Roles = "admin, usuario")]
        [HttpDelete("{todoId:int}", Name = "DeleteTodo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteTodo(int todoId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Acceso no autorizado.");
            }
            var todo = _tdRepo.GetTodo(userId,todoId);
       
            if (todo == null)
            {
                return NotFound("TODO no encontrado.");
            }
            if (!_tdRepo.DeleteTodo(todo))
            {
                ModelState.AddModelError("", $"Algo salio mal, borrando el registro {todo.Description}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        //DeleteAll Todos (from state) ENDPOINT
        [Authorize(Roles = "admin, usuario")]
        [HttpDelete("completed", Name = "DeleteCompletedTodos")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteCompletedTodos()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Acceso no autorizado.");
            }

            if (!_tdRepo.DeleteCompletedTodo(true, userId))
            {
                ModelState.AddModelError("", "Algo salió mal al eliminar los TODOs completados");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
