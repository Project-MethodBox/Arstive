using System.Text.Json.Serialization;
using System.Windows.Input;

namespace Arstive.Model
{
    [Serializable]
    public class JudgmentAngle(Key bindingKey, int index, int speed,
        List<Interfaces.BindNote>? noteList,
        List<Interfaces.ElementEventBase>? eventList, (int,int) position)
    {
        /// <summary>
        /// Keyboard keys that trigger judgment
        /// </summary>
        [JsonPropertyName("binding_key")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Key BindingKey { get; set; } = bindingKey;

        /// <summary>
        /// Index of current judgment angle
        /// </summary>
        [JsonPropertyName("index")]
        public int Index { get; set; } = index;

        /// <summary>
        /// Judgment angle behavior lists
        /// </summary>
        public List<Interfaces.ElementEventBase>? EventList { get; set; } = eventList;

        /// <summary>
        /// List of notes bound to Judgment Angle
        /// </summary>
        public List<Interfaces.BindNote>? NoteLists { get; set; } = noteList;

        /// <summary>
        /// Determine the initialization relative position of the angle
        /// </summary>
        public (int, int) Position { get; set; }= position;

        /// <summary>
        /// Determine the relative speed of online notes, with
        /// 200 pixels per second as 1
        /// </summary>
        public int Speed { get; set; } = speed;
    }
}
