using fastJSON;

namespace Moviit.Infrastructure.CrossCutting.Utils.Extensions
{

    /// <summary>
    /// Class to group the extension methods for the object class but containing only the string class for using JSON serialization.
    /// </summary>
    public static class JSONExtension
    {

        /// <summary>
        /// Serializes an object into its JSON string representation.
        /// </summary>
        /// <param name="obj">The object to be serialized</param>
        /// <returns>A JSON string representation of the object</returns>
        public static string ToJSON(this object obj)
        {
            return JSON.ToJSON(obj);
        }

        /// <summary>
        /// Deserializes a JSON-formatted string into an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str">The string to be deserialized</param>
        /// <returns>The object obtained from the deserialization process</returns>
        public static T FromJSON<T>(this string str)
        {
            return JSON.ToObject<T>(str);
        }

    }
}
