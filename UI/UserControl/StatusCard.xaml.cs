using Arstive.Model;
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

namespace Arstive.UI.UserControl
{
    /// <summary>
    /// StatusCard.xaml 的交互逻辑
    /// </summary>
    public partial class StatusCard : System.Windows.Controls.UserControl
    {
        public StatusCard()
        {
            InitializeComponent();
        }

        public SolidColorBrush BackgroundColor
        {
            get { return (SolidColorBrush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundColoe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(SolidColorBrush), typeof(StatusCard), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(114,5,14))));


        public string LargeText
        {
            get { return (string)GetValue(LargeTextProperty); }
            set { SetValue(LargeTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LargeText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LargeTextProperty =
            DependencyProperty.Register("LargeText", typeof(string), typeof(StatusCard), new PropertyMetadata("QS"));


        public string SmallText
        {
            get { return (string)GetValue(SmallTextProperty); }
            set { SetValue(SmallTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SmallText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SmallTextProperty =
            DependencyProperty.Register("SmallText", typeof(string), typeof(StatusCard), new PropertyMetadata("0.0%"));
    }
}
