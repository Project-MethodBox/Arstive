using System.Text.Json.Serialization;
using System.Windows;

namespace Arstive.Model
{
    public class ElementEvent
    {
        /// <summary>
        /// Event that causes an element to move
        /// </summary>
        internal class MoveEvent(double startTime, Duration duration, (int, int) destination, Easing? easing = null) : Interfaces.ElementEventBase(startTime, duration, easing)
        {
            /// <summary>
            /// The endpoint of element movement
            /// </summary>
            [JsonPropertyName("dest")]
            internal (int, int) Destination = destination;
        }

        /// <summary>
        /// Event that causes an element to rotate
        /// </summary>
        internal class RotateEvent(double startTime, Duration duration, int endAngle, Easing? easing = null) : Interfaces.ElementEventBase(startTime, duration, easing)
        {
            /// <summary>
            /// The endpoint to which the angle rotates
            /// </summary>
            [JsonPropertyName("end_angle")]
            internal int EndAngle = endAngle;
        }

        internal class VisibleEvent(double startTime, bool visibility) : Interfaces.ElementEventBase(startTime,new(),null)
        {
            /// <summary>
            /// Visible state of judgment angle
            /// </summary>
            [JsonPropertyName("visibility")]
            internal bool Visibility = visibility;
        }
    }

}
