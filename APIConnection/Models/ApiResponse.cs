namespace APIConnection.Models
{
    public class ApiResponse
    {
        public required int StatusCode { get; set; }
        public string StatusMessage { get; set; } = string.Empty;
    }
}
