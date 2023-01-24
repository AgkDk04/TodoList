using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoList.Models;
using TodoList.Services;

namespace TodoList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoTaskController : ControllerBase
    {
        private readonly ILogger<TodoTaskController> _logger;
        private readonly ITodoListService _todoListService;
        private readonly IValidator<TodoItem> _validator;
        private readonly IHttpContextAccessor _context;

        public TodoTaskController(ITodoListService todoListService, IValidator<TodoItem> validator, IHttpContextAccessor httpContextAccessor, ILogger<TodoTaskController> logger)
        {            
            _todoListService = todoListService;
            _validator = validator;
            _logger = logger;
            _context = httpContextAccessor;
        }

        [HttpGet]
        [Authorize]
        [Produces("application/json")]
        public async Task<List<TodoItem>> Get()
        {
            return await _todoListService.GetByExecuterIdAsync(GetUserId());
        }

        [HttpPost]
        [Authorize]
        [Produces("application/json")]
        public async Task<IActionResult> Create([FromBody] TodoItem item)
        {
            item.CreatedDate = DateTime.Today;
            item.ExecuterUserId = item.ExecuterUserId ?? GetUserId();
            await _todoListService.CreateAsync(item);

            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        [HttpPut]
        [Authorize]
        [Produces("application/json")]
        public async Task<IActionResult> Update([FromQuery] string id)
        {
            TodoItem existingTodoItem = await _todoListService.GetByIdAsync(id);

            if (existingTodoItem == null)
            {
                return NotFound();
            }

            existingTodoItem.IsDone = true;

            await _todoListService.UpdateAsync(existingTodoItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [Produces("application/json")]
        public async Task<IActionResult> Delete(string id)
        {
            TodoItem existingItem = await _todoListService.GetByIdAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            await _todoListService.RemoveAsync(id);

            return NoContent();
        }
    
        private string GetUserId()
        {
            var userId = _context.HttpContext?.User?.Claims?.First(i => i.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError($"Error extracting userId from the context!");
            }

            return userId;
        }
    }
}
