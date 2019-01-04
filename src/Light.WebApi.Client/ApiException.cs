using System;
namespace Light.WebApi.Client
{
    public class ApiException : Exception
    {
        readonly int code;

        public int Code {
            get {
                return code;
            }
        }

        readonly ExceptionType type;

        public ExceptionType Type {
            get {
                return type;
            }
        }

        public ApiException(ExceptionType type, int code, string message) : base(message)
        {
            this.type = type;
            this.code = code;
        }

        public ApiException(ExceptionType type, int code, string message, Exception innerException) : base(message, innerException)
        {
            this.type = type;
            this.code = code;
        }

        public string ToApiMessage()
        {
            return $"Type:{type},Code:{code},Message:{Message}";
        }
    }


    public enum ExceptionType
    {
        Serialize,
        Net,
        Http,
        Parse,
        Result
    }
}
