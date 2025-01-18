using Arstive.Model;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Arstive.Model.ElementEvent;

namespace Arstive.Display.Converter.JsonConverter
{
    internal class PolymorphicConverter : JsonConverter<Interfaces.ElementEventBase>
    {
        public override Interfaces.ElementEventBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);
            var obj = doc.RootElement;
            var type = obj.GetProperty("type").GetString();

            return type switch
            {
                "Move" => JsonSerializer.Deserialize<MoveEvent>(obj.GetRawText(), options),
                "Rotate" => JsonSerializer.Deserialize<RotateEvent>(obj.GetRawText(), options),
                "Visible" => JsonSerializer.Deserialize<VisibleEvent>(obj.GetRawText(), options),
                _ => throw new JsonException("Invalid event type")
            };
        }

        public override void Write(Utf8JsonWriter writer, Interfaces.ElementEventBase value, JsonSerializerOptions options)
        {
            if (value is MoveEvent move)
                JsonSerializer.Serialize(writer, move, typeof(MoveEvent), options);
            else if (value is RotateEvent rotate)
                JsonSerializer.Serialize(writer, rotate, typeof(RotateEvent), options);
            else if (value is VisibleEvent visible)
                JsonSerializer.Serialize(writer, visible, typeof(RotateEvent), options);
            else
                throw new ArgumentException("Invalid event type");
        }
    }
}
