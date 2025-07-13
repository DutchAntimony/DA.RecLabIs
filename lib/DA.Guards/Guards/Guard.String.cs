using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace DA.Guards;

public static partial class Guard
{
    public static class String
    {
        /// <summary>
        /// Ensures that the specified string is not null or empty.
        /// </summary>
        /// <remarks>This method is typically used to validate input parameters in methods. The <paramref
        /// name="parameter"/> and <paramref name="method"/> arguments are automatically populated by the compiler,
        /// providing additional context in the exception message.</remarks>
        /// <param name="value">The string to validate. Must not be null or empty.</param>
        /// <param name="parameter">The name of the parameter being validated. Automatically populated by the compiler.</param>
        /// <param name="method">The name of the calling method. Automatically populated by the compiler.</param>
        /// <returns>The original string if it is not null or empty.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is null or empty. The exception message includes the name of the
        /// parameter and the calling method for easier debugging.</exception>
        public static string NotNullOrWhiteSpace(string? value,
            [CallerArgumentExpression(nameof(value))] string parameter = "",
            [CallerMemberName] string method = "")
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"Invalid value for {parameter} in method {method}. String must not be null or empty.", parameter);
            }
            return value;
        }

        /// <summary>
        /// Ensures that the specified string has a minimum string length.
        /// </summary>
        /// <remarks>This method is typically used to validate input parameters in methods. The <paramref
        /// name="parameter"/> and <paramref name="method"/> arguments are automatically populated by the compiler,
        /// providing additional context in the exception message.</remarks>
        /// <param name="value">The string to validate. Must have a minimum string length.</param>
        /// <param name="minumumLength">The maximum length of the string.</param>
        /// <param name="parameter">The name of the parameter being validated. Automatically populated by the compiler.</param>
        /// <param name="method">The name of the calling method. Automatically populated by the compiler.</param>
        /// <returns>The original string if it is not null or empty.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is too short. The exception message includes the name of the
        /// parameter and the calling method for easier debugging.</exception>
        public static string MinimumLength(string? value, int minimumLength,
            [CallerArgumentExpression(nameof(value))] string parameter = "",
            [CallerMemberName] string method = "")
        {
            value = NotNullOrWhiteSpace(value, parameter, method);
            Number.Positive(minimumLength);
            return value.Length >= minimumLength
                ? value
                : throw new ArgumentException($"Invalid value ({value}) for {parameter} in method {method}. String must have minimum length of {minimumLength}.", parameter);
        }

        /// <summary>
        /// Ensures that the specified string has a maximum string length.
        /// </summary>
        /// <remarks>This method is typically used to validate input parameters in methods. The <paramref
        /// name="parameter"/> and <paramref name="method"/> arguments are automatically populated by the compiler,
        /// providing additional context in the exception message.</remarks>
        /// <param name="value">The string to validate. Must have a maximum string length.</param>
        /// <param name="maximumLength">The maximum length of the string.</param>
        /// <param name="parameter">The name of the parameter being validated. Automatically populated by the compiler.</param>
        /// <param name="method">The name of the calling method. Automatically populated by the compiler.</param>
        /// <returns>The original string if it is not null or empty.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is too long. The exception message includes the name of the
        /// parameter and the calling method for easier debugging.</exception>
        public static string MaximumLength(string? value, int maximumLength,
            [CallerArgumentExpression(nameof(value))] string parameter = "",
            [CallerMemberName] string method = "")
        {
            value = NotNullOrWhiteSpace(value, parameter, method);
            Number.Positive(maximumLength);
            return value.Length <= maximumLength
                ? value
                : throw new ArgumentException($"Invalid value ({value}) for {parameter} in method {method}. String must have maximum length of {maximumLength}.", parameter);
        }

        /// <summary>
        /// Ensures that the specified string has an exact string length.
        /// </summary>
        /// <remarks>This method is typically used to validate input parameters in methods. The <paramref
        /// name="parameter"/> and <paramref name="method"/> arguments are automatically populated by the compiler,
        /// providing additional context in the exception message.</remarks>
        /// <param name="value">The string to validate. Must have an exact string length.</param>
        /// <param name="exactLength">The exact required length of the string.</param>
        /// <param name="parameter">The name of the parameter being validated. Automatically populated by the compiler.</param>
        /// <param name="method">The name of the calling method. Automatically populated by the compiler.</param>
        /// <returns>The original string if it is not null or empty.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not the right length. The exception message includes the name of the
        /// parameter and the calling method for easier debugging.</exception>
        public static string ExactLength(string value, int exactLength,
            [CallerArgumentExpression(nameof(value))] string parameter = "",
            [CallerMemberName] string method = "")
        {
            value = NotNullOrWhiteSpace(value, parameter, method);
            Number.Positive(exactLength);
            return value.Length == exactLength
                ? value
                : throw new ArgumentException($"Invalid value ({value}) for {parameter} in method {method}. String must have exact length of {exactLength}.", parameter);
        }

        /// <summary>
        /// Ensures that the specified string has an exact string length.
        /// </summary>
        /// <remarks>This method is typically used to validate input parameters in methods. The <paramref
        /// name="parameter"/> and <paramref name="method"/> arguments are automatically populated by the compiler,
        /// providing additional context in the exception message.</remarks>
        /// <param name="value">The string to validate. Must have an exact string length.</param>
        /// <param name="parameter">The name of the parameter being validated. Automatically populated by the compiler.</param>
        /// <param name="method">The name of the calling method. Automatically populated by the compiler.</param>
        /// <returns>The original string if it is not null or empty.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not the right length. The exception message includes the name of the
        /// parameter and the calling method for easier debugging.</exception>
        public static string LengthBetween(string value, int min, int max,
            [CallerArgumentExpression(nameof(value))] string parameter = "",
            [CallerMemberName] string method = "")
        {
            value = NotNullOrWhiteSpace(value, parameter, method);
            Number.Positive(min);
            Number.Positive(max);
            Number.SmallerThan(min, max);
            MinimumLength(value, min, parameter, method);
            return MaximumLength(value, max, parameter, method);
        }

        /// <summary>
        /// Validates that a specified string matches a given regex.
        /// </summary>
        /// <remarks>This method ensures that the input string matches the specified regex. If the
        /// validation fails,  an <see cref="ArgumentException"/> is thrown, providing detailed information about the
        /// invalid input.</remarks>
        /// <param name="value">The string to check whether the value is valid. Cannot be null or empty.</param>
        /// <param name="pattern">The regex to search for within <paramref name="value"/>. Cannot be null or empty.</param>
        /// <param name="parameter">The name of the parameter being validated. Automatically provided by the compiler.</param>
        /// <param name="method">The name of the calling method. Automatically provided by the compiler.</param>
        /// <returns>The original <paramref name="value"/> if it matches the <paramref name="pattern"/>.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> does not match <paramref name="pattern"/>.</exception>
        public static string MatchesRegex(string value, string pattern,
            [CallerArgumentExpression(nameof(value))] string parameter = "",
            [CallerMemberName] string method = "")
        {
            value = NotNullOrWhiteSpace(value, parameter, method);
            NotNullOrWhiteSpace(pattern, nameof(pattern), method);
            return Regex.IsMatch(value, pattern)
                ? value
                : throw new ArgumentException($"Invalid value ({value}) for {parameter} in method {method}. String must match '{pattern}'.", parameter);
        }
    }
}
