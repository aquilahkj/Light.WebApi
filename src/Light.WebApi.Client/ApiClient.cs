using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Light.WebApi.Client
{
    public class ApiClient
    {

        readonly string baseAddress;

        string token;

        TimeSpan? timeoutSpan;

        public ApiClient(string baseAddress)
        {
            this.baseAddress = baseAddress;
            if (baseAddress == null) {
                throw new ArgumentNullException(nameof(baseAddress));
            }
        }

        public void SetToken(string token)
        {
            this.token = token;
        }

        public void SetTimeout(int timeout)
        {
            if (timeout > 0) {
                timeoutSpan = new TimeSpan(0, 0, timeout);
            }
        }

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(token)) {
                client.DefaultRequestHeaders.Add("x-token", token);
            }
            if (timeoutSpan != null) {
                client.Timeout = timeoutSpan.Value;
            }
            return client;
        }

        private async Task ParseResponseAsync(HttpResponseMessage httpResponse)
        {
            if (!httpResponse.IsSuccessStatusCode) {
                throw new ApiException(ExceptionType.Http, (int)httpResponse.StatusCode, httpResponse.ReasonPhrase);
            }
            ResultModel item;
            try {
                item = await httpResponse.Content.ReadAsAsync<ResultModel>();
            }
            catch (Exception ex) {
                throw new ApiException(ExceptionType.Parse, 0, ex.Message, ex);
            }
            if (item == null) {
                throw new ApiException(ExceptionType.Parse, 1, SR.ResponseContentError);
            }
            if (item.StatusCode == 0) {
                throw new ApiException(ExceptionType.Result, item.ErrorCode.Value, item.ErrorMsg);
            }
        }

        private async Task<T> ParseResponseToSingleAsync<T>(HttpResponseMessage httpResponse)
        {
            if (!httpResponse.IsSuccessStatusCode) {
                throw new ApiException(ExceptionType.Http, (int)httpResponse.StatusCode, httpResponse.ReasonPhrase);
            }
            ResultModel<T> item;
            try {
                item = await httpResponse.Content.ReadAsAsync<ResultModel<T>>();
            }
            catch (Exception ex) {
                throw new ApiException(ExceptionType.Parse, 0, ex.Message, ex);
            }
            if (item == null) {
                throw new ApiException(ExceptionType.Parse, 1, SR.ResponseContentError);
            }
            if (item.StatusCode == 0) {
                throw new ApiException(ExceptionType.Result, item.ErrorCode.Value, item.ErrorMsg);
            }
            return item.Data;
        }

        private async Task<List<T>> ParseResponseToListAsync<T>(HttpResponseMessage httpResponse)
        {
            if (!httpResponse.IsSuccessStatusCode) {
                throw new ApiException(ExceptionType.Http, (int)httpResponse.StatusCode, httpResponse.ReasonPhrase);
            }
            ResultModel<List<T>> item;
            try {
                item = await httpResponse.Content.ReadAsAsync<ResultModel<List<T>>>();
            }
            catch (Exception ex) {
                throw new ApiException(ExceptionType.Parse, 0, ex.Message, ex);
            }
            if (item == null) {
                throw new ApiException(ExceptionType.Parse, 1, SR.ResponseContentError);
            }
            if (item.StatusCode == 0) {
                throw new ApiException(ExceptionType.Result, item.ErrorCode.Value, item.ErrorMsg);
            }
            return item.Data != null ? item.Data : new List<T>();
        }

        private async Task<PageList<T>> ParseResponseToPageAsync<T>(HttpResponseMessage httpResponse)
        {
            if (!httpResponse.IsSuccessStatusCode) {
                throw new ApiException(ExceptionType.Http, (int)httpResponse.StatusCode, httpResponse.ReasonPhrase);
            }
            ResultModel<List<T>> item;
            try {
                item = await httpResponse.Content.ReadAsAsync<ResultModel<List<T>>>();
            }
            catch (Exception ex) {
                throw new ApiException(ExceptionType.Parse, 0, ex.Message, ex);
            }
            if (item == null) {
                throw new ApiException(ExceptionType.Parse, 1, SR.ResponseContentError);
            }
            if (item.StatusCode == 0) {
                throw new ApiException(ExceptionType.Result, item.ErrorCode.Value, item.ErrorMsg);
            }
            var list = item.Data != null ? item.Data : new List<T>();
            var count = item.TotalCount != null ? item.TotalCount.Value : list.Count;
            return new PageList<T>(list, count);
        }

        private string CombineUri(string requestUri, object value)
        {
            if (value != null) {
                var ps = ParameterConvertor.Convert(value);
                if (!string.IsNullOrEmpty(ps)) {
                    if (requestUri.EndsWith("?", StringComparison.InvariantCultureIgnoreCase)) {
                        requestUri += ps;
                    }
                    else if (requestUri.LastIndexOf('?') > -1) {
                        requestUri += "&" + ps;
                    }
                    else {
                        requestUri += "?" + ps;
                    }
                }
            }
            return requestUri;
        }

        public async Task<T> GetSingleAsync<T>(string requestUri, CancellationToken token = default(CancellationToken))
        {
            using (var client = CreateHttpClient()) {
                HttpResponseMessage response;
                try {
                    response = await client.GetAsync(requestUri, token);
                }
                catch (Exception ex) {
                    throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
                }
                return await ParseResponseToSingleAsync<T>(response);
            }
        }

        public T GetSingle<T>(string requestUri)
        {
            try {
                return GetSingleAsync<T>(requestUri).Result;
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }

        public async Task<List<T>> GetListAsync<T>(string requestUri, CancellationToken token = default(CancellationToken))
        {
            using (var client = CreateHttpClient()) {
                HttpResponseMessage response;
                try {
                    response = await client.GetAsync(requestUri, token);
                }
                catch (Exception ex) {
                    throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
                }
                return await ParseResponseToListAsync<T>(response);
            }
        }

        public List<T> GetList<T>(string requestUri)
        {
            try {
                return GetListAsync<T>(requestUri).Result;
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }

        public async Task<PageList<T>> GetPageAsync<T>(string requestUri, CancellationToken token = default(CancellationToken))
        {
            using (var client = CreateHttpClient()) {
                HttpResponseMessage response;
                try {
                    response = await client.GetAsync(requestUri, token);
                }
                catch (Exception ex) {
                    throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
                }
                return await ParseResponseToPageAsync<T>(response);
            }
        }

        public PageList<T> GetPage<T>(string requestUri)
        {
            try {
                return GetPageAsync<T>(requestUri).Result;
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }

        public async Task<T> GetSingleAsync<T>(string requestUri, object value, CancellationToken token = default(CancellationToken))
        {
            requestUri = CombineUri(requestUri, value);
            using (var client = CreateHttpClient()) {
                HttpResponseMessage response;
                try {
                    response = await client.GetAsync(requestUri, token);
                }
                catch (Exception ex) {
                    throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
                }
                return await ParseResponseToSingleAsync<T>(response);
            }
        }

        public T GetSingle<T>(string requestUri, object value)
        {
            try {
                return GetSingleAsync<T>(requestUri, value).Result;
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }

        public async Task<List<T>> GetListAsync<T>(string requestUri, object value, CancellationToken token = default(CancellationToken))
        {
            requestUri = CombineUri(requestUri, value);
            using (var client = CreateHttpClient()) {
                HttpResponseMessage response;
                try {
                    response = await client.GetAsync(requestUri, token);
                }
                catch (Exception ex) {
                    throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
                }
                return await ParseResponseToListAsync<T>(response);
            }
        }

        public List<T> GetList<T>(string requestUri, object value)
        {
            try {
                return GetListAsync<T>(requestUri, value).Result;
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }

        public async Task<PageList<T>> GetPageAsync<T>(string requestUri, object value, CancellationToken token = default(CancellationToken))
        {
            requestUri = CombineUri(requestUri, value);
            using (var client = CreateHttpClient()) {
                HttpResponseMessage response;
                try {
                    response = await client.GetAsync(requestUri, token);
                }
                catch (Exception ex) {
                    throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
                }
                return await ParseResponseToPageAsync<T>(response);
            }
        }

        public PageList<T> GetPage<T>(string requestUri, object value)
        {
            try {
                return GetPageAsync<T>(requestUri, value).Result;
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }

        public async Task PostAsync(string requestUri, object value, CancellationToken token = default(CancellationToken))
        {
            using (var client = CreateHttpClient()) {
                HttpResponseMessage response;
                try {
                    response = await client.PutAsJsonAsync(requestUri, value, token);
                }
                catch (Exception ex) {
                    throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
                }
                await ParseResponseAsync(response);
            }
        }

        public void Post(string requestUri, object value)
        {
            try {
                PostAsync(requestUri, value).Wait();
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }


        public async Task<T> PostAndGetSingleAsync<T>(string requestUri, object value, CancellationToken token = default(CancellationToken))
        {
            using (var client = CreateHttpClient()) {
                HttpResponseMessage response;
                try {
                    response = await client.PostAsJsonAsync(requestUri, value, token);
                }
                catch (Exception ex) {
                    throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
                }
                return await ParseResponseToSingleAsync<T>(response);
            }
        }

        public T PostAndGetSingle<T>(string requestUri, object value)
        {
            try {
                return PostAndGetSingleAsync<T>(requestUri, value).Result;
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }

        public async Task<List<T>> PostAndGetListAsync<T>(string requestUri, object value, CancellationToken token = default(CancellationToken))
        {
            using (var client = CreateHttpClient()) {
                HttpResponseMessage response;
                try {
                    response = await client.PostAsJsonAsync(requestUri, value, token);
                }
                catch (Exception ex) {
                    throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
                }
                return await ParseResponseToListAsync<T>(response);
            }
        }

        public List<T> PostAndGetList<T>(string requestUri, object value)
        {
            try {
                return PostAndGetListAsync<T>(requestUri, value).Result;
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }

        public async Task<PageList<T>> PostAndGetPageAsync<T>(string requestUri, object value, CancellationToken token = default(CancellationToken))
        {
            using (var client = CreateHttpClient()) {
                HttpResponseMessage response;
                try {
                    response = await client.PostAsJsonAsync(requestUri, value, token);
                }
                catch (Exception ex) {
                    throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
                }
                return await ParseResponseToPageAsync<T>(response);
            }
        }

        public PageList<T> PostAndGetPage<T>(string requestUri, object value)
        {
            try {
                return PostAndGetPageAsync<T>(requestUri, value).Result;
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }

        public async Task PostAsync(string requestUri, CancellationToken token = default(CancellationToken))
        {
            await PostAsync(requestUri, NullObject, token);
        }

        public void Post(string requestUri)
        {
            Post(requestUri, NullObject);
        }

        public async Task<T> PostAndGetSingleAsync<T>(string requestUri, CancellationToken token = default(CancellationToken))
        {
            return await PostAndGetSingleAsync<T>(requestUri, NullObject, token);
        }

        public T PostAndGetSingle<T>(string requestUri)
        {
            return PostAndGetSingle<T>(requestUri, NullObject);
        }

        public async Task<List<T>> PostAndGetListAsync<T>(string requestUri, CancellationToken token = default(CancellationToken))
        {
            return await PostAndGetListAsync<T>(requestUri, NullObject, token);
        }

        public List<T> PostAndGetList<T>(string requestUri)
        {
            return PostAndGetList<T>(requestUri, NullObject);
        }

        public async Task<PageList<T>> PostAndGetPageAsync<T>(string requestUri, CancellationToken token = default(CancellationToken))
        {
            return await PostAndGetPageAsync<T>(requestUri, NullObject, token);
        }

        public PageList<T> PostAndGetPage<T>(string requestUri)
        {
            return PostAndGetPage<T>(requestUri, NullObject);
        }

        public async Task PutAsync(string requestUri, object value, CancellationToken token = default(CancellationToken))
        {
            using (var client = CreateHttpClient()) {
                HttpResponseMessage response;
                try {
                    response = await client.PutAsJsonAsync(requestUri, value, token);
                }
                catch (Exception ex) {
                    throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
                }
                await ParseResponseAsync(response);
            }
        }

        public void Put(string requestUri, object value)
        {
            try {
                PutAsync(requestUri, value).Wait();
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }

        public async Task<T> PutAndGetSingleAsync<T>(string requestUri, object value, CancellationToken token = default(CancellationToken))
        {
            using (var client = CreateHttpClient()) {
                HttpResponseMessage response;
                try {
                    response = await client.PutAsJsonAsync(requestUri, value, token);
                }
                catch (Exception ex) {
                    throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
                }
                return await ParseResponseToSingleAsync<T>(response);
            }
        }

        public T PutAndGetSingle<T>(string requestUri, object value)
        {
            try {
                return PutAndGetSingleAsync<T>(requestUri, value).Result;
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }

        public async Task<List<T>> PutAndGetListAsync<T>(string requestUri, object value, CancellationToken token = default(CancellationToken))
        {
            using (var client = CreateHttpClient()) {
                HttpResponseMessage response;
                try {
                    response = await client.PutAsJsonAsync(requestUri, value, token);
                }
                catch (Exception ex) {
                    throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
                }
                return await ParseResponseToListAsync<T>(response);
            }
        }

        public List<T> PutAndGetList<T>(string requestUri, object value)
        {
            try {
                return PutAndGetListAsync<T>(requestUri, value).Result;
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }

        public async Task<PageList<T>> PutAndGetPageAsync<T>(string requestUri, object value, CancellationToken token = default(CancellationToken))
        {
            using (var client = CreateHttpClient()) {
                HttpResponseMessage response;
                try {
                    response = await client.PutAsJsonAsync(requestUri, value, token);
                }
                catch (Exception ex) {
                    throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
                }
                return await ParseResponseToPageAsync<T>(response);
            }
        }

        public PageList<T> PutAndGetPage<T>(string requestUri, object value)
        {
            try {
                return PutAndGetPageAsync<T>(requestUri, value).Result;
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }

        public async Task DeleteAsync(string requestUri, CancellationToken token = default(CancellationToken))
        {
            using (var client = CreateHttpClient()) {
                HttpResponseMessage response;
                try {
                    response = await client.DeleteAsync(requestUri, token);
                }
                catch (Exception ex) {
                    throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
                }
                await ParseResponseAsync(response);
            }
        }

        public void Delete(string requestUri)
        {
            try {
                DeleteAsync(requestUri).Wait();
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }

        public async Task DeleteAsync(string requestUri, object value, CancellationToken token = default(CancellationToken))
        {
            requestUri = CombineUri(requestUri, value);
            using (var client = CreateHttpClient()) {
                HttpResponseMessage response;
                try {
                    response = await client.DeleteAsync(requestUri, token);
                }
                catch (Exception ex) {
                    throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
                }
                await ParseResponseAsync(response);
            }
        }

        public void Delete(string requestUri, object value)
        {
            try {
                DeleteAsync(requestUri, value).Wait();
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }

        static readonly object NullObject = new object();


    }
}
