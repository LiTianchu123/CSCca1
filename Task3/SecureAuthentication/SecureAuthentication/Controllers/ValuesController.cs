using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace SecureAuthentication.Controllers
{
    [System.Web.Http.Authorize]
    public class ValuesController : ApiController
    {

        // GET api/values
        [RequireHttps]
        public string Get()
        {
            var userName = this.RequestContext.Principal.Identity.Name;
            return String.Format("Hello, {0}.", userName);
        }
    }
}
