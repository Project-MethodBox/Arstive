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
            internal int StartTime { get; set; }

            /// <summary>
            /// The time elapsed for the complete operation of the event
            /// </summary>
            internal Duration Duration { get; set; }

            /// <summary>
            /// Type of current event
            /// </summary>
            internal ElementEvent.ElementEventType EventType { get; set; }
        } 
    }
}
