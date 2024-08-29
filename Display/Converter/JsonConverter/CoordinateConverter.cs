using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Arstive.Display.Converter.JsonConverter
{
    internal class CoordinateConverter : JsonConverter<(int, int)>
    {
        public override (int, int) Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString();
            var arr = str!.Split(",");
            return (int.Parse(arr[0]), int.Parse(arr[1]));
        }

        public override void Write(Utf8JsonWriter writer, (int, int) value, JsonSerializerOptions options)
        {
            writer.WriteStringValue($"{value.Item1},{value.Item2}");
        }
    }
}
