using Moviit.Infrastructure.CrossCutting.Utils.Exceptions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Moviit.Infrastructure.CrossCutting.Utils.Extensions
{

    /// <summary>
    /// Class to group the extension methods for the Exception class.
    /// </summary>
    public static class ExceptionExtension
    {

        /// <summary>
        /// Obtains error messages and stack trace from an exception, including any inner exceptions contained in an AggregateException. 
        /// </summary>
        /// <param name="exception">The exception to obtain the messages and stack trace from.</param>
        /// <returns>A string containing the message(s) and stack trace(s) within the exception.</returns>
        public static string GetInnerExceptionsDetail(this Exception exception)
        {
            StringBuilder sb = new StringBuilder();

            if (exception is AggregateException)
            {
                //If the exception is an aggregate exception then get the inner exceptions and flat each one of them and append them to the response
                ReadOnlyCollection<Exception> exceptions = (exception as AggregateException).Flatten().InnerExceptions;
                sb.Append("AggregateException InnerExceptions ->");

                if (exceptions != null && exceptions.Any())
                {
                    for (int i = 0; i < exceptions.Count; i++)
                    {
                        sb.AppendLine();
                        sb.Append("Exception ");
                        sb.Append(i + 1);
                        sb.AppendLine();

                        sb.Append(BaseException.FlattenInnerExceptions(exceptions[i]));
                        sb.AppendLine();
                    }
                }
                else
                {
                    sb.AppendLine("AggregateException did not contain any inner exceptions");
                }
            }
            else
            {
                //If the exception is not an aggregate one then just flat it and append it to the response
                sb.Append(BaseException.FlattenInnerExceptions(exception, "InnerExceptions ->"));
            }

            return sb.ToString();
        }

    }
}
