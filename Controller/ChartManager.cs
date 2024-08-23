using System.Windows;
using System.Collections.Frozen;
using System.Windows.Input;
using Arstive.Model;

namespace Arstive.Controller
{
    internal class ChartManager
    {
        /// <summary>
        /// ChartManger singleton instance
        /// </summary>
        internal static ChartManager Shared { get; set; } = new();

        /// <summary>
        /// Dictionary that explains the relationship between line
        /// indexes and associated keys
        /// </summary>
        internal FrozenDictionary<int, Key>? KeyIndexPairs;
    }
}