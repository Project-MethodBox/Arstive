using System.Windows;
using Arstive.Controller;

namespace Arstive
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
            GameController.MainWindow = this;
        }

        public void AddControl(UIElement element)
        {
            this.GameGrid.Children.Add(element);
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            GameController.LoadChart("", AddControl);
        }
    }
}