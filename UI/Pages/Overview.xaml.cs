using Arstive.Model;
using System.Windows;
using static Arstive.Model.Data;

namespace Arstive.UI.Pages
{
    /// <summary>
    /// Overview.xaml 的交互逻辑
    /// </summary>
    public partial class Overview : System.Windows.Controls.UserControl
    {
        public PlayingDataListManger PlayingDatasManger;

        public Overview(List<Data.PlayingData> playingDatas)
        {
            this.DataContext = PlayingDatasManger;
            InitializeComponent();

            PlayingDatasManger = new();
            PlayingDatasManger.SetPlayingData(playingDatas);
            AccuracyChart.Series = PlayingDatasManger.GenerateSeries();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Display corrsponding dataset
            // Draw chart
        }
    }
}
