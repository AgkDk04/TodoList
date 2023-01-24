using FluentValidation;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TodoList.Models
{
    public class TodoItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Todo text must be atleast 3 chars")]
        [MaxLength(50, ErrorMessage = "Todo text must not exceed 50 chars!")]
        public string? Text { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool? IsDone { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? ExecuterUserId { get; set; }
    }

    // Model validator
    public class TodoItemValidator : AbstractValidator<TodoItem>
    {
        public TodoItemValidator() 
        {
            RuleFor(i => i.Text).NotEmpty().NotNull();
        }
    }
}
