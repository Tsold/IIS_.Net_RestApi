using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace RestService.Models.Json
{
    public partial class AlbumInfo
    {
        [JsonProperty("album")]
        public string Album { get; set; }

        [JsonProperty("albumkey")]
        public string Albumkey { get; set; }
    }

    public partial class AlbumInfo
    {
        public static AlbumInfo FromJson(string json) => JsonConvert.DeserializeObject<AlbumInfo>(json, RestService.Models.Json.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this AlbumInfo self) => JsonConvert.SerializeObject(self, RestService.Models.Json.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
