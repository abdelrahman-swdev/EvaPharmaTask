using System.Collections.Generic;

namespace EvaPharmaTask.Services.Common
{
    public class ApiValidationErrorResponse<T> : ApiResponse<T>
    {
        public ApiValidationErrorResponse() : base(400, false, default)
        {
        }
        public IEnumerable<string> Errors { get; set; }
    }
}
