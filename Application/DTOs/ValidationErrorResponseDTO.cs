namespace Application.DTOs
{
    public class ValidationErrorResponseDTO
    {
        public int Status { get; set; } = 400;
        public string Title { get; set; } = "Validation Error";
        public List<ValidationErrorDTO> Errors { get; set; } = new List<ValidationErrorDTO>();
    }
}
