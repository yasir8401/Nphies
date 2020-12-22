
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;


namespace WebApplication8.Helpers
{
    public static class RestUtility
    {
        public static string Msg = string.Empty;

        public static object CallService<T>(string url, string operation, object requestBodyObject, string method, string clientID, string clientSecret, string providerId, out HttpStatusCode status) where T : class
        {
            try
            {

                // Initialize an HttpWebRequest for the current URL.
                var webReq = (HttpWebRequest)WebRequest.Create(url);
                webReq.Method = method;
                webReq.Accept = "application/json";


                //Add basic authentication header if username is supplied
                if (!string.IsNullOrEmpty(clientID))
                {
                    webReq.Headers["X-IBM-Client-Id"] = clientID;
                    webReq.Headers["X-IBM-Client-Secret"] = clientSecret;
                    webReq.Headers["providerId"] = providerId;
                }

                //Add key to header if operation is supplied
                if (!string.IsNullOrEmpty(operation))
                {
                    webReq.Headers["Operation"] = operation;
                }


                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyErrors) { return true; }; 
                //Serialize request object as JSON and write to request body
                if (requestBodyObject != null)
                {

                    var requestBody = JsonConvert.SerializeObject(requestBodyObject);

                    webReq.ContentLength = requestBody.Length;
                    webReq.ContentType = "application/json";

                    var streamWriter = new StreamWriter(webReq.GetRequestStream(), Encoding.ASCII);
                    streamWriter.Write(requestBody);
                    streamWriter.Close();
                }

                var response = webReq.GetResponse();

                status = ((HttpWebResponse)response).StatusCode;

                if (response == null)
                {
                    return null;
                }

                status = ((HttpWebResponse)response).StatusCode;

                var streamReader = new StreamReader(response.GetResponseStream());

                var responseContent = streamReader.ReadToEnd().Trim();

                var jsonObject = JsonConvert.DeserializeObject<T>(responseContent);

                return jsonObject;
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)wex.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string errorContennt = reader.ReadToEnd().Trim();
                            var jsonObject = JsonConvert.DeserializeObject<ErrorResponseObject>(errorContennt);

                            status = ((System.Net.HttpWebResponse)(wex.Response)).StatusCode;
                            Msg = jsonObject.details;

                            return null;
                        }
                    }

                }

                status = HttpStatusCode.InternalServerError;

                return null;
            }

        }
    }

    public class ErrorResponseObject
    {
        public string timestamp { get; set; }
        public string message { get; set; }
        public string details { get; set; }
    }
}

