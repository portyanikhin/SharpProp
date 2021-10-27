using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnitsNet.Serialization.JsonNet;

namespace SharpProp.Outputs
{
    /// <summary>
    ///     An object that can be converted to a JSON string.
    /// </summary>
    public abstract class Jsonable
    {
        private readonly JsonSerializerSettings _settings = new()
            {Converters = new List<JsonConverter> {new StringEnumConverter(), new UnitsNetIQuantityJsonConverter()}};

        /// <summary>
        ///     Converts object to a JSON string.
        /// </summary>
        /// <param name="indented">Adding indents.</param>
        /// <returns>JSON string.</returns>
        public string AsJson(bool indented = true)
        {
            _settings.Formatting = indented ? Formatting.Indented : Formatting.None;
            return JsonConvert.SerializeObject(this, _settings);
        }
    }
}