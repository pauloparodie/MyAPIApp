using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using System.Net.Http;


namespace OwinSelfHostMain
{

    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.EnableCors();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);
        }
    }


    public class Program
    {
        public static void Main()
        {
            string baseAddress = "http://127.0.0.1:84/";

            try
            {
                // Start OWIN host 
                WebApp.Start<Startup>(url: baseAddress);

            } catch(Exception ex)
            {
                int a = 0;
            }

            Console.WriteLine("Web API Server has started at http://localhost:84");
            Console.ReadLine();

            /*
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                HttpClient client = new HttpClient();

                var response = client.GetAsync(baseAddress + "api/values").Result;

                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                Console.ReadLine();
            }*/
        }
    }
}
