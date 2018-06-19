using System;

namespace Moviit.Infrastructure.CrossCutting.Utils.Exceptions
{

    /// <summary>
    /// Represents an exception caused by a failed attempt to connect to the Database.
    /// </summary>
    public class DatabaseConnectionException : BaseException
    {

        /// <summary>
        /// Creates a new instance of the DatabaseConnectionException class with the specified parameterized message.
        /// </summary>
        /// <param name="message">The message describing the exception, may be a composite format string</param>
        /// <param name="args">The object(s) to format into the message</param>
        public DatabaseConnectionException(string message, params object[] args)
            : base(message, args)
        {
        }

        /// <summary>
        /// Creates a new instance of the DatabaseConnectionException class with the specified parameterized message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        /// <param name="message">The message describing the exception, may be a composite format string</param>
        /// <param name="args">The object(s) to format into the message</param>
        public DatabaseConnectionException(Exception innerException, string message, params object[] args)
            : base(innerException, message, args)
        {
        }

    }
}
