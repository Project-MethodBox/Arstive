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
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Arstive.Controller
{
    public class GameController
    {
        public static event EventHandler<PropertyChangedEventArgs>?
            StaticPropertyChanged;
        private static readonly NotePooledObjectPolicy NotePool = new();
        private static Dispatcher? _dispather;
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
        internal static void LoadChart(string path, Action<UIElement> addControl)
        {
            var tap = new Notes.Tap();
            tap.HitTime = 3330;
            tap.JudgmentAngleIndex = 0;
            tap.Index = 1;
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
                    new(Key.A,0, 3,[tap],null,(-90,-840)),
                    //new(Key.X,1,2, [tap2],null,(-590,-940))
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
                    RotateAngle = 45,
                    Margin = new Thickness(w, h, 0, 0),
                    Speed = judgmentAngle.Speed,
                    NoteLists = judgmentAngle.NoteLists!
                };

                // Start judge
                ThreadPool.QueueUserWorkItem(Judge, judgmentAngleInstance);

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
            var angle = (JudgmentAngleDisplay)judgmentAngle!;
            var ticks = 0;

            // Can determine Drag notes
            var pressed = false;
            // Can determine Tap notes
            var tap = false;

            // The notes that have already been created are used to determine
            var judgmentList = new List<Interfaces.NoteBase>();

            // Down time
            var flootTime = 1410 / (double)(200 * (angle.Speed));

            // Capture Dispather for thread synchronization
            _dispather ??= Application.Current.Dispatcher;

            // Enter judgment
            while (true)
            {
                // Count time elapsed
                ticks += 10;
                var ticksInternal = ticks;
                _dispather.Invoke(() =>
                {
                    // Switch if specific key is pressed
                    if (Keyboard.IsKeyDown(angle.BindingKey) && !tap)
                    {
                        tap = !pressed;
                        pressed = true;
                        Trace.WriteLine($"Judgment angle #{angle.Index}:Tap at {ticksInternal}!");
                        Trace.WriteLine($"Judgment angle #{angle.Index}:Drag begin at {ticksInternal}!");
                    }

                    else if (Keyboard.IsKeyUp(angle.BindingKey) && pressed)
                    {
                        pressed = false;
                        Trace.WriteLine($"Judgment angle #{angle.Index}:Drag end at {ticksInternal}!");
                    }

                    if (Keyboard.IsKeyUp(angle.BindingKey) && tap)
                        tap = false;

                    // Note animation
                    if (angle.NoteLists is null)
                    {
                        return;
                    }
                    for (int i = 0; i < angle.NoteLists.Count; i++)
                    {
                        var note = angle.NoteLists[i];
                        int deltaAnimation = note.HitTime - ticksInternal;

                        // Need create
                        if (deltaAnimation / 1000 <= flootTime)
                        {
                            if (note.GetType() == typeof(Notes.Tap))
                            {
                                // Create tap instance
                                var tapInstance = new TapDisplay
                                {
                                    Index = note.Index,
                                    Name = $"tapInstance{note.Index}",
                                    Margin = new(0, -1360, 0, 0)
                                };

                                angle.MainPanel.RegisterName(tapInstance.Name, tapInstance);

                                // Create double animation
                                var downAnimation = new ThicknessAnimation(new(0, -1360, 0, 0),
                                    new(-0, -315, 0, 0),
                                    new Duration(TimeSpan.FromMilliseconds(2000)))
                                {
                                    AutoReverse = false
                                };

                                // Binding animation via story board
                                var storyBoard = new Storyboard();
                                storyBoard.Children.Add(downAnimation);
                                Storyboard.SetTargetName(downAnimation, tapInstance.Name);
                                Storyboard.SetTargetProperty(downAnimation,
                                    new(FrameworkElement.MarginProperty));

                                // Add to angle
                                var test = new TapDisplay();
                                test.Margin = new(0, -1960, 0, 0);
                                angle.MainPanel.Children.Add(tapInstance);

                                storyBoard.Begin(angle.MainPanel);
                                judgmentList.Add(note);
                                angle.NoteLists.Remove(note);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                    // Enter judgment
                    // Find nearest note
                    var length = judgmentList!.Count >= 3 ? 3 : judgmentList.Count;
                    for (var i = 0; i < length; i++)
                    {

                        // Judgment interval:
                        // [-60,60]ms => P
                        // [-120,120]ms => G
                        // [-250,250]ms => B
                        // <-250 or >250 ms => Not making a judgment
                        if (judgmentList!.Count == 0)
                        {
                            // The judgment line is used up, recycle it
                            continue;
                        }

                        // Calculate time difference
                        if (judgmentList[i].GetType() == typeof(Notes.Tap)
                            && tap)
                        {
                            int delta = judgmentList[i].HitTime - ticks - 1100;
                            if (delta is <= 250 and >= -250)
                            {
                                // o.O
                                System.Media.SystemSounds.Asterisk.Play();
                                Score += delta switch
                                {
                                    >= -60 and <= 60 => 2000,
                                    >= -120 and <= 120 => 1200,
                                    _ => 0
                                };
                                // Reset flag and recycle note
                                tap = false;
                                judgmentList.RemoveAt(i);
                            }
                        }
                    }
                });

                Thread.Sleep(10);
            }
        }
    }
}