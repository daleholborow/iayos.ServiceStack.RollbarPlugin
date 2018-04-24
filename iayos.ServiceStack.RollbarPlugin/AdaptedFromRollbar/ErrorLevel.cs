namespace iayos.ServiceStack.RollbarPlugin.AdaptedFromRollbar
{
	/// <summary>
	/// Lists all the supported Rollbar error levels.
	/// </summary>
	public enum ErrorLevel
	{
		/// <summary>
		/// The critical error/log level.
		/// </summary>
		Critical,

		/// <summary>
		/// The error log level.
		/// </summary>
		Error,

		/// <summary>
		/// The warning log level.
		/// </summary>
		Warning,

		/// <summary>
		/// The informational log level.
		/// </summary>
		Info,

		/// <summary>
		/// The debug log level.
		/// </summary>
		Debug,
	}
}