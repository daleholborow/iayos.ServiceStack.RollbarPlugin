

using System.Runtime.Serialization;

namespace iayos.ServiceStack.RollbarPlugin.AdaptedFromRollbar
{
	/// <summary>
    /// Models Rollbar Person DTO.
    /// </summary>
    /// <seealso cref="DtoBase" />
    public class Person
        : DtoBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        public Person()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public Person(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
       // [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>
		/// The name of the user.
		/// </value>
		// [JsonProperty("username", DefaultValueHandling = DefaultValueHandling.Ignore)]
		[DataMember(Name = "username")]
		public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
       // [JsonProperty("email", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Email { get; set; }
    }
}
