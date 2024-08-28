using Arstive.Display;
using Arstive.Display.Element;
using Arstive.Model;
using System.ComponentModel;
using System.Diagnostics;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Arstive.Model.ObjectPool;

namespace Arstive.Controller
{
    public class GameController
    {
        /// <summary>
        /// Binding static property to the main window
        /// </summary>
        public static event EventHandler<PropertyChangedEventArgs>?
            StaticPropertyChanged;

        /// <summary>
        /// Synchronized the calling from different threads
        /// </summary>
        private static Dispatcher? _dispather;

        internal static MainWindow? MainWindow;
        private static int _score = 0;
        private static volatile int _internalTick;

        private static int Tick
        {
            get => _internalTick;
            set
            {
                if (value == 0)
                {
                    var player = new SoundPlayer(Chart.Shared.BasicInfo!.SongName!);
                    player.Play();
                }
                _internalTick = value;
            }
        }

        /// <summary>
        /// When any judgment entity exists, block to avoid
        /// entering the end of the game
        /// </summary>
        private static Barrier? _barrier;

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

            ChartTest.LoadTest();
            var chart = Chart.Shared;

            // Load chart to window
            var indexKeyParis = new Dictionary<int, Key>();

            // Initialize barrier
            // Usage list:
            // n x judgment thread (1 <= n <= 26)
            // 1 x counting thread
            // 1 x controller
            // 1 x outer judgment thread
            _barrier = new(chart.JudgmentAngles.Count + 3);

            // For buffer system
            int earliestTime = 0;

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

                // Calculate earliest time
                var flootTime = 1410 / (double)(200 * (judgmentAngle.Speed)) * 1000;
                if (earliestTime >= judgmentAngle.NoteLists![0].HitTime - (int)(flootTime))
                {
                    // Negative
                    earliestTime = judgmentAngle.NoteLists[0].HitTime - (int)(flootTime);
                }

                // Start judge
                ThreadPool.QueueUserWorkItem(JudgeBindingNote, judgmentAngleInstance);
                addControl(judgmentAngleInstance);
            }

            // Judge free notes
            ThreadPool.QueueUserWorkItem(JudgeFreeNote, chart.FreeNotes);

            // Create elapsed buffer
            Tick = earliestTime;

            // Start time counting
            Task.Run(CountTime);

            // Note:Temp
            MainWindow!.Song.Text = chart.BasicInfo!.SongName!.Split(".")[0];

            // Start game!
            _barrier.SignalAndWait();
        }

        internal static void JudgeBindingNote(object? judgmentAngle)
        {
            // Notify that current angle is running
            _barrier!.SignalAndWait();
            _barrier!.AddParticipant();

            // Prepare instance and timer
            var angle = (JudgmentAngleDisplay)judgmentAngle!;

            // Can determine Drag notes
            var pressed = false;
            // Can determine Tap notes
            var tap = false;

            // The notes that have already been created are used to determine
            var judgmentList = new List<Interfaces.BindNote>();

            // Down time
            var flootTime = 1410 / (double)(200 * (angle.Speed));

            // Capture dispather for thread synchronization
            _dispather ??= Application.Current.Dispatcher;

            // Enter judgment
            while (true)
            {
                // Count time elapsed
                _dispather.Invoke(() =>
                {
                    // Switch if specific key is pressed
                    if (Keyboard.IsKeyDown(angle.BindingKey) && !tap)
                    {
                        tap = !pressed;
                        pressed = true;
                    }

                    else if (Keyboard.IsKeyUp(angle.BindingKey) && pressed)
                    {
                        pressed = false;
                    }

                    if (Keyboard.IsKeyUp(angle.BindingKey) && tap)
                        tap = false;

                    // Note animation
                    for (int i = 0; i < angle.NoteList.Count; i++)
                    {
                        var note = angle.NoteList[i];

                        // Handling note end events
                        double actualFlootTime = flootTime;
                        if (note.GetType() == typeof(Notes.Hold))
                        {
                            actualFlootTime += 320 / (double)(200 * (angle.Speed));
                        }

                        int deltaAnimation = note.HitTime - Tick;

                        // Need create
                        if (deltaAnimation <= actualFlootTime * 1000)
                        {
                            // Create note instance
                            // (Converter works in InitializeComponent)
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
                                Notes.Hold hold => new HoldDisplay(((hold.EndTime - hold.HitTime) * (200 * (angle.Speed)))/1000 + 40)
                                {
                                    Index = note.Index,
                                    Name = $"holdInstance{angle.Index}T{note.Index}",
                                    Margin = new(0, -1360, 0, 0)
                                },
                                _ => null
                            };

                            angle.MainPanel.RegisterName(noteInstance!.Name, noteInstance);

                            // Create double animation
                            var startPosition = -1360;
                            var endPosition = -320;

                            if (note.GetType() == typeof(Notes.Hold))
                            {
                                startPosition -= (((Notes.Hold)note).EndTime - note.HitTime) *
                                    (200 * (angle.Speed)) / 1000;
                                endPosition = -0;
                            }

                            var downAnimation = new ThicknessAnimation(new(0, startPosition, 0, 0),
                                new(-0, endPosition, 0, 0),
                                new Duration(TimeSpan.FromMilliseconds(actualFlootTime * 1000)))
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
                            int delta = judgmentList[i].HitTime - Tick;
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
                                //angle.MainPanel.Children.RemoveAt(1);
                                //judgmentList.RemoveAt(i);
                            }
                        }
                        else if (judgmentList[i].GetType() == typeof(Notes.Drag) && pressed)
                        {
                            var delta = judgmentList[i].HitTime - Tick - 140;
                            if (delta is <= 120 and >= -120)
                            {
                                // Only have judgment perfect
                                System.Media.SystemSounds.Asterisk.Play();
                                Score += 2000;
                                angle.MainPanel.Children.RemoveAt(1);
                                judgmentList.RemoveAt(i);
                            }
                        }
                        else if (judgmentList[i].GetType() == typeof(Notes.Hold))
                        {
                            var hold = (Notes.Hold)judgmentList[i];

                            if ((Tick > hold.HitTime && Tick < hold.EndTime) && !pressed  && hold.IsJudgment)
                            {
                                // Judgment failed, immediately recycle
                                Score -= 1000;
                                hold.IsJudgment = false;
                                break;
                            }

                            if (Tick >= hold.EndTime && pressed && hold.IsJudgment)
                            {
                                // Judgment succeed, immediately recycle
                                Score += 2000;
                                hold.IsJudgment = false;
                                break;
                            }
                        }

                        if (i >= judgmentList.Count)
                        {
                            break;
                        }

                        // We don't know what this,but it can running normally,
                        // In a word,it's only a recycle algorithm :D
                        if (((judgmentList[i].GetType() == typeof(Notes.Tap)
                            || judgmentList[i].GetType() == typeof(Notes.Drag))
                             && judgmentList[i].HitTime - Tick - 140 <= -420)||
                            (judgmentList[i].GetType() == typeof(Notes.Hold)) 
                            &&((Notes.Hold)judgmentList[i]).EndTime
                            - Tick - 140 <= -420)
                        {
                            // Recycle note
                            angle.MainPanel.Children.RemoveAt(1);
                            judgmentList.RemoveAt(i);
                        }
                    }

                    // Enter animation
                    for (int i = 0; i < angle.EventList.Count; i++)
                    {
                        if (angle.EventList[i].StartTime <= Tick)
                        {
                            if (angle.EventList[i].GetType() == typeof(ElementEvent.MoveEvent))
                            {
                                // Create event instance
                                var moveEvent = (ElementEvent.MoveEvent)angle.EventList[i];

                                // Create thickness animation
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

        internal static void JudgeFreeNote(object? freeNoteListState)
        {
            // TODO:Buffering pool


            // Block and synchronous
            _barrier.SignalAndWait();

            // All free note judges in the current thread
            // Create instance
            List<Interfaces.FreeNote> freeNoteList = (List<Interfaces.FreeNote>)freeNoteListState!;

            // Create instance
            List<Interfaces.FreeNote> freeNoteJudgeList = (List<Interfaces.FreeNote>)freeNoteListState!;

            for (int i = 0; i < freeNoteList.Count; i++)
            {
                // Calculate time
                // Think buffering time
                (int actualStartTime, int actualHitTime) =
                    (freeNoteList[i].StartTime + 500, freeNoteList[i].HitTime + 500);

                // Judging time
                var runningTime = actualHitTime - actualStartTime;

                // Enter animation
                if (runningTime >= Tick - actualStartTime)
                {
                    // Create instance
                    if (freeNoteList[i].GetType() == typeof(Notes.Flick))
                    {
                        var flick = (Notes.Flick)freeNoteList[i];
                        // Tree : FlickGrid => LayoutStack => Block(Animate caller)
                        var flickInstance = new FlickDisplay
                        {
                            BindingStartKey = flick.StartKey,
                            BindingEndKey = flick.EndKey,
                        };

                        // Create double animation
                        var lengthAnimation = new DoubleAnimation(0, 250,
                            TimeSpan.FromMilliseconds(runningTime))
                        {
                            AutoReverse = false
                        };

                        // Binding animation via story board
                        var storyBoard = new Storyboard();
                        storyBoard.Children.Add(lengthAnimation);
                        Storyboard.SetTargetName(lengthAnimation, flickInstance.Block.Name);
                        Storyboard.SetTargetProperty(lengthAnimation,
                            new(JudgmentAngleDisplay.WidthProperty));
                        storyBoard.Begin(flickInstance.Block);

                        // Add delta time
                        freeNoteList[i].StartTime = actualStartTime;
                        freeNoteList[i].HitTime = actualHitTime;
                        freeNoteJudgeList.Add(freeNoteList[i]);

                        // Delete it
                        freeNoteList.RemoveAt(i);
                    }
                }
            }

            // Enter judgment
            for (int i = 0; i < freeNoteJudgeList.Count; i++)
            {
                if (freeNoteJudgeList[i].GetType() == typeof(Notes.Flick))
                {
                    var flick = (Notes.Flick)freeNoteJudgeList[i];

                    if (Tick - flick.StartTime is >= -120 and <= 120 && !flick.IsJudgment)
                    {
                        flick.IsJudgment = Keyboard.IsKeyDown(flick.StartKey);
                    } 
                    if (Tick - flick.HitTime is >= -120 and <= 120 && flick.IsJudgment && Keyboard.IsKeyDown(flick.EndKey))
                    {
                        Score += 2000;
                    }
                }
            }

        }

        static async void CountTime()
        {
            // Ensure that the timing and judgment threads run simultaneously
            _barrier!.SignalAndWait();

            // Record start time
            var startTime = DateTime.Now;

            // Counting time elapsed asynchronous
            var periodicTimer = new PeriodicTimer(TimeSpan.FromMilliseconds(10));

            while (await periodicTimer.WaitForNextTickAsync())
            {
                Tick += (int)(DateTime.Now - startTime).TotalMilliseconds;
            }
        }
    }
}