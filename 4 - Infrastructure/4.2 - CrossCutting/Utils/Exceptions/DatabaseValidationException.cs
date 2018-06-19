using System;

namespace Moviit.Infrastructure.CrossCutting.Utils.Exceptions
{

    /// <summary>
    /// Represents an exception caused by invalid data sent to the Database or the attempt to execute an invalid operation in the Database.
    /// This validation error occurs in the Entity Framework model level and doesn't reaches the Database level.
    /// </summary>
    public class DatabaseValidationException : BaseException
    {

        /// <summary>
        /// Creates a new instance of the DatabaseValidationException class with the specified parameterized message.
        /// </summary>
        /// <param name="message">The message describing the exception, may be a composite format string</param>
        /// <param name="args">The object(s) to format into the message</param>
        public DatabaseValidationException(string message, params object[] args)
            : base(message, args)
        {
        }

        /// <summary>
        /// Creates a new instance of the DatabaseValidationException class with the specified parameterized message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        /// <param name="message">The message describing the exception, may be a composite format string</param>
        /// <param name="args">The object(s) to format into the message</param>
        public DatabaseValidationException(Exception innerException, string message, params object[] args)
            : base(innerException, message, args)
        {
        }

    }
}