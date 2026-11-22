namespace Talabat.APIs.Errors;

public class ApiValidationResponse : ApiRequest
{
    public IEnumerable<string> Errors { get; set; }
    public ApiValidationResponse(int _statusCode, IEnumerable<string> _errors, string? _title = null) : base(_statusCode, _title)
    {
        Errors = _errors;
    }
}
