using System.Windows.Input;
using static Arstive.Model.Interfaces;

namespace Arstive.Model
{
    public class JudgmentAngle(Key bindingKey, int index, List<Interfaces.ElementEventBase>? eventLists, (int,int) position)
    {
        /// <summary>
        /// Keyboard keys that trigger judgment
        /// </summary>
        public Key BindingKey { get; set; } = bindingKey;

        /// <summary>
        /// Index of current judgment angle
        /// </summary>
        public int Index { get; set; } = index;

        /// <summary>
        /// Judgment angle behavior lists
        /// </summary>
        public List<ElementEventBase>? EventLists { get; set; } = eventLists;

        /// <summary>
        /// Determine the initialization relative position of the angle
        /// </summary>
        public (int, int) Position { get; set; }= position;
    }
}
