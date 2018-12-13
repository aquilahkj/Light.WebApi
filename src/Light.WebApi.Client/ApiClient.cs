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

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(token)) {
                client.DefaultRequestHeaders.Add("x-token", token);
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


        public async Task PostAsync<K>(string requestUri, K value, CancellationToken token = default(CancellationToken))
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

        public async Task<T> PostAndGetSingleAsync<T, K>(string requestUri, K value, CancellationToken token = default(CancellationToken))
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

        public async Task<List<T>> PostAndGetListAsync<T, K>(string requestUri, K value, CancellationToken token = default(CancellationToken))
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

        public async Task<PageList<T>> PostAndGetPageAsync<T, K>(string requestUri, K value, CancellationToken token = default(CancellationToken))
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

        public async Task PostAsync(string requestUri, CancellationToken token = default(CancellationToken))
        {
            await PostAsync<NullObject>(requestUri, NullObject.Instance, token);
        }

        public async Task<T> PostAndGetSingleAsync<T>(string requestUri, CancellationToken token = default(CancellationToken))
        {
            return await PostAndGetSingleAsync<T, NullObject>(requestUri, NullObject.Instance, token);
        }

        public async Task<List<T>> PostAndGetListAsync<T>(string requestUri, CancellationToken token = default(CancellationToken))
        {
            return await PostAndGetListAsync<T, NullObject>(requestUri, NullObject.Instance, token);
        }

        public async Task<PageList<T>> PostAndGetPageAsync<T>(string requestUri, CancellationToken token = default(CancellationToken))
        {
            return await PostAndGetPageAsync<T, NullObject>(requestUri, NullObject.Instance, token);

        }

        public async Task PutAsync<K>(string requestUri, K value, CancellationToken token = default(CancellationToken))
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

        public async Task<T> PutAndGetSingleAsync<T, K>(string requestUri, K value, CancellationToken token = default(CancellationToken))
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

        public async Task<List<T>> PutAndGetListAsync<T, K>(string requestUri, K value, CancellationToken token = default(CancellationToken))
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

        public async Task<PageList<T>> PutAndGetPageAsync<T, K>(string requestUri, K value, CancellationToken token = default(CancellationToken))
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

        private sealed class NullObject
        {
            static readonly NullObject instance = new NullObject();

            public static NullObject Instance {
                get { return instance; }
            }

        }
    }
}
