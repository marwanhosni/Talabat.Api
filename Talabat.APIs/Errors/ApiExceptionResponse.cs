namespace Talabat.APIs.Errors
{
    public class ApiExceptionResponse : ApiRequest
    {
        public string? Details { get; set; }
        public ApiExceptionResponse(int _statuesCode, string? _tittle = null, string? _details = null)
            : base(_statuesCode, _tittle)
        {
            Details = _details;
        }
    }
}
