using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Light.WebApi.Client
{
    public class ApiClient
    {
        readonly HttpClient client;

        public ApiClient(HttpClient client)
        {
            this.client = client;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async Task<T> ReadData<T>(HttpResponseMessage httpResponse) where T : ResultModel
        {
            if (!httpResponse.IsSuccessStatusCode) {
                throw new ApiException(ExceptionType.Http, (int)httpResponse.StatusCode, httpResponse.ReasonPhrase);
            }
            T item;
            try {
                item = await httpResponse.Content.ReadAsAsync<T>();
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
            return item;
        }

        private async Task ParseResponseAsync(HttpResponseMessage httpResponse)
        {
            var item = await ReadData<ResultModel>(httpResponse);
        }

        private async Task<T> ParseResponseToSingleAsync<T>(HttpResponseMessage httpResponse)
        {
            var item = await ReadData<ResultModel<T>>(httpResponse);
            return item.Data;
        }

        private async Task<List<T>> ParseResponseToListAsync<T>(HttpResponseMessage httpResponse)
        {
            var item = await ReadData<ResultModel<List<T>>>(httpResponse);
            return item.Data != null ? item.Data : new List<T>();
        }

        private async Task<PageList<T>> ParseResponseToPageAsync<T>(HttpResponseMessage httpResponse)
        {
            var item = await ReadData<ResultModel<List<T>>>(httpResponse);
            var list = item.Data != null ? item.Data : new List<T>();
            var count = item.TotalCount != null ? item.TotalCount.Value : list.Count;
            return new PageList<T>(list, count);
        }

        private string CombineUri(string requestUri, object value)
        {
            if (value != null) {
                TypeCode code = Type.GetTypeCode(value.GetType());
                string ps;
                if (code == TypeCode.Object) {
                    ps = ParameterConvertor.Convert(value);
                }
                else if (code == TypeCode.String) {
                    ps = value as string;
                }
                else {
                    ps = value.ToString();
                }
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

        private async Task<HttpResponseMessage> Internal_GetAsync(string requestUri, object value, CancellationToken cancellationToken)
        {
            requestUri = CombineUri(requestUri, value);
            HttpResponseMessage response;
            try {
                response = await client.GetAsync(requestUri, cancellationToken);
            }
            catch (Exception ex) {
                throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
            }
            return response;
        }

        public async Task<T> GetAsync<T>(string requestUri, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await Internal_GetAsync(requestUri, null, cancellationToken);
            return await ParseResponseToSingleAsync<T>(response);
        }

        public T Get<T>(string requestUri)
        {
            try {
                return GetAsync<T>(requestUri).Result;
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }

        public async Task<List<T>> GetListAsync<T>(string requestUri, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await Internal_GetAsync(requestUri, null, cancellationToken);
            return await ParseResponseToListAsync<T>(response);
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

        public async Task<PageList<T>> GetPageAsync<T>(string requestUri, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await Internal_GetAsync(requestUri, null, cancellationToken);
            return await ParseResponseToPageAsync<T>(response);
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

        public async Task<T> GetAsync<T>(string requestUri, object value, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await Internal_GetAsync(requestUri, value, cancellationToken);
            return await ParseResponseToSingleAsync<T>(response);
        }

        public T Get<T>(string requestUri, object value)
        {
            try {
                return GetAsync<T>(requestUri, value).Result;
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }

        public async Task<List<T>> GetListAsync<T>(string requestUri, object value, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await Internal_GetAsync(requestUri, value, cancellationToken);
            return await ParseResponseToListAsync<T>(response);
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

        public async Task<PageList<T>> GetPageAsync<T>(string requestUri, object value, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await Internal_GetAsync(requestUri, value, cancellationToken);
            return await ParseResponseToPageAsync<T>(response);
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

        private async Task<HttpResponseMessage> Internal_PostAsync(string requestUri, object value, CancellationToken cancellationToken)
        {
            HttpResponseMessage response;
            try {
                if (value != null) {
                    response = await client.PostAsJsonAsync(requestUri, value, cancellationToken);
                }
                else {
                    response = await client.PostAsJsonAsync(requestUri, "{}", cancellationToken);
                }
            }
            catch (Exception ex) {
                throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
            }
            return response;
        }

        public async Task PostAsync(string requestUri, object value, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await Internal_PostAsync(requestUri, value, cancellationToken);
            await ParseResponseAsync(response);
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

        public async Task<T> PostAndGetAsync<T>(string requestUri, object value, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await Internal_PostAsync(requestUri, value, cancellationToken);
            return await ParseResponseToSingleAsync<T>(response);
        }

        public T PostAndGet<T>(string requestUri, object value)
        {
            try {
                return PostAndGetAsync<T>(requestUri, value).Result;
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }

        public async Task<List<T>> PostAndGetListAsync<T>(string requestUri, object value, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await Internal_PostAsync(requestUri, value, cancellationToken);
            return await ParseResponseToListAsync<T>(response);
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

        public async Task<PageList<T>> PostAndGetPageAsync<T>(string requestUri, object value, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await Internal_PostAsync(requestUri, value, cancellationToken);
            return await ParseResponseToPageAsync<T>(response);
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

        public async Task PostAsync(string requestUri, CancellationToken cancellationToken = default(CancellationToken))
        {
            await PostAsync(requestUri, null, cancellationToken);
        }

        public void Post(string requestUri)
        {
            Post(requestUri, null);
        }

        public async Task<T> PostAndGetAsync<T>(string requestUri, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PostAndGetAsync<T>(requestUri, null, cancellationToken);
        }

        public T PostAndGet<T>(string requestUri)
        {
            return PostAndGet<T>(requestUri, null);
        }

        public async Task<List<T>> PostAndGetListAsync<T>(string requestUri, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PostAndGetListAsync<T>(requestUri, null, cancellationToken);
        }

        public List<T> PostAndGetList<T>(string requestUri)
        {
            return PostAndGetList<T>(requestUri, null);
        }

        public async Task<PageList<T>> PostAndGetPageAsync<T>(string requestUri, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PostAndGetPageAsync<T>(requestUri, null, cancellationToken);
        }

        public PageList<T> PostAndGetPage<T>(string requestUri)
        {
            return PostAndGetPage<T>(requestUri, null);
        }

        private async Task<HttpResponseMessage> Internal_PutAsync(string requestUri, object value, CancellationToken cancellationToken)
        {
            if (value == null) {
                throw new ApiException(ExceptionType.Net, 0, "value is null");
            }

            HttpResponseMessage response;
            try {
                response = await client.PutAsJsonAsync(requestUri, value, cancellationToken);
            }
            catch (Exception ex) {
                throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
            }
            return response;
        }

        public async Task PutAsync(string requestUri, object value, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await Internal_PutAsync(requestUri, value, cancellationToken);
            await ParseResponseAsync(response);
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

        public async Task<T> PutAndGetAsync<T>(string requestUri, object value, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await Internal_PutAsync(requestUri, value, cancellationToken);
            return await ParseResponseToSingleAsync<T>(response);
        }

        public T PutAndGet<T>(string requestUri, object value)
        {
            try {
                return PutAndGetAsync<T>(requestUri, value).Result;
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }

        //public async Task<List<T>> PutAndGetListAsync<T>(string requestUri, object value, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    var response = await Internal_PutAsync(requestUri, value, cancellationToken);
        //    return await ParseResponseToListAsync<T>(response);
        //}

        //public List<T> PutAndGetList<T>(string requestUri, object value)
        //{
        //    try {
        //        return PutAndGetListAsync<T>(requestUri, value).Result;
        //    }
        //    catch (AggregateException ex) {
        //        throw ex.InnerException;
        //    }
        //}

        //public async Task<PageList<T>> PutAndGetPageAsync<T>(string requestUri, object value, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    var response = await Internal_PutAsync(requestUri, value, cancellationToken);
        //    return await ParseResponseToPageAsync<T>(response);
        //}

        //public PageList<T> PutAndGetPage<T>(string requestUri, object value)
        //{
        //    try {
        //        return PutAndGetPageAsync<T>(requestUri, value).Result;
        //    }
        //    catch (AggregateException ex) {
        //        throw ex.InnerException;
        //    }
        //}

        private async Task<HttpResponseMessage> Internal_DeleteAsync(string requestUri, object value, CancellationToken cancellationToken)
        {
            requestUri = CombineUri(requestUri, value);
            HttpResponseMessage response;
            try {
                response = await client.DeleteAsync(requestUri, cancellationToken);
            }
            catch (Exception ex) {
                throw new ApiException(ExceptionType.Net, 0, ex.Message, ex);
            }
            return response;
        }

        public async Task DeleteAsync(string requestUri, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await Internal_DeleteAsync(requestUri, null, cancellationToken);
            await ParseResponseAsync(response);
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

        public async Task DeleteAsync(string requestUri, object value, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await Internal_DeleteAsync(requestUri, value, cancellationToken);
            await ParseResponseAsync(response);
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
    }
}
