using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Arstive.Display.Element
{
    /// <summary>
    /// HoldDisplay.xaml 的交互逻辑
    /// </summary>
    public partial class HoldDisplay : UserControl,INoteDisplay
    {
        public HoldDisplay(int length)
        {
            this.Length = length;
            this.DataContext = this;

            InitializeComponent();
        }

        public int Index { get; set; }

        /// <summary>
        /// Actual length of hold
        /// </summary>
        public int Length
        {
            get => (int)GetValue(LengthProperty);
            set => SetValue(LengthProperty, value);
        }

        /// <summary>
        /// <inheritdoc cref="Length"/>
        /// </summary>
        public static readonly DependencyProperty LengthProperty =
            DependencyProperty.Register(nameof(Length), typeof(int), typeof(HoldDisplay), new PropertyMetadata(20));
    }
}
