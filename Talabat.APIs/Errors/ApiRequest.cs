namespace Talabat.APIs.Errors
{
    public class ApiRequest
    {
        public int StatusCode { get; set; }
        public string? Title { get; set; }

        public ApiRequest(int _statusCode, string? _title = null)
        {
            StatusCode = _statusCode;
            Title = _title ?? GetDefaultMessageForStatusCode(StatusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request ,you have made",
                401 => "Authorized , you are not",
                404 => "Resource are not Found",
                500 => "Error are the path to the dark side , Error lead to anger . Anger leads to hate. Hate leads to career change",
                _ => null
            };
        }
    }
}
