using System;

namespace Moviit.Infrastructure.CrossCutting.Utils.Exceptions
{

    /// <summary>
    /// Represents an exception created after the translation of the message of an original exception.
    /// </summary>
    public class TranslatedException : BaseException
    {

        /// <summary>
        /// Creates a new instance of the TranslatedException class with the specified parameterized message.
        /// </summary>
        /// <param name="message">The message describing the exception, may be a composite format string</param>
        /// <param name="args">The object(s) to format into the message</param>
        public TranslatedException(string message, params object[] args)
            : base(message, args)
        {
        }

        /// <summary>
        /// Creates a new instance of the TranslatedException class with the specified parameterized message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        /// <param name="message">The message describing the exception, may be a composite format string</param>
        /// <param name="args">The object(s) to format into the message</param>
        public TranslatedException(Exception innerException, string message, params object[] args)
            : base(innerException, message, args)
        {
        }

    }
}
