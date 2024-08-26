using Arstive.Model;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Arstive.Display
{
    internal class ChartTest
    {
        internal static Chart LoadTest()
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
                JudgmentAngles =
                [
                    new(Key.A, 0, 3,
                        [tap, tap2, tap3],
                        [new ElementEvent.MoveEvent(3000,
                                new(TimeSpan.FromSeconds(3)),
                                (-90,-1200),new(Easing.EasingFunctionType.SineEase,EasingMode.EaseInOut))
                            ,new ElementEvent.RotateEvent(7000,new(TimeSpan.FromSeconds(1)),/*(590, -940)*/-45,new(Easing.EasingFunctionType.SineEase,EasingMode.EaseInOut))],
                        (-90, -840)),
                    new(Key.X, 1, 6, [tap4,tap5,tap6], [new ElementEvent.MoveEvent(8800,TimeSpan.FromMilliseconds(800),(220,-840))], (-90, -2340))
                ]
            };
            return chart;
        }
    }
}
