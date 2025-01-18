using System.IO;
using System.Text.Json;
using Arstive.Model;
using System.Windows.Input;
using System.Windows.Media.Animation;
using static Arstive.Model.Notes;

namespace Arstive.Display
{
    internal class ChartTest
    {
        internal static void LoadTest()
        {
            var tap = new Notes.Tap();
            tap.HitTime = 1000;
            tap.JudgmentAngleIndex = 0;
            tap.Index = 1;
            var tap2 = new Notes.Hold();
            tap2.HitTime = 3930;
            tap2.EndTime = 4500;
            tap2.JudgmentAngleIndex = 0;
            tap2.Index = 2;
            var tap3 = new Notes.Tap();
            tap3.HitTime = 9200;
            tap3.JudgmentAngleIndex = 0;
            tap3.Index = 3;
            var tap4 = new Notes.Drag();
            tap4.HitTime = 10300;
            tap4.JudgmentAngleIndex = 1;
            tap4.Index = 4;
            var tap5 = new Notes.Drag();
            tap5.HitTime = 10450;
            tap5.JudgmentAngleIndex = 1;
            tap5.Index = 5;
            var tap6 = new Notes.Drag();
            tap6.HitTime = 10600;
            tap6.JudgmentAngleIndex = 1;
            tap6.Index = 6;
            var tap7 = new Notes.Tap();
            tap7.HitTime = 11200;
            tap7.JudgmentAngleIndex = 2;
            tap7.Index = 7;
            var tap8 = new Notes.Tap();
            tap8.HitTime = 11300;
            tap8.JudgmentAngleIndex = 3;
            tap8.Index = 8;
            var tap9 = new Notes.Tap();
            tap9.HitTime = 11400;
            tap9.JudgmentAngleIndex = 2;
            tap9.Index = 9;
            var tap10 = new Notes.Tap();
            tap10.HitTime = 11500;
            tap10.JudgmentAngleIndex = 3;
            tap10.Index = 10;
            var tap11 = new Notes.Tap();
            tap11.HitTime = 12000;
            tap11.JudgmentAngleIndex = 3;
            tap11.Index = 11;
            var tap12 = new Notes.Tap();
            tap12.HitTime = 12150;
            tap12.JudgmentAngleIndex = 3;
            tap12.Index = 12;
            var tap13 = new Notes.Tap();
            tap13.HitTime = 12300;
            tap13.JudgmentAngleIndex = 3;
            tap13.Index = 13;
            var tap14 = new Notes.Hold();
            tap14.HitTime = 13500;
            tap14.EndTime = 16500;
            tap14.JudgmentAngleIndex = 3;
            tap14.Index = 14;

            var move1 = new ElementEvent.MoveEvent();
            move1.Duration = new(TimeSpan.FromSeconds(3));
            move1.StartTime = 3000;
            move1.EventType = ElementEvent.ElementEventType.Move;
            move1.Destination = (-90, -1200);
            var easing1 = new Easing();
            easing1.EasingFunction = Easing.EasingFunctionType.SineEase;
            easing1.EasingMode = EasingMode.EaseInOut;
            move1.Easing = easing1;
            var rotate1 = new ElementEvent.RotateEvent();
            rotate1.StartTime = 7000;
            rotate1.EventType = ElementEvent.ElementEventType.Rotate;
            rotate1.Duration = TimeSpan.FromSeconds(1);
            rotate1.EndAngle = -45;
            var easing2 = new Easing();
            easing2.EasingFunction = Easing.EasingFunctionType.BackEase;
            easing2.EasingMode = EasingMode.EaseInOut;
            rotate1.Easing = easing2;
            var move2 = new ElementEvent.MoveEvent();
            move2.Duration = new(TimeSpan.FromMilliseconds(1200));
            move2.StartTime = 9000;
            move2.Destination = (-90, -700);
            move2.Easing = easing2;
            var rotate2 = new ElementEvent.RotateEvent();
            rotate2.StartTime = 9800;
            rotate2.EventType = ElementEvent.ElementEventType.Rotate;
            rotate2.Duration = TimeSpan.FromSeconds(1);
            rotate2.EndAngle = 360;
            rotate2.Duration = TimeSpan.FromMilliseconds(1200);
            var easing3 = new Easing();
            easing3.EasingFunction = Easing.EasingFunctionType.BackEase;
            easing3.EasingMode = EasingMode.EaseIn;
            rotate2.Easing = easing3;
            var move3 = new ElementEvent.MoveEvent();
            move3.StartTime = 9800;
            move3.Duration = TimeSpan.FromMilliseconds(1200);
            move3.Destination = (-0, -0);
            move3.Easing = easing3;
            var move4 = new ElementEvent.MoveEvent();
            move4.StartTime = 8800;
            move4.Duration = TimeSpan.FromMilliseconds(800);
            move4.Destination = (220, -840);
            var move5 = new ElementEvent.MoveEvent();
            move5.StartTime = 10100;
            move5.Duration = TimeSpan.FromMilliseconds(1200);
            move5.Destination = (1000, 1000);
            move5.Easing = easing1;
            var rotate3 = new ElementEvent.RotateEvent();
            rotate3.StartTime = 10100;
            rotate3.Duration = TimeSpan.FromSeconds(1);
            rotate3.EndAngle = -360;
            rotate3.EventType = ElementEvent.ElementEventType.Rotate;
            rotate3.Easing = easing2;
            var rotate4 = new ElementEvent.RotateEvent();
            rotate4.StartTime = 0;
            rotate4.Duration = TimeSpan.FromMilliseconds(1200);
            rotate4.EndAngle = 360;
            rotate4.EventType = ElementEvent.ElementEventType.Rotate;
            rotate4.Easing = easing3;
            var move6 = new ElementEvent.MoveEvent();
            move6.StartTime = 9800;
            move6.Duration = TimeSpan.FromMilliseconds(800);
            move6.Destination = (220, -940);
            var move7 = new ElementEvent.MoveEvent();
            move7.StartTime = 11500;
            move7.Duration = TimeSpan.FromMilliseconds(800);
            move7.Destination = (888, -0);
            var rotate5 = new ElementEvent.RotateEvent();
            rotate5.StartTime = 0;
            rotate5.Duration = TimeSpan.FromMilliseconds(1200);
            rotate5.EndAngle = 360;
            rotate5.EventType = ElementEvent.ElementEventType.Rotate;
            rotate5.Easing = easing3;
            var move8 = new ElementEvent.MoveEvent();
            move8.StartTime = 8600;
            move8.Duration = TimeSpan.FromMilliseconds(800);
            move8.Destination = (-300, -940);

            var chart = new Chart
            {
                BasicInfo = new()
                {
                    Charter = "Arabidopsis -Overdose-",
                    Composer = "Fl00t vs. Halv",
                    ChartDifficultyNumber = 7.1,
                    ChartDifficultyName = ChartDifficulty.Quadrilateral,
                    SongName = "Cuvism.wav",
                    Version = "0.3.0"
                },
                JudgmentAngles =
                [
                    new(Key.A, 0, 3,
                        [tap, tap2, tap3],
                        [move1, rotate1,move2, rotate2,move3],
                        (-90, -840)),
                    new(Key.X, 1, 6, [tap4,tap5,tap6], 
                        [move4, move5, rotate3],(-90, -2340)),
                    new(Key.J, 2, 6, [tap7,tap9],
                        [rotate4, move6, move7], (-90, 0)),
                    new(Key.L, 3, 6, [tap8,tap10,tap11,tap12,tap13,tap14],
                        [rotate5, move8], (-90, 0)),
                ],
                FreeNotes = []
            };

            Chart.Shared = chart;
            Chart.Save("cuvism.json");
        }

        internal static void LoadTest2()
        {
            var flick = new Flick();
            flick.StartKey = Key.J;
            flick.EndKey = Key.O;
            flick.StartTime = 3000;
            flick.HitTime = 5000;
            flick.NoteMargin = (0, 1);
            var chart = new Chart
            {
                BasicInfo = new()
                {
                    Charter = "Arabidopsis -Overdose-",
                    Composer = "Fl00t vs. Halv",
                    ChartDifficultyNumber = 7.1,
                    ChartDifficultyName = ChartDifficulty.Quadrilateral,
                    SongName = "Cuvism.wav",
                    Version = "1.0.0"
                },
                JudgmentAngles = [],
                FreeNotes = [flick]
            };
            Chart.Shared =  chart;
        }
    }
}