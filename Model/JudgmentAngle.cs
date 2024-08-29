using System.Text.Json.Serialization;
using System.Windows.Input;
using Arstive.Display.Converter.JsonConverter;

namespace Arstive.Model
{
    [Serializable]
    public class JudgmentAngle
    {
        public JudgmentAngle(Key bindingKey, int index, int speed, List<Interfaces.BindNote>? noteLists, List<Interfaces.ElementEventBase>? eventList, (int, int) position)
        {
            BindingKey = bindingKey;
            Index = index;
            EventList = eventList;
            NoteLists = noteLists;
            Position = position;
            Speed = speed;
        }

        /// <summary>
        /// Keyboard keys that trigger judgment
        /// </summary>
        [JsonPropertyName("binding_key")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Key BindingKey { get; set; }

        /// <summary>
        /// Index of current judgment angle
        /// </summary>
        [JsonPropertyName("index")]
        public int Index { get; set; }

        /// <summary>
        /// Judgment angle behavior lists
        /// </summary>
        [JsonPropertyName("events")]
        public List<Interfaces.ElementEventBase>? EventList { get; set; }

        /// <summary>
        /// List of notes bound to Judgment Angle
        /// </summary>
        [JsonPropertyName("notes")]
        public List<Interfaces.BindNote>? NoteLists { get; set; }

        /// <summary>
        /// Determine the initialization relative position of the angle
        /// </summary>
        [JsonPropertyName("position")]
        [JsonConverter(typeof(CoordinateConverter))]
        public (int, int) Position { get; set; }

        /// <summary>
        /// Determine the relative speed of online notes, with
        /// 200 pixels per second as 1
        /// </summary>
        [JsonPropertyName("speed")]
        public int Speed { get; set; }
    }
}
