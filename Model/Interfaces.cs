using System.Text.Json.Serialization;
using System.Windows;
using Arstive.Display.Converter.JsonConverter;
using static Arstive.Model.ElementEvent;
using DurationConverter = Arstive.Display.Converter.JsonConverter.DurationConverter;

namespace Arstive.Model
{
    public class Interfaces
    {
        /// <summary>
        /// Public interface for all event in the game
        /// </summary>
        [Serializable]
        [JsonDerivedType(typeof(MoveEvent),"moveEvent")]
        [JsonDerivedType(typeof(RotateEvent), "rotateEvent")]
        [JsonDerivedType(typeof(VisibleEvent), "visibleEvent")]
        public class ElementEventBase
        {
            /// <summary>
            /// The number of milliseconds between the start of the game
            /// and the triggering of the event
            /// </summary>
            [JsonPropertyName("start_time")]
            public double StartTime { get; set; }

            /// <summary>
            /// The time elapsed for the complete operation of the event
            /// </summary>
            [JsonPropertyName("duration")]
            [JsonConverter(typeof(DurationConverter))]
            public Duration Duration { get; set; }

            /// <summary>
            /// Types of event easing
            /// </summary>
            [JsonPropertyName("easing")]
            public Easing? Easing { get; set; }

            /// <summary>
            /// Type of current event
            /// </summary>
            [JsonPropertyName("type")]
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public ElementEvent.ElementEventType EventType { get; set; }
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

            /// <summary>
            /// Type of note
            /// </summary>
            public Notes.NoteType NoteType { get; set; }
        }

        [JsonDerivedType(typeof(Notes.Flick),"flickNote")]
        public class FreeNote(Notes.NoteType type) : INote
        {
            [JsonPropertyName("hit_time")]
            public int HitTime { get; set; }

            [JsonPropertyName("index")]
            public int Index{ get; set; }

            [JsonPropertyName("type")]
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public Notes.NoteType NoteType { get; set; } = type;

            /// <summary>
            /// Time when the note start judge
            /// </summary>
            [JsonPropertyName("start_time")]
            public int StartTime { get; set; }

            /// <summary>
            /// Position of note
            /// </summary>
            [JsonPropertyName("margin")]
            [JsonConverter(typeof(CoordinateConverter))]
            public (int,int) NoteMargin { get; set; }
        }

        [JsonDerivedType(typeof(Notes.Tap), "tapNote")]
        [JsonDerivedType(typeof(Notes.Drag), "dragNote")]
        [JsonDerivedType(typeof(Notes.Hold), "holdNote")]
        public class BindNote : INote
        {
            [JsonPropertyName("hit_time")]
            public int HitTime { get; set; }

            [JsonPropertyName("index")]
            public int Index { get; set; }

            /// <summary>
            /// Index of determining the angle to which the note belongs
            /// </summary>
            [JsonPropertyName("jndex")]
            public int JudgmentAngleIndex { get; set; }

            [JsonPropertyName("type")]
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public Notes.NoteType NoteType { get; set; }
        }
    }
}
