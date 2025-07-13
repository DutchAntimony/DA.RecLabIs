using System.Runtime.CompilerServices;

namespace DA.Guards;

public static partial class Guard
{
    public static partial class That
    {
        /// <summary>
        /// Ensure that a given value is not null.
        /// </summary>
        /// <typeparam name="T">The type of the provided value.</typeparam>
        /// <param name="value">The value to ensure is not null.</param>
        /// <param name="parameter">Automatically filled; the name of the provided value.</param>
        /// <param name="method">Automatically filled; the name of the method calling this value.</param>
        /// <returns>Fluently the provided value, if value is valid.</returns>
        /// <exception cref="ArgumentNullException">Thrown if ensure does not succeed.</exception>
        public static T NotNull<T>(T? value,
            [CallerArgumentExpression(nameof(value))] string parameter = "",
            [CallerMemberName] string method = "")
                where T: class =>
            value ?? throw new ArgumentNullException(parameter, $"Invalid value {value} for {parameter} in method {method}. Value must not be null.");

        /// <summary>
        /// Ensure that a given value is not null.
        /// </summary>
        /// <typeparam name="T">The type of the provided value.</typeparam>
        /// <param name="value">The value to ensure is not null.</param>
        /// <param name="parameter">Automatically filled; the name of the provided value.</param>
        /// <param name="method">Automatically filled; the name of the method calling this value.</param>
        /// <returns>Fluently the provided value, if value is valid.</returns>
        /// <exception cref="ArgumentNullException">Thrown if ensure does not succeed.</exception>
        public static T NotNull<T>(T? value,
            [CallerArgumentExpression(nameof(value))] string parameter = "",
            [CallerMemberName] string method = "")
                where T: struct =>
            value ?? throw new ArgumentNullException(parameter, $"Invalid value {value} for {parameter} in method {method}. Value must not be null.");

        /// <summary>
        /// Ensure that a given value is not null.
        /// </summary>
        /// <typeparam name="T">The type of the provided value.</typeparam>
        /// <param name="value">The value to ensure is not null.</param>
        /// <param name="parameter">Automatically filled; the name of the provided value.</param>
        /// <param name="method">Automatically filled; the name of the method calling this value.</param>
        /// <returns>Fluently the provided value, if value is valid.</returns>
        /// <exception cref="ArgumentNullException">Thrown if ensure does not succeed.</exception>
        public static T NotDefault<T>(T value,
            [CallerArgumentExpression(nameof(value))] string parameter = "",
            [CallerMemberName] string method = "")
                where T: struct => 
            !Equals(value, default(T)) 
                ? value
                : throw new ArgumentNullException(parameter, $"Invalid value {value} for {parameter} in method {method}. Value must not be null.");

        /// <summary>
        /// Ensure that a predicate on a given object is true.
        /// </summary>
        /// <param name="value">The value to ensure.</param>
        /// <param name="predicate">The function that must be true.</param>
        /// <param name="message">The message in the exception if the value is invalid.</param>
        /// <param name="parameter">Automatically filled; the name of the provided value.</param>
        /// <param name="method">Automatically filled; the name of the method calling this value.</param>
        /// <returns>Fluently the provided value, if value is valid.</returns>
        /// <exception cref="ArgumentException">Thrown if ensure does not succeed.</exception>
        public static T True<T>(T value, Func<T, bool> predicate,
            [CallerArgumentExpression(nameof(value))] string parameter = "",
            [CallerMemberName] string method = "") =>
            predicate(value)
                ? value
                : throw new ArgumentException($"Invalid value {value} for {parameter} in method {method}. Value must not match predicate.", parameter);

        /// <summary>
        /// Ensure that a predicate on a given object is true.
        /// </summary>
        /// <param name="value">The value to ensure.</param>
        /// <param name="predicate">The function that must be true.</param>
        /// <param name="message">The message in the exception if the value is invalid.</param>
        /// <param name="parameter">Automatically filled; the name of the provided value.</param>
        /// <param name="method">Automatically filled; the name of the method calling this value.</param>
        /// <returns>Fluently the provided value, if value is valid.</returns>
        /// <exception cref="ArgumentException">Thrown if ensure does not succeed.</exception>
        public static async Task<T> TrueAsync<T>(T value, Func<T, Task<bool>> predicate,
            [CallerArgumentExpression(nameof(value))] string parameter = "",
            [CallerMemberName] string method = "") =>
            await predicate(value)
                ? value
                : throw new ArgumentException($"Invalid value {value} for {parameter} in method {method}. Value must not match predicate.", parameter);
    }
}
