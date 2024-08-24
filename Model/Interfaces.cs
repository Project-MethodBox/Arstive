using System.Windows;

namespace Arstive.Model
{
    public class Interfaces
    {
        /// <summary>
        /// Public interface for all event in the game
        /// </summary>
        public abstract class ElementEventBase
        {
            /// <summary>
            /// The number of milliseconds between the start of the game
            /// and the triggering of the event
            /// </summary>
            internal double StartTime { get; set; }

            /// <summary>
            /// The time elapsed for the complete operation of the event
            /// </summary>
            internal Duration Duration { get; set; }

            /// <summary>
            /// Type of current event
            /// </summary>
            internal ElementEvent.ElementEventType EventType { get; set; }
        }
        
        public abstract class NoteBase
        {
            /// <summary>
            /// Index of determining the angle to which the note belongs
            /// </summary>
            public int JudgmentAngleIndex { get; set; }

            /// <summary>
            /// The time when the Note was hit, measured in ten milliseconds
            /// </summary>
            public int HitTime { get; set; }

            /// <summary>
            /// The Index of note relative to the current judgment angle
            /// </summary>
            public int Index{ get; set; }
        }
    }
}
