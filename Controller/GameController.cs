using Arstive.Display.Element;
using Arstive.Model;
using System.Collections.Frozen;
using System.ComponentModel;
using System.Diagnostics;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Arstive.Display;

namespace Arstive.Controller
{
    public class GameController
    {
        public static event EventHandler<PropertyChangedEventArgs>?
            StaticPropertyChanged;

        private static Dispatcher? _dispather;
        internal static MainWindow? MainWindow;
        private static int _score = 0;

        /// <summary>
        /// When any judgment entity exists, block to avoid
        /// entering the end of the game
        /// </summary>
        private static Barrier? _Barrier;

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

            var chart = ChartTest.LoadTest();

            // Load chart to window
            var indexKeyParis = new Dictionary<int, Key>();

            // Initialize barrier
            _Barrier = new(chart.JudgmentAngles.Count);

            foreach (var judgmentAngle in chart.JudgmentAngles)
            {
                indexKeyParis.Add(judgmentAngle.Index, judgmentAngle.BindingKey);

                // Generate judgment angles
                var w = judgmentAngle.Position.Item1;
                var h = judgmentAngle.Position.Item2;

                // Create instance and set default value
                var judgmentAngleInstance = new JudgmentAngleDisplay
                {
                    Index = judgmentAngle.Index,
                    BindingKey = judgmentAngle.BindingKey,
                    RotateAngle = 45,
                    Margin = new Thickness(w, h, 0, 0),
                    Speed = judgmentAngle.Speed,
                    NoteList = judgmentAngle.NoteLists!,
                    EventList = judgmentAngle.EventList!,
                    Name = $"Angle_{judgmentAngle.Index}"
                };

                // Start judge
                //new Thread(Judge).Start(judgmentAngleInstance);
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

            // Capture dispather for thread synchronization
            _dispather ??= Application.Current.Dispatcher;

            // Enter judgment
            while (true)
            {
                // Count time elapsed
                ticks += 10;
                var ticksInternal = ticks;
                var ticksLocal = ticks;
                _dispather.Invoke(() =>
                {
                    // Switch if specific key is pressed
                    if (Keyboard.IsKeyDown(angle.BindingKey) && !tap)
                    {
                        tap = !pressed;
                        pressed = true;
                        //Trace.WriteLine($"Judgment angle #{angle.Index}:Tap at {ticksInternal}!");
                        //Trace.WriteLine($"Judgment angle #{angle.Index}:Drag begin at {ticksInternal}!");
                    }

                    else if (Keyboard.IsKeyUp(angle.BindingKey) && pressed)
                    {
                        pressed = false;
                        //Trace.WriteLine($"Judgment angle #{angle.Index}:Drag end at {ticksInternal}!");
                    }

                    if (Keyboard.IsKeyUp(angle.BindingKey) && tap)
                        tap = false;

                    //// Wait for suicide
                    //if (angle.NoteList.Count == 0)
                    //{
                    //    _Barrier!.SignalAndWait();
                    //    return;
                    //}

                    // Note animation
                    for (int i = 0; i < angle.NoteList.Count; i++)
                    {
                        var note = angle.NoteList[i];
                        int deltaAnimation = note.HitTime - ticksInternal;

                        // Need create
                        if (deltaAnimation <= flootTime * 1000)
                        {
                            // Create note instance
                            UserControl? noteInstance = note switch
                            {
                                Notes.Tap _ => new TapDisplay
                                {
                                    Index = note.Index,
                                    Name = $"tapInstance{angle.Index}T{note.Index}",
                                    Margin = new(0, -1360, 0, 0)
                                },
                                Notes.Drag _ => new DragDisplay()
                                {
                                    Index = note.Index,
                                    Name = $"dragInstance{angle.Index}T{note.Index}",
                                    Margin = new(0, -1360, 0, 0)
                                },
                                Notes.Hold hold => new HoldDisplay()
                                {
                                    Index = note.Index,
                                    Name = $"holdInstance{angle.Index}T{note.Index}",
                                    Margin = new(0, -1360, 0, 0),
                                    Length = ((hold.EndTime - hold.HitTime) * (200 * (angle.Speed)))/1000 + 40
                                },
                                _ => null
                            };

                            angle.MainPanel.RegisterName(noteInstance!.Name, noteInstance);

                            // Create double animation
                            var startPosition = -1360;
                            if (note.GetType() == typeof(Notes.Hold))
                            {
                                startPosition -= (((Notes.Hold)note).EndTime - note.HitTime) * 
                                                 (200 * (angle.Speed))/1000;
                            }

                            var downAnimation = new ThicknessAnimation(new(0, startPosition, 0, 0),
                                new(-0, -315, 0, 0),
                                new Duration(TimeSpan.FromMilliseconds(flootTime * 1000)))
                            {
                                AutoReverse = false
                            };

                            // Binding animation via story board
                            var storyBoard = new Storyboard();
                            storyBoard.Children.Add(downAnimation);
                            Storyboard.SetTargetName(downAnimation, noteInstance.Name);
                            Storyboard.SetTargetProperty(downAnimation,
                                new(FrameworkElement.MarginProperty));

                            // Add to angle
                            angle.MainPanel.Children.Add(noteInstance);

                            storyBoard.Begin(angle.MainPanel);
                            judgmentList.Add(note);
                            angle.NoteList.Remove(note);

                        }
                        else
                        {
                            break;
                        }
                    }

                    // Enter judgment
                    // Find nearest note
                    for (int i = 0; i < judgmentList.Count; i++)
                    {
                        // Judgment interval:
                        // [-60,60]ms => P
                        // [-120,120]ms => G
                        // [-250,250]ms => B
                        // <-250 or >250 ms => Not making a judgment
                        // Calculate time difference
                        if (judgmentList[i].GetType() == typeof(Notes.Tap)
                            && tap)
                        {
                            int delta = judgmentList[i].HitTime - ticksLocal - 140;
                            Trace.WriteLine($"#{i}:{delta}");
                            if (delta is <= 420 and >= -420)
                            {
                                // o.O
                                System.Media.SystemSounds.Asterisk.Play();
                                Score += delta switch
                                {
                                    >= -120 and <= 120 => 2000,
                                    >= -240 and <= 240 => 1200,
                                    _ => 0
                                };
                                // Reset flag and recycle note
                                tap = false;
                                angle.MainPanel.Children.RemoveAt(1);
                                judgmentList.RemoveAt(i);
                            }
                        }
                        else if (judgmentList[i].GetType() == typeof(Notes.Drag) && pressed)
                        {
                            var delta = judgmentList[i].HitTime - ticksLocal - 140;
                            if (delta is <= 120 and >= -120)
                            {
                                // Only have judgment perfect
                                System.Media.SystemSounds.Asterisk.Play();
                                Score += 2000;
                                angle.MainPanel.Children.RemoveAt(1);
                                judgmentList.RemoveAt(i);
                            }
                        }
                    }

                    // Enter animation
                    for (int i = 0; i < angle.EventList.Count; i++)
                    {
                        if (angle.EventList[i].StartTime <= ticksLocal)
                        {
                            if (angle.EventList[i].GetType() == typeof(ElementEvent.MoveEvent))
                            {
                                // Create event instance
                                var moveEvent = (ElementEvent.MoveEvent)angle.EventList[i];

                                // Create double animation
                                var moveAnimation = new ThicknessAnimation(angle.Margin,
                                    new(moveEvent.Destination.Item1, moveEvent.Destination.Item2, 0, 0),
                                    moveEvent.Duration)
                                {
                                    AutoReverse = false
                                };

                                // Register in MainWindow
                                MainWindow!.GameGrid.RegisterName(angle.Name, angle);

                                // Add easing function
                                if (moveEvent.Easing is not null)
                                {
                                    moveAnimation.EasingFunction = moveEvent.Easing.GetEasingFunction();
                                }

                                // Binding animation via story board
                                var storyBoard = new Storyboard();
                                storyBoard.Children.Add(moveAnimation);
                                Storyboard.SetTargetName(moveAnimation, angle.Name);
                                Storyboard.SetTargetProperty(moveAnimation,
                                    new(FrameworkElement.MarginProperty));

                                storyBoard.Begin(MainWindow.GameGrid);

                                // Remove processed events from the head
                                MainWindow!.GameGrid.UnregisterName(angle.Name);
                                angle.EventList.RemoveAt(0);
                            }
                            else if (angle.EventList[i].GetType() == typeof(ElementEvent.RotateEvent))
                            {
                                // Create event instance
                                var rotateEvent = (ElementEvent.RotateEvent)angle.EventList[i];

                                // Create double animation
                                var rotateAnimation = new DoubleAnimation(angle.RotateAngle, rotateEvent.EndAngle,
                                    rotateEvent.Duration)
                                {
                                    AutoReverse = false
                                };

                                // Register in MainWindow
                                MainWindow!.GameGrid.RegisterName(angle.Name, angle);

                                // Add easing function
                                if (rotateEvent.Easing is not null)
                                {
                                    rotateAnimation.EasingFunction = rotateEvent.Easing.GetEasingFunction();
                                }

                                // Binding animation via story board
                                var storyBoard = new Storyboard();
                                storyBoard.Children.Add(rotateAnimation);
                                Storyboard.SetTargetName(rotateAnimation, angle.Name);
                                Storyboard.SetTargetProperty(rotateAnimation,
                                    new(JudgmentAngleDisplay.RotateAngleProperty));
                                storyBoard.Begin(MainWindow.GameGrid);

                                // Remove processed events from the head
                                MainWindow!.GameGrid.UnregisterName(angle.Name);
                                angle.EventList.RemoveAt(0);
                            }
                            else if (angle.EventList[i].GetType() == typeof(ElementEvent.VisibleEvent))
                            {
                                // Create event instance
                                var visibleEvent = (ElementEvent.VisibleEvent)angle.EventList[i];

                                // Set new state
                                angle.Visibility = visibleEvent.Visibility ? Visibility.Visible : Visibility.Hidden;
                                angle.EventList.RemoveAt(0);
                            }
                        }
                    }
                });
                Thread.Sleep(10);
            }
        }
    }
}