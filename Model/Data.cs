using LiveCharts;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using System.Windows.Media;

namespace Arstive.Model
{
    public class Data
    {
        public class PlayingData(string songName, double accuracy, ChartDifficulty difficulty)
        {
            public string? SongName { get; set; } = songName;

            public double Accuracy { get; set; } = accuracy;

            public ChartDifficulty ChartDifficulty { get; set; } = difficulty;
        }

        public class PlayingDataListManger
        {
            private List<PlayingData> PlayingDatas = new();
            public PlayingData this[int i]
            {
                get => PlayingDatas[i + 1];
                set => PlayingDatas[i + 1] = value;
            }

            public void SetPlayingData(List<PlayingData> playingDatas)
            {
                PlayingDatas.Add(null);
                playingDatas.ForEach(data => PlayingDatas.Add(data));
                PlayingDatas[0] = playingDatas[1];
            }

            public SeriesCollection GenerateSeries()
            {
                // Add to number of 10
                PlayingDatas[0] = PlayingDatas[1];
                if (PlayingDatas.Count < 10)
                {
                    for (int pivot = PlayingDatas.Count; pivot < 11; pivot++)
                        PlayingDatas.Add(new("nil", 0, ChartDifficulty.Hexagon));
                }

                // Generate labels and values
                SeriesCollection SeriesViews =
                [
                    new LineSeries
                    {
                        Values = new ChartValues<double>
                        (from data in PlayingDatas select data.Accuracy),
                         Fill = new SolidColorBrush(Color.FromArgb(100,219,226,232)),
                         StrokeThickness = 5,
                         Stroke = new LinearGradientBrush([
                             new GradientStop(Color.FromArgb(0,0,0,0),0),
                             new GradientStop(Color.FromArgb(100,220,217,249),.15),
                             new GradientStop(Color.FromArgb(0,0,0,0),2)
                         ])
                    }
                ];
                return SeriesViews;
            }
        }
    }
}
