using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SharpProp.Outputs
{
    public class Jsonable
    {
        private readonly JsonSerializerSettings _settings = new()
            {Converters = new List<JsonConverter> {new StringEnumConverter()}};

        /// <summary>
        ///     Converts object to a JSON string
        /// </summary>
        /// <param name="indented">Adding indents</param>
        /// <returns>JSON string</returns>
        public string AsJson(bool indented = false)
        {
            _settings.Formatting = indented ? Formatting.Indented : Formatting.None;
            return JsonConvert.SerializeObject(this, _settings);
        }
    }
}