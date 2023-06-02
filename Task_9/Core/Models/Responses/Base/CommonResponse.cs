using System.Net;

namespace Task_9.Core.Models.Responses.Base
{
    public class CommonResponse<T>
    {
        public HttpStatusCode Status;

        public string Content;

        public T Body;
    }
}
