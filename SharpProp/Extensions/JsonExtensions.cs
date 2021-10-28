using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnitsNet.Serialization.JsonNet;

namespace SharpProp.Extensions
{
    public static class JsonExtensions
    {
        private static readonly JsonSerializerSettings Settings = new()
            {Converters = new List<JsonConverter> {new StringEnumConverter(), new UnitsNetIQuantityJsonConverter()}};

        /// <summary>
        ///     Converts a <see cref="Fluid" /> instance to a JSON string.
        /// </summary>
        /// <param name="instance">A <see cref="Fluid" /> instance.</param>
        /// <returns>A JSON string.</returns>
        public static string AsJson(this Fluid instance) => instance.AsJson<Fluid>();
        
        /// <summary>
        ///     Converts a <see cref="Mixture" /> instance to a JSON string.
        /// </summary>
        /// <param name="instance">A <see cref="Mixture" /> instance.</param>
        /// <returns>A JSON string.</returns>
        public static string AsJson(this Mixture instance) => instance.AsJson<Mixture>();
        
        /// <summary>
        ///     Converts a <see cref="HumidAir" /> instance to a JSON string.
        /// </summary>
        /// <param name="instance">A <see cref="HumidAir" /> instance.</param>
        /// <returns>A JSON string.</returns>
        public static string AsJson(this HumidAir instance) => instance.AsJson<HumidAir>();
        
        private static string AsJson<T>(this T instance, bool indented = true)
        {
            Settings.Formatting = indented ? Formatting.Indented : Formatting.None;
            return JsonConvert.SerializeObject(instance, Settings);
        }
    }
}