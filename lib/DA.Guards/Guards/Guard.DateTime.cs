using System.Runtime.CompilerServices;

namespace DA.Guards;
public static partial class Guard
{
    public static partial class Date
    {
        /// <summary>
        /// Ensure that a given DateTime is after a specified date.
        /// </summary>
        /// <param name="value">The value to ensure is after the comparison.</param>
        /// <param name="comparison">The Date to compare with.</param>
        /// <param name="parameter">Automatically filled; the name of the provided value.</param>
        /// <param name="method">Automatically filled; the name of the method calling this value.</param>
        /// <returns>Fluently the provided value, if value is valid.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if ensure does not succeed.</exception>
        public static DateTime After(DateTime value, DateTime comparison,
            [CallerArgumentExpression(nameof(value))] string parameter = "",
            [CallerMemberName] string method = "") =>
            value >= comparison
                ? value
                : throw new ArgumentOutOfRangeException(parameter, value, $"Invalid value {value:d} for {parameter} in method {method}. Date must be after {comparison:d}.");

        /// <summary>
        /// Ensure that a given DateTime is after a specified date.
        /// </summary>
        /// <param name="value">The value to ensure is after the comparison.</param>
        /// <param name="comparison">The Date to compare with.</param>
        /// <param name="parameter">Automatically filled; the name of the provided value.</param>
        /// <param name="method">Automatically filled; the name of the method calling this value.</param>
        /// <returns>Fluently the provided value, if value is valid.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if ensure does not succeed.</exception>
        public static DateTime After(DateTime value, DateOnly comparison,
            [CallerArgumentExpression(nameof(value))] string parameter = "",
            [CallerMemberName] string method = "") =>
            After(value, comparison.ToDateTime(TimeOnly.MinValue), parameter, method);

        /// <summary>
        /// Ensure that a given DateTime is before a specified date.
        /// </summary>
        /// <param name="value">The value to ensure is before the comparison.</param>
        /// <param name="comparison">The Date to compare with.</param>
        /// <param name="parameter">Automatically filled; the name of the provided value.</param>
        /// <param name="method">Automatically filled; the name of the method calling this value.</param>
        /// <returns>Fluently the provided value, if value is valid.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if ensure does not succeed.</exception>
        public static DateTime Before(DateTime value, DateTime comparison,
            [CallerArgumentExpression(nameof(value))] string parameter = "",
            [CallerMemberName] string method = "") =>
            value < comparison
                ? value
                : throw new ArgumentOutOfRangeException(parameter, value, $"Invalid value {value:d} for {parameter} in method {method}. Date must be before {comparison:d}.");

        /// <summary>
        /// Ensure that a given DateTime is before a specified date.
        /// </summary>
        /// <param name="value">The value to ensure is before the comparison.</param>
        /// <param name="comparison">The Date to compare with.</param>
        /// <param name="parameter">Automatically filled; the name of the provided value.</param>
        /// <param name="method">Automatically filled; the name of the method calling this value.</param>
        /// <returns>Fluently the provided value, if value is valid.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if ensure does not succeed.</exception>
        public static DateTime Before(DateTime value, DateOnly comparison,
            [CallerArgumentExpression(nameof(value))] string parameter = "",
            [CallerMemberName] string method = "") =>
            Before(value, comparison.ToDateTime(TimeOnly.MinValue), parameter, method);

        /// <summary>
        /// Ensure that a given DateTime in within a given range.
        /// </summary>
        /// <param name="value">The value to ensure is in range.</param>
        /// <param name="after">The date the value should be after.</param>
        /// <param name="before">The date the value should be before.</param>
        /// <param name="parameter">Automatically filled; the name of the provided value.</param>
        /// <param name="method">Automatically filled; the name of the method calling this value.</param>
        /// <returns>Fluently the provided value, if value is valid.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if ensure does not succeed.</exception>
        public static DateTime InRange(DateTime value, DateTime after, DateTime before, 
            [CallerArgumentExpression(nameof(value))] string parameter = "",
            [CallerMemberName] string method = "") =>
            After(Before(value, before, parameter, method),after, parameter, method);

        /// <summary>
        /// Ensure that a given DateTime in within a given range.
        /// </summary>
        /// <param name="value">The value to ensure is in range.</param>
        /// <param name="after">The date the value should be after.</param>
        /// <param name="before">The date the value should be before.</param>
        /// <param name="parameter">Automatically filled; the name of the provided value.</param>
        /// <param name="method">Automatically filled; the name of the method calling this value.</param>
        /// <returns>Fluently the provided value, if value is valid.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if ensure does not succeed.</exception>
        public static DateTime InRange(DateTime value, DateOnly after, DateOnly before,
            [CallerArgumentExpression(nameof(value))] string parameter = "",
            [CallerMemberName] string method = "") =>
            After(Before(value, before, parameter, method), after, parameter, method);
    }
}