using System;
using System.Collections.Generic;

namespace Light.WebApi.Core
{
    /// <summary>
    /// Result model.
    /// </summary>
    public abstract class ResultModel
    {
        public static ErrorResultModel CreateErrorResult(int errorCode, string errorMsg)
        {
            var result = new ErrorResultModel(errorCode, errorMsg);
            return result;
        }

        public static SuccessResultModel CerateSuccessResult()
        {
            var result = new SuccessResultModel();
            return result;
        }

        public static DataResultModel<T> CreateDataResult<T>(T data)
        {
            var result = new DataResultModel<T>(data);
            return result;
        }

        public static CollectionResultModel<T> CreateCollectionResult<T>(IEnumerable<T> data)
        {
            var result = new CollectionResultModel<T>(data);
            return result;
        }

        public static PageResultModel<T> CreatePageResult<T>(IEnumerable<T> data, int totalCount)
        {
            var result = new PageResultModel<T>(data, totalCount);
            return result;
        }


        private readonly int statusCode;

        /// <summary>
        /// Result status code.
        /// </summary>
        /// <value>The status code.</value>
        public int StatusCode {
            get {
                return statusCode;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Light.WebApi.Core.ResultModel"/> class.
        /// </summary>
        /// <param name="statusCode">Status code.</param>
        protected ResultModel(int statusCode)
        {
            this.statusCode = statusCode;
        }
    }

    /// <summary>
    /// Error result model.
    /// </summary>
    public class ErrorResultModel : ResultModel
    {
        private readonly int errorCode;

        /// <summary>
        /// Result error code.
        /// </summary>
        /// <value>The error code.</value>
        public int ErrorCode {
            get {
                return errorCode;
            }
        }

        private readonly string errorMsg;

        /// <summary>
        /// Result error message.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMsg {
            get {
                return errorMsg;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Light.WebApi.Core.ErrorResultModel"/> class.
        /// </summary>
        /// <param name="errorCode">Error code.</param>
        /// <param name="errorMsg">Error message.</param>
        internal ErrorResultModel(int errorCode, string errorMsg) : base(0)
        {
            this.errorCode = errorCode;
            this.errorMsg = errorMsg;
        }
    }

    /// <summary>
    /// Success result model.
    /// </summary>
    public class SuccessResultModel : ResultModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Light.WebApi.Core.SuccessResultModel"/> class.
        /// </summary>
        public SuccessResultModel() : base(1)
        {

        }
    }

    /// <summary>
    /// Data result model.
    /// </summary>
    public class DataResultModel<T> : ResultModel
    {
        private readonly T data;

        /// <summary>
        /// Result data
        /// </summary>
        /// <value>The data.</value>
        public T Data {
            get {
                return data;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Light.WebApi.Core.DataResultModel`1"/> class.
        /// </summary>
        /// <param name="data">Data.</param>
        internal DataResultModel(T data) : base(1)
        {
            this.data = data;
        }
    }

    /// <summary>
    /// Collection result model.
    /// </summary>
    public class CollectionResultModel<T> : ResultModel
    {
        private readonly IEnumerable<T> data;

        /// <summary>
        /// Result data array
        /// </summary>
        /// <value>The data.</value>
        public IEnumerable<T> Data {
            get {
                return data;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Light.WebApi.Core.CollectionResultModel`1"/> class.
        /// </summary>
        /// <param name="data">Data.</param>
        internal CollectionResultModel(IEnumerable<T> data) : base(1)
        {
            this.data = data;
        }
    }

    /// <summary>
    /// Page result model.
    /// </summary>
    public class PageResultModel<T> : CollectionResultModel<T>
    {
        private readonly int totalCount;

        /// <summary>
        /// The total count
        /// </summary>
        /// <value>The total count.</value>
        public int TotalCount {
            get {
                return totalCount;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Light.WebApi.Core.PageResultModel`1"/> class.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="totalCount">Total count.</param>
        internal PageResultModel(IEnumerable<T> data, int totalCount) : base(data)
        {
            this.totalCount = totalCount;
        }
    }
}
