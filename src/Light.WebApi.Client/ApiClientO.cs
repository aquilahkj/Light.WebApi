//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Net;
//using System.Text;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;

//namespace Light.WebApi.Client
//{
//    public class ApiClient
//    {
//        const string GET = "GET";
//        const string POST = "POST";
//        const string JSON_CONTENT_TYPE = "application/json";
//        const string POST_JSON_CONTENT_TYPE = "application/json; charset=utf-8";

//        static ApiClient()
//        {
//            System.Net.ServicePointManager.DefaultConnectionLimit = 2000;
//        }

//        JsonSerializerSettings settings;

//        public ApiClient(string baseAddress)
//        {
//            //if (baseAddress == null) {
//            //    throw new ArgumentNullException(nameof(baseAddress));
//            //}
//            //if (!baseAddress.StartsWith("http://", StringComparison.Ordinal) && !baseAddress.StartsWith("https://", StringComparison.Ordinal)) {
//            //    throw new ArgumentException(nameof(baseAddress) + " format error");
//            //}
//            this.baseAddress = baseAddress;

//            settings = new JsonSerializerSettings();
//            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
//        }

//        readonly string baseAddress;
//        int? timeout;
//        string token;

//        public void SetToken(string token)
//        {
//            this.token = token;
//        }

//        public void SetTimeout(int timeout)
//        {
//            if (timeout > 0) {
//                this.timeout = timeout * 1000;
//            }
//        }

//        private HttpWebRequest CreateHttpRequest(string url, string method)
//        {
//            HttpWebRequest request = WebRequest.Create(baseAddress + url) as HttpWebRequest;
//            request.ServicePoint.ConnectionLimit = 2000;
//            request.AllowAutoRedirect = true;
//            request.Method = method;
//            request.Accept = JSON_CONTENT_TYPE;
//            request.KeepAlive = true;
//            request.Proxy = null;
//            if (timeout != null) {
//                request.Timeout = timeout.Value;
//            }
//            if (!string.IsNullOrEmpty(token)) {
//                request.Headers.Add("x-token", token);
//            }
//            return request;
//        }

//        private string SerializeObject(object value)
//        {
//            if (value == null) {
//                return "{}";
//            }
//            try {
//                var content = JsonConvert.SerializeObject(value, settings);
//                return content;
//            }
//            catch (Exception ex) {
//                throw new ApiException(ExceptionType.Serialize, 0, ex.Message);
//            }
//        }


//        private string PostAndGetResponseString(HttpWebRequest request, string input)
//        {
//            HttpWebResponse response = null;
//            Stream stream = null;
//            try {
//                request.ContentType = POST_JSON_CONTENT_TYPE;
//                stream = request.GetRequestStream();
//                byte[] buffer = Encoding.UTF8.GetBytes(input);
//                stream.Write(buffer, 0, buffer.Length);
//                response = request.GetResponse() as HttpWebResponse;
//                if (response.StatusCode != HttpStatusCode.OK) {
//                    throw new ApiException(ExceptionType.Http, (int)response.StatusCode, response.StatusDescription);
//                }
//                string content;
//                Encoding encoding;
//                if (string.IsNullOrEmpty(response.CharacterSet)) {
//                    encoding = Encoding.GetEncoding(response.CharacterSet);
//                }
//                else {
//                    encoding = Encoding.UTF8;
//                }
//                using (var reader = new StreamReader(response.GetResponseStream(), encoding)) {
//                    content = reader.ReadToEnd();
//                }
//                return content;
//            }
//            catch (ApiException ex) {
//                throw ex;
//            }
//            catch (Exception ex) {
//                throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
//            }
//            finally {
//                if (stream != null) {
//                    stream.Dispose();
//                }
//                if (response != null) {
//                    response.Dispose();
//                }
//            }
//        }


//        private string GetResponseString(HttpWebRequest request)
//        {
//            HttpWebResponse response = null;
//            try {
//                response = request.GetResponse() as HttpWebResponse;
//                if (response.StatusCode != HttpStatusCode.OK) {
//                    throw new ApiException(ExceptionType.Http, (int)response.StatusCode, response.StatusDescription);
//                }
//                string content;
//                Encoding encoding;
//                if (string.IsNullOrEmpty(response.CharacterSet)) {
//                    encoding = Encoding.GetEncoding(response.CharacterSet);
//                }
//                else {
//                    encoding = Encoding.UTF8;
//                }
//                using (var reader = new StreamReader(response.GetResponseStream(), encoding)) {
//                    content = reader.ReadToEnd();
//                }
//                return content;
//            }
//            catch (ApiException ex) {
//                throw ex;
//            }
//            catch (Exception ex) {
//                throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
//            }
//            finally {
//                if (response != null) {
//                    response.Dispose();
//                }
//            }
//        }


//        private void ParseVoid(string content)
//        {
//            ResultModel item;
//            try {
//                item = JsonConvert.DeserializeObject<ResultModel>(content, settings);
//            }
//            catch (Exception ex) {
//                throw new ApiException(ExceptionType.Parse, 0, ex.Message, ex);
//            }
//            if (item == null) {
//                throw new ApiException(ExceptionType.Parse, 1, SR.ResponseContentError);
//            }
//            if (item.StatusCode == 0) {
//                throw new ApiException(ExceptionType.Result, item.ErrorCode.Value, item.ErrorMsg);
//            }
//        }

//        private T ParseSingle<T>(string content)
//        {
//            ResultModel<T> item;
//            try {
//                item = JsonConvert.DeserializeObject<ResultModel<T>>(content, settings);
//            }
//            catch (Exception ex) {
//                throw new ApiException(ExceptionType.Parse, 0, ex.Message, ex);
//            }
//            if (item == null) {
//                throw new ApiException(ExceptionType.Parse, 1, SR.ResponseContentError);
//            }
//            if (item.StatusCode == 0) {
//                throw new ApiException(ExceptionType.Result, item.ErrorCode.Value, item.ErrorMsg);
//            }
//            return item.Data;
//        }

//        private List<T> ParseList<T>(string content)
//        {
//            ResultModel<List<T>> item;
//            try {
//                item = JsonConvert.DeserializeObject<ResultModel<List<T>>>(content, settings);
//            }
//            catch (Exception ex) {
//                throw new ApiException(ExceptionType.Parse, 0, ex.Message, ex);
//            }
//            if (item == null) {
//                throw new ApiException(ExceptionType.Parse, 1, SR.ResponseContentError);
//            }
//            if (item.StatusCode == 0) {
//                throw new ApiException(ExceptionType.Result, item.ErrorCode.Value, item.ErrorMsg);
//            }
//            return item.Data != null ? item.Data : new List<T>();
//        }


//        private PageList<T> ParsePage<T>(string content)
//        {
//            ResultModel<List<T>> item;
//            try {
//                item = JsonConvert.DeserializeObject<ResultModel<List<T>>>(content, settings);
//            }
//            catch (Exception ex) {
//                throw new ApiException(ExceptionType.Parse, 0, ex.Message, ex);
//            }
//            if (item == null) {
//                throw new ApiException(ExceptionType.Parse, 1, SR.ResponseContentError);
//            }
//            if (item.StatusCode == 0) {
//                throw new ApiException(ExceptionType.Result, item.ErrorCode.Value, item.ErrorMsg);
//            }
//            var list = item.Data != null ? item.Data : new List<T>();
//            var count = item.TotalCount != null ? item.TotalCount.Value : list.Count;
//            return new PageList<T>(list, count);
//        }

//        private string CombineUri(string requestUri, object value)
//        {
//            if (value != null) {
//                var ps = ParameterConvertor.Convert(value);
//                if (!string.IsNullOrEmpty(ps)) {
//                    if (requestUri.EndsWith("?", StringComparison.InvariantCultureIgnoreCase)) {
//                        requestUri += ps;
//                    }
//                    else if (requestUri.LastIndexOf('?') > -1) {
//                        requestUri += "&" + ps;
//                    }
//                    else {
//                        requestUri += "?" + ps;
//                    }
//                }
//            }
//            return requestUri;
//        }

//        public T Get<T>(string requestUri, object value)
//        {
//            requestUri = CombineUri(requestUri, value);
//            var request = CreateHttpRequest(requestUri, GET);
//            string content = GetResponseString(request);
//            return ParseSingle<T>(content);
//        }

//        public List<T> GetList<T>(string requestUri, object value)
//        {
//            requestUri = CombineUri(requestUri, value);
//            var request = CreateHttpRequest(requestUri, GET);
//            string content = GetResponseString(request);
//            return ParseList<T>(content);
//        }

//        public PageList<T> GetPage<T>(string requestUri, object value)
//        {
//            requestUri = CombineUri(requestUri, value);
//            var request = CreateHttpRequest(requestUri, GET);
//            string content = GetResponseString(request);
//            return ParsePage<T>(content);

//        }

//        public T Get<T>(string requestUri)
//        {
//            return Get<T>(requestUri, null);
//        }

//        public List<T> GetList<T>(string requestUri)
//        {
//            return GetList<T>(requestUri, null);
//        }

//        public PageList<T> GetPage<T>(string requestUri)
//        {
//            return GetPage<T>(requestUri, null);
//        }

//        public void Post(string requestUri, object value)
//        {
//            var request = CreateHttpRequest(requestUri, POST);
//            var input = SerializeObject(value);
//            string content = PostAndGetResponseString(request, input);
//            ParseVoid(content);
//        }

//        public T PostAndGet<T>(string requestUri, object value)
//        {
//            var request = CreateHttpRequest(requestUri, POST);
//            var input = SerializeObject(value);
//            string content = PostAndGetResponseString(request, input);
//            return ParseSingle<T>(content);
//        }

//        public List<T> PostAndGetList<T>(string requestUri, object value)
//        {
//            var request = CreateHttpRequest(requestUri, POST);
//            var input = SerializeObject(value);
//            string content = PostAndGetResponseString(request, input);
//            return ParseList<T>(content);
//        }

//        public PageList<T> PostAndGetPage<T>(string requestUri, object value)
//        {
//            var request = CreateHttpRequest(requestUri, POST);
//            var input = SerializeObject(value);
//            string content = PostAndGetResponseString(request, input);
//            return ParsePage<T>(content);
//        }


//        public void Post(string requestUri)
//        {
//            Post(requestUri, null);
//        }

//        public T PostAndGet<T>(string requestUri)
//        {
//            return PostAndGet<T>(requestUri, null);
//        }

//        public List<T> PostAndGetList<T>(string requestUri)
//        {
//            return PostAndGetList<T>(requestUri, null);
//        }

//        public PageList<T> PostAndGetPage<T>(string requestUri)
//        {
//            return PostAndGetPage<T>(requestUri, null);
//        }
//    }
//}
