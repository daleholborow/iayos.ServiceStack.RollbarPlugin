namespace iayos.ServiceStack.RollbarPlugin.AdaptedFromRollbar
{

	/// <summary>
    /// !! Originally called Exception.cs !!
    /// 
    /// Models Rollbar Exception DTO.
    /// </summary>
    /// <seealso cref="DtoBase" />
    public class RollbarExceptionDto : DtoBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RollbarExceptionDto"/> class.
        /// </summary>
        /// <param name="class">The class.</param>
        public RollbarExceptionDto(string @class)
        {
            Class = @class;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RollbarExceptionDto"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public RollbarExceptionDto(System.Exception exception)
        {
            Assumption.AssertNotNull(exception, nameof(exception));

            Class = exception.GetType().FullName;
            Message = exception.Message;
        }

        /// <summary>
        /// Gets the class.
        /// </summary>
        /// <value>
        /// The class.
        /// </value>
        //[JsonProperty("class", Required = Required.Always)]
        public string Class { get; private set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
       // [JsonProperty("message", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
       // [JsonProperty("description", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Description { get; set; }
    }
}
