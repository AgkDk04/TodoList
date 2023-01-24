using MongoDB.Bson;
using TodoList.Models;

namespace TodoList.Services
{
    public interface ITodoListService
    {
        Task CreateAsync(TodoItem item);

        Task UpdateAsync(TodoItem item);

        Task RemoveAsync(string id);

        Task<List<TodoItem>> GetByExecuterIdAsync(string executerId);

        Task<TodoItem> GetByIdAsync(string id);
    }
}