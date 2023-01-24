using MongoDB.Driver;
using TodoList.Models;

namespace TodoList.Services
{
    public class TodoListService : ITodoListService
    {
        private readonly IMongoCollection<TodoItem> _todoCollection;
        
        public TodoListService(IMongoClient client, IDatabaseSettings settings)
        {
            var database = client.GetDatabase(settings.DatabaseName);

            _todoCollection = database.GetCollection<TodoItem>(settings.CollectionName);
        }

        public async Task CreateAsync(TodoItem item) =>
            await _todoCollection.InsertOneAsync(item);

        public async Task UpdateAsync(TodoItem item) =>
            await _todoCollection.ReplaceOneAsync(i => i.Id == item.Id, item);

        public async Task RemoveAsync(string id)
        {
            FilterDefinition<TodoItem> filter = Builders<TodoItem>.Filter.Eq("Id", id);
            await _todoCollection.DeleteOneAsync(filter);
        }
        
        public async Task<List<TodoItem>> GetByExecuterIdAsync(string executerId) =>
            await _todoCollection.Find(i => i.ExecuterUserId == executerId).ToListAsync();

        public async Task<TodoItem> GetByIdAsync(string id) =>
            await _todoCollection.Find(i => i.Id == id).FirstOrDefaultAsync();
        
    }
}