using System.Net;

namespace SND.BusinessObjects
{
    public class AppResponse
    {
        public string data { get; set; }

        public string message { get; set; }
        public HttpStatusCode status_code { get; set; }
    }
}