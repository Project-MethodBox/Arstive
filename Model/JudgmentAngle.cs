using System.Windows.Input;

namespace Arstive.Model
{
    internal class JudgmentAngle
    {
        /// <summary>
        /// Keyboard keys that trigger judgment
        /// </summary>
        internal Key BindingKey { get; set; }

        /// <summary>
        /// Index of current judgment angle
        /// </summary>
        internal int Index { get; set; }
    }
}
