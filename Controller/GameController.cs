using Arstive.Display.Element;
using Arstive.Model;
using Arstive.Model.ObjectPool;
using System.Collections.Frozen;
using System.ComponentModel;
using System.Diagnostics;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Arstive.Controller
{
    public class GameController
    {
        public static event EventHandler<PropertyChangedEventArgs>? 
            StaticPropertyChanged;
        private static readonly NotePooledObjectPolicy NotePool = new();
        private static Dispatcher? dispather;
        private static int _score = 0;

        /// <summary>
        /// User score used to bind to the main window
        /// </summary>
        public static int Score
        {
            get => _score;
            set
            {
                _score = value;
                StaticPropertyChanged!(null, new("Score"));
            }
        }

        /// <summary>
        /// Load chart to window
        /// </summary>
        /// <param name="path">Path of the chart to be read</param>
        /// <param name="addControl">Method for adding controls to the
        /// main window</param>
        internal static async void LoadChart(string path, Action<UIElement> addControl)
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
                    new(Key.A,0, null,null,(-90,250)),
                    new(Key.X,1, null,null,(90,-120))
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
                var judgmentAngleInstance = new JudgmentAngleDisplay
                {
                    BindingKey = judgmentAngle.BindingKey,
                    LeftRotateAngle = -45,
                    RightRotateAngle = -45,
                    Margin = new Thickness(w, h, 0, 0)
                };

                // Start judge
                ThreadPool.QueueUserWorkItem(Judge, judgmentAngle);

                // Add event
                //foreach (var elementEvent in judgmentAngle.EventList!)
                //{
                //    HandleEvent(elementEvent);
                //}

                // Add to grid
                addControl(judgmentAngleInstance);
            }
            // Push to corresponding classes
            ChartManager.Shared.KeyIndexPairs = indexKeyParis.ToFrozenDictionary();

            // Play chart and count time
            SoundPlayer player = new SoundPlayer("Cuvism.wav");
            player.Play();
        }

        internal static void Judge(object? judgmentAngle)
        {
            // Prepare instance and timer
            var angle = (JudgmentAngle)judgmentAngle!;
            var ticks = 0;

            // Can determine Drag notes
            var pressed = false;
            // Can determine Tap notes
            var tap = false;

            // Capture Dispather for thread synchronization
            dispather ??= Application.Current.Dispatcher;

            // Enter judgment
            while (true)
            {
                // Count time elapsed
                ticks += 10;
                dispather.Invoke(() =>
                {
                    // Switch if specific key is pressed
                    if (Keyboard.IsKeyDown(angle.BindingKey) && !tap)
                    {
                        tap = !pressed;
                        pressed = true;
                        Trace.WriteLine($"Judgment angle #{angle.Index}:Tap at {ticks}!");
                        Trace.WriteLine($"Judgment angle #{angle.Index}:Drag begin at {ticks}!");
                    }

                    else if (Keyboard.IsKeyUp(angle.BindingKey) && pressed)
                    {
                        pressed = false;
                        Trace.WriteLine($"Judgment angle #{angle.Index}:Drag end at {ticks}!");
                    }

                    if (Keyboard.IsKeyUp(angle.BindingKey) && tap)
                        tap = false;
                });
                Thread.Sleep(10);
                //// Find nearest note
                //for (int i = 0; i < 3; i++)
                //{

                //    // Judgment interval:
                //    // [-60,60]ms => P
                //    // [-120,120]ms => G
                //    // [-250,250]ms => B
                //    // <-250 or >250 ms => Not making a judgment
                //    if (angle.NoteLists!.Count == 0)
                //    {
                //        // The judgment line is used up, recycle it
                //        return;
                //    }

                //    // Calculate time difference
                //    if (angle.NoteLists![i].GetType() == typeof(Notes.Tap)
                //        && tap)
                //    {
                //        int delta = angle.NoteLists![i].HitTime - ticks;
                //        if (delta <= 250 && delta >= -250)
                //        {
                //            Score += delta switch
                //            {
                //                >= -60 and <= 60 => 2000,
                //                >= -120 and <= 120 => 1200,
                //                _ => 0
                //            };
                //        }

                //        // Reset flag and recycle note
                //        tap = false;
                //        angle.NoteLists.RemoveAt(i);
                //    }

                //}

                // Enter animate


            }
        }
    }
}
