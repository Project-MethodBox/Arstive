using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arstive.Model
{
    internal class ElementEvent
    {
        /// <summary>
        /// Action event types of elements in the game
        /// </summary>
        internal enum ElementEventType
        {
            Move,
            Rotate,
            Opaque
        }
    }
}
