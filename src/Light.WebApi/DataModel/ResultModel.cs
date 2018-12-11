using System;
using System.Collections.Generic;

namespace Light.WebApi
{
    public abstract class ResultModel
    {
        public static ResultModel CreateErrorResult(int errorCode, string errorMsg)
        {
            var result = new ErrorResultModel(errorCode, errorMsg);
            return result;
        }

        public static ResultModel CerateSuccessResult()
        {
            var result = new SuccessResultModel();
            return result;
        }

        public static ResultModel CreateDataResult<T>(T data)
        {
            var result = new DataResultModel<T>(data);
            return result;
        }

        public static ResultModel CreateCollectionResult<T>(IEnumerable<T> data)
        {
            var result = new CollectionResultModel<T>(data);
            return result;
        }

        public static ResultModel CreatePageResult<T>(IEnumerable<T> data, int totalCount)
        {
            var result = new PageResultModel<T>(data, totalCount);
            return result;
        }


        private readonly int statusCode;

        public int StatusCode {
            get {
                return statusCode;
            }
        }

        protected ResultModel(int statusCode)
        {
            this.statusCode = statusCode;
        }
    }

    class ErrorResultModel : ResultModel
    {
        private readonly int errorCode;

        public int ErrorCode {
            get {
                return errorCode;
            }
        }

        private readonly string errorMsg;

        public string ErrorMsg {
            get {
                return errorMsg;
            }
        }

        public ErrorResultModel(int errorCode, string errorMsg) : base(0)
        {
            this.errorCode = errorCode;
            this.errorMsg = errorMsg;
        }
    }

    class SuccessResultModel : ResultModel
    {
        public SuccessResultModel() : base(1)
        {

        }
    }

    class DataResultModel<T> : ResultModel
    {
        private readonly T data;

        public T Data {
            get {
                return data;
            }
        }

        public DataResultModel(T data) : base(1)
        {
            this.data = data;
        }
    }

    class CollectionResultModel<T> : ResultModel
    {
        private readonly IEnumerable<T> data;

        public IEnumerable<T> Data {
            get {
                return data;
            }
        }

        public CollectionResultModel(IEnumerable<T> data) : base(1)
        {
            this.data = data;
        }
    }

    class PageResultModel<T> : CollectionResultModel<T>
    {
        private readonly int totalCount;

        public int TotalCount {
            get {
                return totalCount;
            }
        }

        public PageResultModel(IEnumerable<T> data, int totalCount) : base(data)
        {
            this.totalCount = totalCount;
        }
    }
}
