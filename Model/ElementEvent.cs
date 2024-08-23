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
        internal class MoveEvent : Interfaces.ElementEventBase
        {
            /// <summary>
            /// The endpoint of element movement
            /// </summary>
            internal (int, int) Destination;
        }
    }

}
