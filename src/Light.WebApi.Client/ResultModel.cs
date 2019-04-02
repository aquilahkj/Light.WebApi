using System;
namespace Light.WebApi.Client
{
    class ResultModel
    {
        public int? ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

        public int? TotalCount { get; set; }

        public int StatusCode { get; set; }
    }

    class ResultModel<T> : ResultModel
    {
        public T Data { get; set; }
    }
}
