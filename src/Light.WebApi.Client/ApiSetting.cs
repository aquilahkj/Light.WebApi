using System;
namespace Microsoft.Extensions.DependencyInjection
{
    public class ApiSetting
    {
        public string BaseAddress { get; set; }

        public int? Timeout { get; set; }
    }
}
