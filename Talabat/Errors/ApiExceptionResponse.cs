namespace Talabat.Errors
{
    public class ApiExceptionResponse :ApiResponse
    {
        public string? Details { get; set; }
        public ApiExceptionResponse(int statusCode,string? massage = null,string? details = null)
            :base(statusCode, massage)
        {
            Details = details;
        }
    }
}
