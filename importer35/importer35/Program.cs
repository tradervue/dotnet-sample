using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.tradervue.com/api/v1/imports");
            req.Accept = "application/json";
            req.ContentType = "application/json";
            req.UserAgent = "DotNet Sample Application (https://github.com/tradervue/dotnet-sample)";
            req.Credentials = new NetworkCredential(username, password);
            try
            {
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream s = response.GetResponseStream();
                    StreamReader sr = new StreamReader(s);
                    string content = sr.ReadToEnd();
                    sr.Close();
                    s.Close();
                    response.Close();

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    Dictionary<string, object> tokens = jss.Deserialize<Dictionary<string, object>>(content);
                    return (string)tokens["status"];
                }
                else
                {
                    response.Close();
                    return "Error, status = " + response.StatusCode.ToString();
                }
            }
            catch (WebException e)
            {
                WebExceptionStatus exStatus = e.Status;
                if (exStatus == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse resp = (HttpWebResponse)e.Response;
                    return "Error, status = " + resp.StatusCode.ToString();
                }
                else
                {
                    return "Error occurred: " + e.ToString();
                }
            }
        }

        static string CreateImport(string username, string password, Import import)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.tradervue.com/api/v1/imports");
            req.Accept = "application/json";
            req.ContentType = "application/json";
            req.UserAgent = "DotNet Sample Application (https://github.com/tradervue/dotnet-sample)";
            req.Credentials = new NetworkCredential(username, password);
            req.Method = "POST";
            Stream rs = req.GetRequestStream();
            StreamWriter strw = new StreamWriter(rs);
            strw.Write(import.ToJson());
            strw.Close();
            rs.Close();

            try
            {
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream s = response.GetResponseStream();
                    StreamReader sr = new StreamReader(s);
                    string content = sr.ReadToEnd();
                    sr.Close();
                    s.Close();
                    response.Close();

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    Dictionary<string, object> tokens = jss.Deserialize<Dictionary<string, object>>(content);
                    return (string)tokens["status"];
                }
                else
                {
                    response.Close();
                    return "Error, status = " + response.StatusCode.ToString();
                }
            }
            catch (WebException e)
            {
                WebExceptionStatus exStatus = e.Status;
                if (exStatus == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse resp = (HttpWebResponse)e.Response;
                    return "Error, status = " + resp.StatusCode.ToString();
                }
                else
                {
                    return "Error occurred: " + e.ToString();
                }
            }
        }

    }
}
