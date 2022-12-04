using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnitsNet.Serialization.JsonNet;

namespace SharpProp;

public static class JsonExtensions
{
    private static readonly JsonSerializerSettings Settings = new()
    {
        Converters = new List<JsonConverter>
            {new StringEnumConverter(), new UnitsNetIQuantityJsonConverter()}
    };

    /// <summary>
    ///     Converts the fluid instance to a JSON string.
    /// </summary>
    /// <param name="instance">The fluid instance.</param>
    /// <param name="indented"><c>true</c> if indented.</param>
    /// <returns>A JSON string.</returns>
    public static string AsJson(this Fluid instance, bool indented = true) =>
        instance.AsJson<Fluid>(indented);

    /// <summary>
    ///     Converts the mixture instance to a JSON string.
    /// </summary>
    /// <param name="instance">The mixture instance.</param>
    /// <param name="indented"><c>true</c> if indented.</param>
    /// <returns>A JSON string.</returns>
    public static string AsJson(this Mixture instance, bool indented = true) =>
        instance.AsJson<Mixture>(indented);

    /// <summary>
    ///     Converts the humid air instance to a JSON string.
    /// </summary>
    /// <param name="instance">The humid air instance.</param>
    /// <param name="indented"><c>true</c> if indented.</param>
    /// <returns>A JSON string.</returns>
    public static string AsJson(this HumidAir instance, bool indented = true) =>
        instance.AsJson<HumidAir>(indented);

    private static string AsJson<T>(this T instance, bool indented = true)
    {
        Settings.Formatting = indented ? Formatting.Indented : Formatting.None;
        return JsonConvert.SerializeObject(instance, Settings);
    }
}