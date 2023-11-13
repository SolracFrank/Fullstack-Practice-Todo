using Application.Features.Todo.Commands.Delete;
using Application.Features.Todo.Commands.DeleteRange;
using Application.Features.Todo.Commands.Post;
using Application.Features.Todo.Commands.Put;
using Application.Features.Todo.GET;
using Domain.Dtos.Todos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "TodoAuth")]
    [ApiController]
    public class TodoController : BaseApiController
    {
        [HttpPost("{iduser}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Todo added succesful", typeof(string))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid inputs", typeof(ValidationProblemDetails))]
        public async Task<IActionResult> AddTodo([FromRoute] int iduser, [FromBody] PostTodoCommand request, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new PostTodoCommand
            {
                IdUser = iduser,
                DateLimit = request?.DateLimit,
                Description = request?.Description,
            });

           
            return result.ToOk();
        }

        [HttpGet("{iduser}/getall")]
        //[ResponseCache(CacheProfileName = "DefaultCache")]
        [SwaggerResponse(StatusCodes.Status200OK, "All User TODO displayed", typeof(IEnumerable<TodoDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid inputs", typeof(ValidationProblemDetails))]
        public async Task<IActionResult> GetAllTodoByUserId([FromRoute] int iduser, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetAllTodoByUserIdQuery
            {
                idUser = iduser
            });

            

            return result.ToOk();
        }

        [HttpDelete("{iduser}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Todo removed succesful", typeof(string))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid inputs", typeof(ValidationProblemDetails))]
        public async Task<IActionResult> DeleteTodoByUserId([FromRoute] int iduser, [FromBody] DeleteTodoDto request, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new DeleteTodoByUserIdCommand
            {
               IdTodo = request.IdTodo,
               IdUser = iduser,
            });

            return result.ToOk();
        }

        [HttpDelete("{iduser}/completed")]
        [SwaggerResponse(StatusCodes.Status200OK, "Todos removed succesful", typeof(string))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid inputs", typeof(ValidationProblemDetails))]
        public async Task<IActionResult> DeleteTodoByUserIdAndCompleted([FromRoute] int iduser, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new DeleteTodosByStateAndUserIdCommand
            {
                IdUser = iduser
            });

            return result.ToOk();
        }

        [HttpPut("{iduser}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Todo updated succesfully", typeof(string))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid inputs", typeof(ValidationProblemDetails))]
        public async Task<IActionResult> UpdateTodoByUserId([FromRoute] int iduser, [FromBody] PutTodoByUserIdCommand request, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new PutTodoByUserIdCommand
            {
               IdUser = iduser,
               IdTodo = request.IdTodo,
               Estado = request.Estado,
           
            });

            return result.ToOk();
        }
    }
}
