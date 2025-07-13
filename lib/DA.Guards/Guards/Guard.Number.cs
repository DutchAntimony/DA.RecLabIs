using System.Numerics;
using System.Runtime.CompilerServices;

namespace DA.Guards;
public static partial class Guard
{
    public static class Number
    {
        /// <summary>
        /// Ensure that a given number has a value greater than the comparison value.
        /// </summary>
        /// <typeparam name="TNumber">The type of the provided number.</typeparam>
        /// <param name="value">The value to ensure.</param>
        /// <param name="comparison">The value to compare to.</param>
        /// <param name="parameter">Automatically filled; the name of the provided value.</param>
        /// <param name="method">Automatically filled; the name of the method calling this value.</param>
        /// <returns>Fluently the provided value, if value is valid.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if ensure does not succeed.</exception>
        public static TNumber GreaterThan<TNumber>(TNumber value, TNumber comparison,
        [CallerArgumentExpression(nameof(value))] string parameter = "",
        [CallerMemberName] string method = "")
            where TNumber : INumber<TNumber> =>
            value > comparison
                ? value
                : throw new ArgumentOutOfRangeException(parameter, value, $"Invalid value {value} for {parameter} in method {method}. Value must be greater than {comparison}.");

        /// <summary>
        /// Ensure that a given number has a value greater than or equal to the comparison value.
        /// </summary>
        /// <typeparam name="TNumber">The type of the provided number.</typeparam>
        /// <param name="value">The value to ensure.</param>
        /// <param name="comparison">The value to compare to.</param>
        /// <param name="parameter">Automatically filled; the name of the provided value.</param>
        /// <param name="method">Automatically filled; the name of the method calling this value.</param>
        /// <returns>Fluently the provided value, if value is valid.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if ensure does not succeed.</exception>
        public static TNumber GreaterThanOrEqualTo<TNumber>(TNumber value, TNumber comparison,
        [CallerArgumentExpression(nameof(value))] string parameter = "",
        [CallerMemberName] string method = "")
            where TNumber : INumber<TNumber> =>
            value >= comparison
                ? value
                : throw new ArgumentOutOfRangeException(parameter, value, $"Invalid value {value} for {parameter} in method {method}. Value must be greater than or equal to {comparison}.");

        /// <summary>
        /// Ensure that a given number has a value smaller than the comparison value.
        /// </summary>
        /// <typeparam name="TNumber">The type of the provided number.</typeparam>
        /// <param name="value">The value to ensure.</param>
        /// <param name="comparison">The value to compare to.</param>
        /// <param name="parameter">Automatically filled; the name of the provided value.</param>
        /// <param name="method">Automatically filled; the name of the method calling this value.</param>
        /// <returns>Fluently the provided value, if value is valid.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if ensure does not succeed.</exception>
        public static TNumber SmallerThan<TNumber>(TNumber value, TNumber comparison,
        [CallerArgumentExpression(nameof(value))] string parameter = "",
        [CallerMemberName] string method = "")
            where TNumber : INumber<TNumber> =>
            value < comparison
                ? value
                : throw new ArgumentOutOfRangeException(parameter, value, $"Invalid value {value} for {parameter} in method {method}. Value must be smaller than {comparison}.");

        /// <summary>
        /// Ensure that a given number has a value smaller than or equal to the comparison value.
        /// </summary>
        /// <typeparam name="TNumber">The type of the provided number.</typeparam>
        /// <param name="value">The value to ensure.</param>
        /// <param name="comparison">The value to compare to.</param>
        /// <param name="parameter">Automatically filled; the name of the provided value.</param>
        /// <param name="method">Automatically filled; the name of the method calling this value.</param>
        /// <returns>Fluently the provided value, if value is valid.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if ensure does not succeed.</exception>
        public static TNumber SmallerThanOrEqualTo<TNumber>(TNumber value, TNumber comparison,
        [CallerArgumentExpression(nameof(value))] string parameter = "",
        [CallerMemberName] string method = "")
            where TNumber : INumber<TNumber> =>
            value <= comparison
                ? value
                : throw new ArgumentOutOfRangeException(parameter, value, $"Invalid value {value} for {parameter} in method {method}. Value must be smaller than or equal to {comparison}.");

        /// <summary>
        /// Ensure that a given number has a value that is in range of the provided min and max values.
        /// </summary>
        /// <typeparam name="TNumber">The type of the provided number.</typeparam>
        /// <param name="value">The value to ensure.</param>
        /// <param name="min">The minimum value of the range.</param>
        /// <param name="max">The maximum value to the range.</param>
        /// <param name="parameter">Automatically filled; the name of the provided value.</param>
        /// <param name="method">Automatically filled; the name of the method calling this value.</param>
        /// <returns>Fluently the provided value, if value is valid.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if ensure does not succeed.</exception>
        public static TNumber IsBetween<TNumber>(TNumber value, TNumber min, TNumber max,
        [CallerArgumentExpression(nameof(value))] string parameter = "",
        [CallerMemberName] string method = "")
            where TNumber : INumber<TNumber> =>
            SmallerThanOrEqualTo(GreaterThanOrEqualTo(value, min, parameter, method), max, parameter, method);

        /// <summary>
        /// Ensure that a given number has a positive value.
        /// </summary>
        /// <typeparam name="TNumber">The type of the provided number.</typeparam>
        /// <param name="value">The value to ensure.</param>
        /// <param name="parameter">Automatically filled; the name of the provided value.</param>
        /// <param name="method">Automatically filled; the name of the method calling this value.</param>
        /// <returns>Fluently the provided value, if value is valid.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if ensure does not succeed.</exception>
        public static TNumber Positive<TNumber>(TNumber value,
        [CallerArgumentExpression(nameof(value))] string parameter = "",
        [CallerMemberName] string method = "")
            where TNumber : INumber<TNumber> =>
            GreaterThan(value, TNumber.Zero, parameter, method);

        /// <summary>
        /// Ensure that a given number has a value greater then or equal to 0.
        /// </summary>
        /// <typeparam name="TNumber">The type of the provided number.</typeparam>
        /// <param name="value">The value to ensure.</param>
        /// <param name="parameter">Automatically filled; the name of the provided value.</param>
        /// <param name="method">Automatically filled; the name of the method calling this value.</param>
        /// <returns>Fluently the provided value, if value is valid.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if ensure does not succeed.</exception>
        public static TNumber NotNegative<TNumber>(TNumber value,
        [CallerArgumentExpression(nameof(value))] string parameter = "",
        [CallerMemberName] string method = "")
            where TNumber : INumber<TNumber> =>
            GreaterThanOrEqualTo(value, TNumber.Zero, parameter, method);
    }
}
