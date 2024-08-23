using Arstive.Display;
using Arstive.Model;
using System.Collections.Frozen;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Arstive.Controller
{
    public class GameController
    {
        /// <summary>
        /// Load chart to window
        /// </summary>
        /// <param name="path">Path of the chart to be read</param>
        /// <param name="addControl">Method for adding controls to the
        /// main window</param>
        internal static void LoadChart(string path, Action<UIElement> addControl)
        {
            var chart = new Chart
            {
                BasicInfo = new()
                {
                    Charter = "Arabidopsis -Overdose-",
                    Composer = "Fl00t vs. Halv",
                    ChartDifficultyNumber = 7.1,
                    ChartDifficultyName = ChartDifficulty.Quadrilateral,
                    SongName = "Cuvism.mp3",
                    Version = "1.0.0"
                },
                JudgmentAngles = [
                    new(Key.W,0, null,(-60,5))
                ]
            };

            // Load chart to window
            var indexKeyParis = new Dictionary<int, Key>();
            foreach (var judgmentAngle in chart.JudgmentAngles)
            {
                indexKeyParis.Add(judgmentAngle.Index, judgmentAngle.BindingKey);

                // Generate judgment angles
                var w = judgmentAngle.Position.Item1;
                var h = judgmentAngle.Position.Item2;

                // Create instance and set default value
                var judgmentAngleInstance = new JudementAngleDisplay
                {
                    BindingKey = judgmentAngle.BindingKey,
                    LeftRotateAngle = -45,
                    RightRotateAngle = -45,
                    Margin = new Thickness(w, h, 0, 0)
                };

                // Add event
                foreach (var elementEvent in judgmentAngle.EventLists!)
                {
                    HandleEvent(elementEvent);
                }

                // Add to grid
                addControl(judgmentAngleInstance);
            }
            // Push to corresponding classes
            ChartManager.Shared.KeyIndexPairs = indexKeyParis.ToFrozenDictionary();

            // Play chart and count time
            var player = new MediaPlayer();
            player.Open(new($"{Environment.CurrentDirectory}\\Cuvism.mp3"));
            player.Play();
        }

        private static void HandleEvent(Interfaces.ElementEventBase elementEvent)
        {

        }

        internal void Judgment()
        {

        }
    }
}
