using System.Windows;

namespace Arstive.Model
{
    internal class Interfaces
    {
        /// <summary>
        /// Public interface for all event in the game
        /// <typeparam name="T">Parameter type corresponding to the event</typeparam>
        /// </summary>
        internal interface IElementEvent<T>
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

            /// <summary>
            /// Other parameters required for the event, for Move events,
            /// it should be (int, int); for Rotation events, it should
            /// be an int; For opacity events, it should be an int between 0 and 100.
            /// </summary>
            internal T Parameters { get; set; }
        } 
    }
}
