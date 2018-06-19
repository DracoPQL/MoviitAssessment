using System;

namespace Moviit.Infrastructure.CrossCutting.Utils.Exceptions
{

    /// <summary>
    /// Represents an exception caused by an attempt to map an arbitrary value to an Enum value.
    /// </summary>
    public class InvalidEnumValueException : BaseException
    {

        /// <summary>
        /// Creates a new instance of the InvalidEnumValueException class with a default message.
        /// </summary>
        public InvalidEnumValueException()
            : base("The input enumeration value could not be resolved.")
        {
        }

        /// <summary>
        /// Creates a new instance of the InvalidEnumValueException class with a default message, including the value passed as input.
        /// </summary>
        /// <param name="enumType">The target enum type</param>
        /// <param name="value">The invalid enum value</param>
        public InvalidEnumValueException(Type enumType, object value)
            : base("The input enumeration value [{0}] could not be resolved into a valid value of the [{1}] enum.", value, enumType)
        {
        }

        /// <summary>
        /// Creates a new instance of the InvalidEnumValueException class with the specified parameterized message.
        /// </summary>
        /// <param name="message">The message describing the exception, may be a composite format string</param>
        /// <param name="args">The object(s) to format into the message</param>
        public InvalidEnumValueException(string message, params object[] args)
            : base(message, args)
        {
        }

        /// <summary>
        /// Creates a new instance of the InvalidEnumValueException class with the specified parameterized message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        /// <param name="message">The message describing the exception, may be a composite format string</param>
        /// <param name="args">The object(s) to format into the message</param>
        public InvalidEnumValueException(Exception innerException, string message, params object[] args)
            : base(innerException, message, args)
        {
        }

        /// <summary>
        /// Creates a new instance of the InvalidEnumValueException class with the specified parameterized message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        /// <param name="enumType">The target enum type</param>
        /// <param name="value">The invalid enum value</param>
        public InvalidEnumValueException(Exception innerException, Type enumType, object value)
            : base(innerException, "The input enumeration value [{0}] could not be resolved into a valid value of the [{1}] enum.", value, enumType)
        {
        }

    }
}
