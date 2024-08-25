using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Arstive.Model
{
    public class ElementEvent
    {
        /// <summary>
        /// Action event types of elements in the game
        /// </summary>
        public enum ElementEventType
        {
            Move,
            Rotate,
            Opaque
        }

        /// <summary>
        /// Event that causes an element to move
        /// </summary>
        internal class MoveEvent(double startTime, Duration duration, (int,int) destination) : Interfaces.ElementEventBase(startTime, duration)
        {
            /// <summary>
            /// The endpoint of element movement
            /// </summary>
            internal (int, int) Destination = destination;
        }

        /// <summary>
        /// Event that causes an element to rotate
        /// </summary>
        internal class RotateEvent(double startTime, Duration duration, int endAngle) : Interfaces.ElementEventBase(startTime, duration)
        {
            /// <summary>
            /// The endpoint to which the angle rotates
            /// </summary>
            internal int EndAngle = endAngle;
        }
    }

}
