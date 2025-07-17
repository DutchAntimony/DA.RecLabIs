namespace DA.Messaging.Extensions;

internal static class TypeExtensions
{
    /// <summary>
    /// Display a type name in a human-readable format.
    /// Generic types will be displayed with their type arguments.
    /// </summary>
    public static string PrettyName(this Type type)
    {
        if (!type.IsGenericType)
            return type.Name;

        var genericTypeName = type.GetGenericTypeDefinition().Name;
        genericTypeName = genericTypeName[..genericTypeName.IndexOf('`')];

        var genericArgs = string.Join(", ", type.GetGenericArguments().Select(PrettyName));
        return $"{genericTypeName}<{genericArgs}>";
    }
}