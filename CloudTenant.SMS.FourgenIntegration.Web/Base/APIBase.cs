

namespace SND
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using System.Text;
    using System.Net.Http.Headers;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;

    public class APIBase : ApiController
    {
        protected HttpResponseMessage SendToApp(object param)
        {
            var resp = new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(param))
            };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }

        protected object GetRawResponse()
        {
            return Request.Content.ReadAsStringAsync();
        }

        protected T GetRawResponse<T>()
        {
            // Helper.Logger.PrintInfo(Request.Content.ReadAsStringAsync().Result);
            return JsonConvert.DeserializeObject<T>(Request.Content.ReadAsStringAsync().Result);
        }

        protected object GetHeaderResponse(params object[] param)
        {
            return Request.Headers.GetValues("value").First();
        }
    }
}
