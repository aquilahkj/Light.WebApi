using System;
namespace Light.WebApi.Core
{
    public class SuccessResult
    {
        public SuccessResult()
        {
            Result = "success";
        }

        public string Result { get; set; }
    }
}
