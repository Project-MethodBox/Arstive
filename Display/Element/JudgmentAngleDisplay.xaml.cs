using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Arstive.Model;

namespace Arstive.Display.Element
{
    /// <summary>
    /// JudgmentAngleDisplay.xaml 的交互逻辑
    /// </summary>
    public partial class JudgmentAngleDisplay : UserControl
    {
        public JudgmentAngleDisplay()
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
            get => (Key)GetValue(BindingKeyProperty);
            set => SetValue(BindingKeyProperty, value);
        }


        /// <summary>
        /// <inheritdoc cref="BindingKey"/>
        /// </summary>
        public static readonly DependencyProperty BindingKeyProperty =
            DependencyProperty.Register(nameof(BindingKey), typeof(Key), typeof(JudgmentAngleDisplay), new PropertyMetadata(new KeyConverter().ConvertFrom("W")));

        /// <summary>
        /// Determine the angle of rotation
        /// side of the judgment angle
        /// </summary>
        public int RotateAngle
        {
            get => (int)GetValue(RotateAngleProperty);
            set => SetValue(RotateAngleProperty, value);
        }

        /// <summary>
        /// <inheritdoc cref="RotateAngle"/>
        /// </summary>
        public static readonly DependencyProperty RotateAngleProperty =
            DependencyProperty.Register("RotateAngle", typeof(int), typeof(JudgmentAngleDisplay), new PropertyMetadata(45));

        /// <summary>
        /// Index of current judgment angle
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Determine the relative speed of online notes, with
        /// 200 pixels per second as 1
        /// </summary>
        public int Speed;

        public List<Interfaces.NoteBase> NoteLists;
    }
}
