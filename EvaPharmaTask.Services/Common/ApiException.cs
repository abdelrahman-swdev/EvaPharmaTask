namespace EvaPharmaTask.Services.Common
{
    public class ApiException<T> : ApiResponse<T>
    {
        public ApiException(int statusCode, string errorMessage = null, string details = null)
        : base(statusCode, false, default, errorMessage)
        {
            Details = details;
        }

        public string Details { get; set; }
    }
}
