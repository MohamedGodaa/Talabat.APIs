
namespace Talabat.Errors
{
    public class ApiResponse
    {
        public int StatusCodes { get; set; }
        public string? Massage { get; set; }
        public ApiResponse(int statusCode , string? massage = null)
        {
            StatusCodes = statusCode;
            Massage = massage ?? GetDefaultMassageForStatusCode(statusCode);
        }


        private string? GetDefaultMassageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                404 => "Resource Not Found",
                500 => " Server Error",
                _ => null,
            };
        }
    }
}
