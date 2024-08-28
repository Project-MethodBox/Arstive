using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Arstive.Display.Element
{
    /// <summary>
    /// FlickDisplay.xaml 的交互逻辑
    /// </summary>
    public partial class FlickDisplay : UserControl
    {
        public FlickDisplay()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        /// <summary>
        /// The start key for Flick sliding
        /// </summary>
        public Key BindingStartKey
        {
            get => (Key)GetValue(BindingStartKeyProperty);
            set => SetValue(BindingStartKeyProperty, value);
        }

        /// <summary>
        /// <inheritdoc cref="BindingStartKey"/>
        /// </summary>
        public static readonly DependencyProperty BindingStartKeyProperty =
            DependencyProperty.Register(nameof(BindingStartKey), typeof(Key), typeof(FlickDisplay), new PropertyMetadata(new KeyConverter().ConvertFrom("W")));

        /// <summary>
        /// The end key for Flick sliding
        /// </summary>
        public Key BindingEndKey
        {
            get => (Key)GetValue(BindingEndKeyProperty);
            set => SetValue(BindingEndKeyProperty, value);
        }

        /// <summary>
        /// <inheritdoc cref="BindingEndKey"/>
        /// </summary>
        public static readonly DependencyProperty BindingEndKeyProperty =
            DependencyProperty.Register(nameof(BindingEndKey), typeof(Key), typeof(FlickDisplay), new PropertyMetadata(new KeyConverter().ConvertFrom("D")));


        /// <summary>
        /// Determine the angle of rotation side of note
        /// </summary>
        public double RotateAngle
        {
            get => (double)GetValue(RotateAngleProperty);
            set => SetValue(RotateAngleProperty, value);
        }

        /// <summary>
        /// <inheritdoc cref="RotateAngle"/>
        /// </summary>
        public static readonly DependencyProperty RotateAngleProperty =
            DependencyProperty.Register(nameof(RotateAngle), typeof(double), typeof(FlickDisplay), new PropertyMetadata(0d));

    }
}
