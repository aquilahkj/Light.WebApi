namespace Microsoft.AspNetCore.Mvc
{
    public class InvalidModelStateSetting
    {
        public int? Code { get; set; }

        public string Message { get; set; }

        public bool? ShowFieldName { get; set; }

        public bool? ShowErrorDetail { get; set; }
    }
}