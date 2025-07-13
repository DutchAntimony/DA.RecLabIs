using System.Runtime.CompilerServices;

namespace DA.Guards;

public static partial class Guard
{
    public static class Guid
    {
        /// <summary>
        /// Ensure that a given Guid is not empty.
        /// </summary>
        /// <param name="value">The value to ensure is not empty.</param>
        /// <param name="parameter">Automatically filled; the name of the provided value.</param>
        /// <param name="method">Automatically filled; the name of the method calling this value.</param>
        /// <returns>Fluently the provided value, if value is valid.</returns>
        /// <exception cref="ArgumentException">Thrown if ensure does not succeed.</exception>
        public static System.Guid NotEmpty(System.Guid value,
            [CallerArgumentExpression(nameof(value))] string parameter = "",
            [CallerMemberName] string method = "") =>
            value != System.Guid.Empty
                ? value
                : throw new ArgumentException($"Invalid value {value} for {parameter} in method {method}. Guid must not be empty.", parameter);

        /// <summary>
        /// Ensure that a given Guid is a version 7 Guid.
        /// </summary>
        /// <param name="value">The value to ensure.</param>
        /// <param name="parameter">Automatically filled; the name of the provided value.</param>
        /// <param name="method">Automatically filled; the name of the method calling this value.</param>
        /// <returns>Fluently the provided value, if value is valid.</returns>
        /// <exception cref="ArgumentException">Thrown if ensure does not succeed.</exception>
        public static System.Guid Version7(System.Guid value,
            [CallerArgumentExpression(nameof(value))] string parameter = "",
            [CallerMemberName] string method = "")
        {
            NotEmpty(value, parameter, method);
            return value.Version == 7
                ? value
                : throw new ArgumentException($"Invalid value {value} for {parameter} in method {method}. Guid must be a version 7 GUID.", parameter);
        }
    }
}
