using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class ValidationErrorDTO
    {
        [JsonPropertyName("field")]
        public string PropertyName { get; set; }

        [JsonPropertyName("message")]
        public string ErrorMessage { get; set; }
    }
}
