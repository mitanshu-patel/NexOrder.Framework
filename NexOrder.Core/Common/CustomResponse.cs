using System.Net;

namespace NexOrder.Framework.Core.Common
{
    public class CustomResponse<T>
    {
        public HttpStatusCode ResponseCode { get; set; }
        public string ErrorMessage { get; set; }

        public Dictionary<string, List<string>>? Errors { get; set; }
        public T Data { get; set; }
    }
}
