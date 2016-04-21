using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Couchbase;

namespace TaskManager.Domain.Infrastructure
{
    public class CouchbaseRestClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly UriBuilder _uriBuilder;
            private readonly string _username;
            private readonly string _password;

        public CouchbaseRestClient()
        {
            _httpClient = new HttpClient();
            string host = ConfigurationManager.AppSettings.Get("couchbase.url.host");
            int port = int.Parse(ConfigurationManager.AppSettings.Get("couchbase.url.port"));

            _uriBuilder = new UriBuilder("http", host, port);

            _username = ConfigurationManager.AppSettings.Get("couchbase.credentials.username");
            _password = ConfigurationManager.AppSettings.Get("couchbase.credentials.password");
            var auth = string.Format("{0}:{1}", _username, _password);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", ConvertFromUtf8ToBase64(auth));
        }

        public HttpResponseMessage DeleteBucket(string bucketName)
        {
            _uriBuilder.Path = string.Format("pools/default/buckets/{0}", bucketName);
            Task<HttpResponseMessage> request = _httpClient.DeleteAsync(_uriBuilder.Uri);
            HttpResponseMessage httpResponseMessage = request.Result;
            return httpResponseMessage;
        }
        
        public HttpResponseMessage GetBucket(string bucketName)
        {
            _uriBuilder.Path = string.Format("pools/default/buckets/{0}", bucketName);
            Task<HttpResponseMessage> request = _httpClient.GetAsync(_uriBuilder.ToString());
            HttpResponseMessage httpResponseMessage = request.Result;
            return httpResponseMessage;
        }

        public HttpResponseMessage CreateBucket(string bucketName, int proxyPort)
        {
            var formUrlEncodedContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("authType", "none"),
                new KeyValuePair<string, string>("name", bucketName),
                new KeyValuePair<string, string>("ramQuotaMB", "100"),
                new KeyValuePair<string, string>("replicaNumber", "1"),
                new KeyValuePair<string, string>("flushEnabled", "1"),
                new KeyValuePair<string, string>("proxyPort", proxyPort.ToString())
            });


            _uriBuilder.Path = "pools/default/buckets";
            Task<HttpResponseMessage> request = _httpClient.PostAsync(_uriBuilder.ToString(), formUrlEncodedContent);
            HttpResponseMessage httpResponseMessage = request.Result;
            return httpResponseMessage;
        }

        public HttpResponseMessage FlushBucket(string bucketName)
        {
            _uriBuilder.Path = string.Format("pools/default/buckets/{0}/controller/doFlush", bucketName);
            Task<HttpResponseMessage> request = _httpClient.PostAsync(_uriBuilder.Uri, new StringContent(""));
            HttpResponseMessage httpResponseMessage = request.Result;
            var debug = request.Result.Content.ReadAsStringAsync().Result;
            return httpResponseMessage;
        }

        private string ConvertFromUtf8ToBase64(string utf8String)
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(utf8String);
            string base64String = Convert.ToBase64String(utf8Bytes);
            return base64String;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}