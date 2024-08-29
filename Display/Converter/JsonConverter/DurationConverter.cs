using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
using System.Windows;

namespace Arstive.Display.Converter.JsonConverter
{
    public class DurationConverter : JsonConverter<Duration>
    {
        public override Duration Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException("Invalid token type");
            }

            string durationString = reader.GetString();

            if (durationString == null)
            {
                return new Duration(TimeSpan.Zero);
            }

            if (TimeSpan.TryParse(durationString, out var timeSpan))
            {
                return new Duration(timeSpan);
            }
            else
            {
                throw new JsonException($"Cannot parse '{durationString}' to a valid duration.");
            }
        }

        public override void Write(Utf8JsonWriter writer, Duration value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.TimeSpan.ToString());
        }
    }
}