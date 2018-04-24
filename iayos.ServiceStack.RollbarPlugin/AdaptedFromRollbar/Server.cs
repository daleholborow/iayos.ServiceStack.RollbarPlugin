using System.Collections.Generic;

namespace iayos.ServiceStack.RollbarPlugin.AdaptedFromRollbar
{
	/// <summary>
	///     Models Rollbar Server DTO.
	/// </summary>
	/// <seealso cref="ExtendableDtoBase" />
	public class Server
		: ExtendableDtoBase
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="Server" /> class.
		/// </summary>
		public Server()
			: this(null)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Server" /> class.
		/// </summary>
		/// <param name="arbitraryKeyValuePairs">The arbitrary key value pairs.</param>
		public Server(IDictionary<string, object> arbitraryKeyValuePairs)
			: base(arbitraryKeyValuePairs)
		{
		}

		/// <summary>
		///     The DTO reserved properties.
		/// </summary>
		public static class ReservedProperties
		{
			/// <summary>
			///     The host
			/// </summary>
			public const string Host = "host";

			/// <summary>
			///     The root
			/// </summary>
			public const string Root = "root";

			/// <summary>
			///     The branch
			/// </summary>
			public const string Branch = "branch";

			/// <summary>
			///     The code version
			/// </summary>
			public const string CodeVersion = "code_version";
		}

		/// <summary>
		///     Gets or sets the host.
		/// </summary>
		/// <value>
		///     The host.
		/// </value>
		public string Host
		{
			get => _keyedValues[ReservedProperties.Host] as string;
			set => _keyedValues[ReservedProperties.Host] = value;
		}

		/// <summary>
		///     Gets or sets the root.
		/// </summary>
		/// <value>
		///     The root.
		/// </value>
		public string Root
		{
			get => _keyedValues[ReservedProperties.Root] as string;
			set => _keyedValues[ReservedProperties.Root] = value;
		}

		/// <summary>
		///     Gets or sets the branch.
		/// </summary>
		/// <value>
		///     The branch.
		/// </value>
		public string Branch
		{
			get => _keyedValues[ReservedProperties.Branch] as string;
			set => _keyedValues[ReservedProperties.Branch] = value;
		}

		/// <summary>
		///     Gets or sets the code version.
		/// </summary>
		/// <value>
		///     The code version.
		/// </value>
		public string CodeVersion
		{
			get => _keyedValues[ReservedProperties.CodeVersion] as string;
			set => _keyedValues[ReservedProperties.CodeVersion] = value;
		}
	}
}