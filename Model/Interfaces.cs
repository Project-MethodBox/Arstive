using System.Text.Json.Serialization;
using System.Windows;

namespace Arstive.Model
{
    public class Interfaces
    {
      
        /// <summary>
        /// Public interface for all event in the game
        /// </summary>
        [Serializable]
        public abstract class ElementEventBase(
            double startTime,
            Duration duration,Easing? easing = null)
        {
            /// <summary>
            /// The number of milliseconds between the start of the game
            /// and the triggering of the event
            /// </summary>
            [JsonPropertyName("start_time")]
            internal double StartTime { get; set; } = startTime;

            /// <summary>
            /// The time elapsed for the complete operation of the event
            /// </summary>
            internal Duration Duration { get; set; } = duration;

            /// <summary>
            /// Types of event easing
            /// </summary>
            [JsonPropertyName("easing")]
            internal Easing? Easing = easing;
        }

        public interface INote
        {
            /// <summary>
            /// The time when the Note was hit, measured in ten milliseconds
            /// </summary>
            [JsonPropertyName("hit_time")]
            public int HitTime { get; set; }

            /// <summary>
            /// The Index of note relative to the current judgment angle
            /// </summary>
            [JsonPropertyName("index")]
            public int Index { get; set; }
        }

        public abstract class FreeNote : INote
        {
            [JsonPropertyName("hit_time")]
            public int HitTime { get; set; }

            [JsonPropertyName("index")]
            public int Index{ get; set; }

            /// <summary>
            /// Time when the note start judge
            /// </summary>
            [JsonPropertyName("start_time")]
            public int StartTime { get; set; }
        }

        public abstract class BindNote: INote
        {
            [JsonPropertyName("hit_time")]
            public int HitTime { get; set; }

            [JsonPropertyName("index")]
            public int Index { get; set; }

            /// <summary>
            /// Index of determining the angle to which the note belongs
            /// </summary>
            [JsonPropertyName("index")]
            public int JudgmentAngleIndex { get; set; }

        }
    }
}
