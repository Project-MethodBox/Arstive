using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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