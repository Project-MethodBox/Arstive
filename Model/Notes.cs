using System.Text.Json.Serialization;
using System.Windows.Input;

namespace Arstive.Model
{
    public class Notes
    {
        public class Tap : Interfaces.BindNote;
        public class Drag : Interfaces.BindNote;

        public class Hold : Interfaces.BindNote
        {
            /// <summary>
            /// End tap of hold hit
            /// </summary>
            [JsonPropertyName("end_time")]
            public int EndTime { get; set; }

            /// <summary>
            /// Indicate whether Hold can be further evaluated
            /// </summary>
            [JsonIgnore]
            public bool IsJudgment { get; set; } = true;
        }

        public class Flick : Interfaces.FreeNote 
        {
            /// <summary>
            /// Start key of flick
            /// </summary>
            [JsonPropertyName("start_key")]
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public Key StartKey { get; set; }


            /// <summary>
            /// End key of flick
            /// </summary>
            [JsonPropertyName("end_key")]
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public Key EndKey { get; set; }

            /// <summary>
            /// Indicate whether Hold can be further evaluated
            /// </summary>
            [JsonIgnore]
            public bool IsJudgment { get; set; } = false;
        }

        public enum NoteType
        {
            Tap,
            Drag,
            Hold,
            Flick
        }
    }
}
