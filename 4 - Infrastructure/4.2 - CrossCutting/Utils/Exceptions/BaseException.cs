using System;
using System.Text;

namespace Moviit.Infrastructure.CrossCutting.Utils.Exceptions
{

    /// <summary>
    /// Base Exception class.
    /// </summary>
    public class BaseException : Exception
    {

        #region Variables

        /// <summary>
        /// Internal message to use on this base class and the derived ones
        /// </summary>
        protected string _message;

        #endregion

        #region Properties

        /// <summary>
        /// Message of the inner exceptions
        /// </summary>
        public string InnerExceptionMessages { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the BaseException class with a specified parameterized message.
        /// </summary>
        /// <param name="message">The message describing the exception, may be a composite format string</param>
        /// <param name="args">The object(s) to format into the message</param>
        public BaseException(string message, params object[] args)
            : base(string.Format(message, args))
        {
            //Build the exception message and save the arguments in the exception data for later use
            _message = string.Format(message, args);
            for (int i = 0; i < args.Length; i++)
            {
                Data.Add(i, args[i]);
            }
        }

        /// <summary>
        /// Creates a new instance of the BaseException class with a specified parameterized message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        /// <param name="message">The message describing the exception, may be a composite format string</param>
        /// <param name="args">The object(s) to format into the message</param>
        public BaseException(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            //Build the exception message and save the arguments in the exception data for later use
            _message = string.Format(message, args);
            for (int i = 0; i < args.Length; i++)
            {
                Data.Add(i, args[i]);
            }

            SetInnerExceptionsMessages(innerException);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets all the inner exceptions inside the exception received and returns a formatted string with them
        /// </summary>
        /// <param name="ex">Exception to fetch all the inner exceptions from</param>
        /// <param name="prefix">String to put before the string built with the inner exceptions</param>
        /// <returns>A string with all the exceptions inside the exception received</returns>
        internal static string FlattenInnerExceptions(Exception ex, string prefix = "")
        {
            StringBuilder sb = new StringBuilder(prefix);

            while (ex != null)
            {
                sb.AppendLine();
                sb.Append(ex.GetType().ToString());
                sb.Append(": ");
                sb.AppendLine(ex.Message);
                sb.AppendLine("Stack Trace:");
                sb.AppendLine(ex.StackTrace);

                ex = ex.InnerException;
            }

            return sb.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Set all the inner exceptions inside the exception received inside the InnerExceptionMessages property
        /// </summary>
        /// <param name="innerException">Exception to fetch all the inner exceptions from</param>
        private void SetInnerExceptionsMessages(Exception innerException)
        {
            if (innerException == null)
                return;

            InnerExceptionMessages += FlattenInnerExceptions(innerException, "InnerExceptions ->");
        }

        #endregion

    }
}
