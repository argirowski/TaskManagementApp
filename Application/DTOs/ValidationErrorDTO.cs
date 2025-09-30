using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class ValidationErrorDTO
    {
        [JsonPropertyName("field")]
        public required string PropertyName { get; set; }

        [JsonPropertyName("message")]
        public required string ErrorMessage { get; set; }
    }
}
