using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;

namespace importer
{
    class Program
    {
        static void Main(string[] args)
        {
            string username = "example";
            string password = "password";

            string status = GetImportStatus(username, password);
            Console.WriteLine("Initial import status: " + status);

            Import import = new Import(false,false,null);
            import.AddTag("one");
            import.AddTag("two");
            import.AddExecution(new Execution("2013-02-07T09:53:34-05:00", "SPY", "100", "151.05", "", "1.00", "0.04", "0.21"));
            import.AddExecution(new Execution("2013-02-07T09:54:01-05:00", "SPY", "-100", "150.94", "", "1.00", "0.17", "0.21"));
            
            status = CreateImport(username, password, import);
            Console.WriteLine("Importing trades: " + status);

            status = GetImportStatus(username, password);
            Console.WriteLine("Import status: " + status);

            Console.WriteLine("Waiting 5 seconds...");
            System.Threading.Thread.Sleep(5000);

            status = GetImportStatus(username, password);
            Console.WriteLine("Import status: " + status);

            Console.WriteLine("Press return to exit...");
            Console.ReadLine();
        }

        static string GetImportStatus(string username, string password)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://www.tradervue.com/api/v1/");
            client.DefaultRequestHeaders.Authorization = CreateBasicHeader(username, password);

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync("imports").Result;
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;

                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> tokens = jss.Deserialize<Dictionary<string, object>>(content);
                return (string)tokens["status"];
            }
            else
            {
                return response.ReasonPhrase;
            }
        }

        static string CreateImport(string username, string password, Import import)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://www.tradervue.com/api/v1/");
            client.DefaultRequestHeaders.Authorization = CreateBasicHeader(username, password);

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            StringContent payload = new StringContent(import.ToJson(), new UTF8Encoding(), "application/json");
            HttpResponseMessage response = client.PostAsync("imports", payload).Result;
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, string> tokens = jss.Deserialize<Dictionary<string, string>>(content);
                return tokens["status"];
            }
            else
            {
                return "Error status code: " + response.StatusCode.ToString();
            }
        }

        static AuthenticationHeaderValue CreateBasicHeader(string username, string password)
        {
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(username + ":" + password);
            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

    }
}
