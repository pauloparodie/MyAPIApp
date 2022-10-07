using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Cors;


namespace OwinSelfHostMain
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class WebSitesController
    {

        public WebSitesController()
        {
            int a = 0;

        }
        public string Get()
        {
            return "Hello World!";
        }

    }
}
