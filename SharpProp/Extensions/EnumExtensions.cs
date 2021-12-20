using System;

namespace SharpProp.Extensions;

internal static class EnumExtensions
{
    /// <summary>
    ///     Gets an attribute on an enum field value.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <typeparam name="T">The type of the attribute you want to retrieve.</typeparam>
    /// <returns>The attribute of type T that exists on the enum value.</returns>
    /// <exception cref="ArgumentNullException">Invalid attribute!</exception>
    internal static T GetAttribute<T>(this Enum value) where T : Attribute
    {
        var type = value.GetType();
        var memberInfo = type.GetMember(value.ToString());
        var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
        return attributes.Length > 0
            ? (T) attributes[0]
            : throw new ArgumentNullException(nameof(T), "Invalid attribute!");
    }
}