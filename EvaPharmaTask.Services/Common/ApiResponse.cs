using System.Net;

namespace EvaPharmaTask.Services.Common
{
    public class ApiResponse<T>
    {
        private ApiResponse() { }

        public ApiResponse(int statusCode, bool succeeded, T result, string errorMessage = null)
        {
            StatusCode = statusCode;
            Succeeded = succeeded;
            Result = result;
            ErrorMessage = errorMessage ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public bool Succeeded { get; set; }
        public T Result { get; set; }
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }

        public static ApiResponse<T> Success(T result)
        {
            return new ApiResponse<T>((int)HttpStatusCode.OK, true, result, null);
        }

        public static ApiResponse<T> Failure(HttpStatusCode httpStatusCode, string errorMessage = null)
        {
            return new ApiResponse<T>((int)httpStatusCode, false, default, errorMessage);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "You have made a bad request.",
                401 => "You are not authorized.",
                404 => "Resource not found.",
                500 => "Something went wrong.",
                _ => null
            };
        }
    }
}
