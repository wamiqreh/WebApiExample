using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
namespace SND.Models
{
    public class AppResponse
    {
        public string message { get; set; }
        public HttpStatusCode status_code { get; set; }
        public object data { get; set; }
        public string file_path { get; set; }
        public string error_msg { get; set; }


    }
}
