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

namespace Arstive.Display
{
    /// <summary>
    /// JudementAngleDisplay.xaml 的交互逻辑
    /// </summary>
    public partial class JudementAngleDisplay : UserControl
    {
        public JudementAngleDisplay()
        {
            this.DataContext = this;
            InitializeComponent();
        }

        /// <summary>
        /// The key that can be used to determine the binding
        /// of the judgment angle
        /// </summary>
        public Key BindingKey
        {
            get { return (Key)GetValue(BindingKeyProperty); }
            set { SetValue(BindingKeyProperty, value); }
        }


        /// <summary>
        /// <inheritdoc cref="BindingKey"/>
        /// </summary>
        public static readonly DependencyProperty BindingKeyProperty =
            DependencyProperty.Register("BindingKey", typeof(Key), typeof(JudementAngleDisplay), new PropertyMetadata(new KeyConverter().ConvertFrom("W")));

        /// <summary>
        /// Determine the angle of rotation on the left
        /// side of the judgment angle
        /// </summary>
        public int LeftRotateAngle
        {
            get { return (int)GetValue(LeftRotateAngleProperty); }
            set { SetValue(LeftRotateAngleProperty, value); }
        }


        /// <summary>
        /// <inheritdoc cref="LeftRotateAngle"/>
        /// </summary>
        public static readonly DependencyProperty LeftRotateAngleProperty =
            DependencyProperty.Register("LeftRotateAngle", typeof(int), typeof(JudementAngleDisplay), new PropertyMetadata(-45));


        /// <summary>
        /// Determine the angle of rotation on the right
        /// side of the judgment angle
        /// </summary>
        public int RightRotateAngle
        {
            get { return (int)GetValue(RightRotateAngleProperty); }
            set { SetValue(RightRotateAngleProperty, value); }
        }

        /// <summary>
        /// <inheritdoc cref="RightRotateAngle"/>
        /// </summary>
        public static readonly DependencyProperty RightRotateAngleProperty =
            DependencyProperty.Register("LeftRotate", typeof(int), typeof(JudementAngleDisplay), new PropertyMetadata(45));

        /// <summary>
        /// Index of current judgment angle
        /// </summary>
        public int Index { get; set; }
    }
}
