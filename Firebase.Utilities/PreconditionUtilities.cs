using System;

namespace Firebase
{
    /// <summary>
    /// Helper functions to help write less boilerplate.
    /// </summary>
    public static class PreconditionUtilities
    {
        /// <summary>
        /// Checks if a string is empty or null and throws the corrisponding exception.
        /// </summary>
        /// <param name="argument">The string to check.</param>
        /// <param name="paramName">The name of the string argument to use in throwing exceptions.</param>
        /// <returns>The string if it's not null or empty.</returns>
        public static string CheckNotNullOrEmpty(string argument, string paramName)
        {
            if(argument == null)
            {
                throw new ArgumentNullException(paramName);
            }
            if(string.IsNullOrEmpty(argument))
            {
                throw new ArgumentException("An empty string was provided, but that's not allowed.", paramName);
            }
            return argument;
        }
        /// <summary>
        /// Checks if an argument meets a certain condition, throws an exception if it doesn't.
        /// </summary>
        /// <param name="condition">The condition validator function.</param>
        /// <param name="paramName">The argument name.</param>
        /// <param name="message">The message to send in the exception.</param>
        public static void CheckArgument(bool condition, string paramName, string message)
        {
            if(!condition)
            {
                throw new ArgumentException(message, paramName);
            }
        }
        /// <summary>
        /// Checks if an argument is between certain values, if its not throws an exception, otherwise returns the argument value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argument">The argument to check.</param>
        /// <param name="paramName">The argument name.</param>
        /// <param name="minInclusive">The minimum value to check against(inclusive).</param>
        /// <param name="maxInclusive">The maximum value to check against(inclusive).</param>
        /// <returns></returns>
        public static T CheckArgumentRange<T>(T argument, string paramName, T minInclusive, T maxInclusive)
            where T : IComparable<T>
        {
            if(argument.CompareTo(minInclusive) < 0  || argument.CompareTo(maxInclusive) > 0)
            {
                throw new ArgumentOutOfRangeException(paramName, $"Value {argument} must be between [{minInclusive}, {maxInclusive}]");
            }
            return argument;
        }

        /// <summary>
        /// Checks if an argument is null and throws an except if so.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argument">The argument to check if its null.</param>
        /// <param name="paramName">The argument name to use when throwing a null exception.</param>
        /// <returns>The argument if it wasn't null.</returns>
        public static T CheckNotNull<T>(T argument, string paramName)
            where T:class
        {
            T t = argument;
            if(t == null)
            {
                throw new ArgumentNullException(paramName);
            }
            return t;
        }
    }
}
