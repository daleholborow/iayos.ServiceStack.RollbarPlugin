using System.Diagnostics;
using System.Linq;

namespace iayos.ServiceStack.RollbarPlugin.AdaptedFromRollbar
{
	/// <summary>
    /// Models Rollbar Trace DTO.
    /// </summary>
    /// <seealso cref="DtoBase" />
    public class Trace
        : DtoBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Trace"/> class.
        /// </summary>
        /// <param name="frames">The frames.</param>
        /// <param name="exception">The exception.</param>
        public Trace(Frame[] frames, RollbarExceptionDto exception)
        {
            Assumption.AssertNotNull(frames, nameof(frames));
            Assumption.AssertNotNull(exception, nameof(exception));

            Frames = frames;
            Exception = exception;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trace"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public Trace(System.Exception exception)
        {
            Assumption.AssertNotNull(exception, nameof(exception));

            var frames = new StackTrace(exception, true).GetFrames() ?? new StackFrame[0];

            Frames = frames.Select(frame => new Frame(frame)).ToArray();
            Exception = new RollbarExceptionDto(exception);
        }

        /// <summary>
        /// Gets the frames.
        /// </summary>
        /// <value>
        /// The frames.
        /// </value>
       // [JsonProperty("frames", Required = Required.Always)]
        public Frame[] Frames { get; internal set; }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        //[JsonProperty("exception", Required = Required.Always)]
        public RollbarExceptionDto Exception { get; private set; }
    }
}
