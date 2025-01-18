using System.Text.Json.Serialization;
using System.Windows;
using Arstive.Display.Converter.JsonConverter;

namespace Arstive.Model
{
    public class ElementEvent
    {
        /// <summary>
        /// Event that causes an element to move
        /// </summary>
        public class MoveEvent : Interfaces.ElementEventBase
        {
            /// <summary>
            /// The endpoint of element movement
            /// </summary>
            [JsonPropertyName("dest")]
            [JsonConverter(typeof(CoordinateConverter))]
            public (int, int) Destination { get; set; }
        }

        /// <summary>
        /// Event that causes an element to rotate
        /// </summary>
        public class RotateEvent : Interfaces.ElementEventBase
        {
            /// <summary>
            /// The endpoint to which the angle rotates
            /// </summary>
            [JsonPropertyName("end_angle")]
            public int EndAngle { get; set; }
        }

        public class VisibleEvent : Interfaces.ElementEventBase
        {
            /// <summary>
            /// Visible state of judgment angle
            /// </summary>
            [JsonPropertyName("visibility")] 
            public bool Visibility { get; set; }
        }

        public enum ElementEventType
        {
            Move,
            Rotate,
            Visible,
            Color
        }
    }
}
