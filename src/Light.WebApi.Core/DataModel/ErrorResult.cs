using System;
namespace Light.WebApi.Core
{
    /// <summary>
    /// Error result.
    /// </summary>
    public class ErrorResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Light.WebApi.Core.ErrorResult"/> class.
        /// </summary>
        public ErrorResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Light.WebApi.Core.ErrorResult"/> class.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="message">Message.</param>
        public ErrorResult(int code, string message)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>The errors.</value>
        public ErrorData[] Errors { get; set; }
    }

    /// <summary>
    /// Error data.
    /// </summary>
    public class ErrorData
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the info.
        /// </summary>
        /// <value>The info.</value>
        public string Info { get; set; }
    }
}
