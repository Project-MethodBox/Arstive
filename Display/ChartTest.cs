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
            //var chart = new Chart
            //{
            //    BasicInfo = new()
            //    {
            //        Charter = "Arabidopsis -Overdose-",
            //        Composer = "Fl00t vs. Halv",
            //        ChartDifficultyNumber = 7.1,
            //        ChartDifficultyName = ChartDifficulty.Quadrilateral,
            //        SongName = "Cuvism.wav",
            //        Version = "1.0.0"
            //    },
            //    JudgmentAngles =
            //    [
            //        new(Key.A, 0, 3,
            //            [tap, tap2, tap3],
            //            [new ElementEvent.MoveEvent(3000,
            //                    new(TimeSpan.FromSeconds(3)),
            //                    (-90,-1200),new(Easing.EasingFunctionType.SineEase,EasingMode.EaseInOut))
            //                ,new ElementEvent.RotateEvent(7000,new(TimeSpan.FromSeconds(1)),/*(590, -940)*/-45,new(Easing.EasingFunctionType.BackEase,EasingMode.EaseInOut))
            //            ,new ElementEvent.MoveEvent(9000,new(TimeSpan.FromMilliseconds(1200)),(-90,-700),new(Easing.EasingFunctionType.BackEase,EasingMode.EaseIn)),
            //            new ElementEvent.RotateEvent(9800,new(TimeSpan.FromMilliseconds(1200)),360,new(Easing.EasingFunctionType.BackEase,EasingMode.EaseIn)),new ElementEvent.MoveEvent(9800,new(TimeSpan.FromMilliseconds(1200)),(-0,-0),new(Easing.EasingFunctionType.BackEase,EasingMode.EaseIn))],
            //            (-90, -840)),
            //        new(Key.X, 1, 6, [tap4,tap5,tap6], [new ElementEvent.MoveEvent(8800,TimeSpan.FromMilliseconds(800),(220,-840)),new ElementEvent.MoveEvent(10100, new(TimeSpan.FromMilliseconds(1200)),(1000,1000),new(Easing.EasingFunctionType.BackEase,EasingMode.EaseIn)), new ElementEvent.RotateEvent(10100, new(TimeSpan.FromSeconds(1)),/*(590, -940)*/-360, new(Easing.EasingFunctionType.BackEase, EasingMode.EaseInOut))], (-90, -2340)),
            //        new(Key.J, 2, 6, [tap7,tap9], [new ElementEvent.RotateEvent(0, new(TimeSpan.FromMilliseconds(1200)), 360, new(Easing.EasingFunctionType.BackEase, EasingMode.EaseIn)),new ElementEvent.MoveEvent(9800,TimeSpan.FromMilliseconds(800),(220,-940)),new ElementEvent.MoveEvent(11500,TimeSpan.FromMilliseconds(800),(888,-0))], (-90, 0)),
            //        new(Key.L, 3, 6, [tap8,tap10,tap11,tap12,tap13,tap14], [new ElementEvent.RotateEvent(0, new(TimeSpan.FromMilliseconds(1200)), 360, new(Easing.EasingFunctionType.BackEase, EasingMode.EaseIn)), new ElementEvent.MoveEvent(9600,TimeSpan.FromMilliseconds(800),(-300,-940))], (-90, 0)),
            //    ],
            //    FreeNotes = []
            //};
            
            // Chart.Shared = chart;
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