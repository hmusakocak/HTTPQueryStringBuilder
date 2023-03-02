using System;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace QueryBuilder
{
    public class QueryBuild
    {
        public string paramURL = String.Empty;
        public string[] parameters;
        public QueryBuild(string URL, string[] queryParams)
        {
            paramURL = URL;
            parameters = queryParams;
        }
        public QueryBuild(string[] queryParams)
        {
            parameters = queryParams;
        }

        public string GenerateQueryString(string[] values)
        {
            if (paramURL != String.Empty)
            {
                var builder = new UriBuilder(paramURL);
                builder.Port = -1;
                var querystring = HttpUtility.ParseQueryString(builder.Query);
                for (int i = 0; i < parameters.Length; i++)
                {
                    querystring.Add(parameters[i], values[i]);
                }
                builder.Query = querystring.ToString();
                string resultURL = builder.ToString();
                return resultURL;
            }
            else
            {
                var querystring = HttpUtility.ParseQueryString(String.Empty);
                for (int i = 0; i < parameters.Length; i++)
                {
                    querystring[parameters[i]] = values[i];
                }
                var result = querystring.ToString();
                return result;
            }
        }
        public string GetHTML(string[] values)
        {
            string data;
            var url = new QueryBuild(paramURL, parameters).GenerateQueryString(values);
            var request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                var response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;
                    if (String.IsNullOrWhiteSpace(response.CharacterSet))
                    {
                        //reads without encoding
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        //if encoding found, encodes according to website encoding type
                        readStream = new StreamReader(receiveStream,
                            Encoding.GetEncoding(response.CharacterSet));
                    }

                    data = readStream.ReadToEnd();
                    response.Close();
                    readStream.Close();
                }
                else
                {
                    data = "HTTP STATUS NOT OK!";
                }
                return data;
            }
            catch (Exception)
            {
                data = "undefined URL";
                return data;
            }
           

        }
    }

}
