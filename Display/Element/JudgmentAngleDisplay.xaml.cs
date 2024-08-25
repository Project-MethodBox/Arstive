using Arstive.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        public double RotateAngle
        {
            get => (double)GetValue(RotateAngleProperty);
            set => SetValue(RotateAngleProperty, value);
        }

        /// <summary>
        /// <inheritdoc cref="RotateAngle"/>
        /// </summary>
        public static readonly DependencyProperty RotateAngleProperty =
            DependencyProperty.Register(nameof(RotateAngle), typeof(double), typeof(JudgmentAngleDisplay), new PropertyMetadata(45d));

        /// <summary>
        /// Index of current judgment angle
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Determine the relative speed of online notes, with
        /// 200 pixels per second as 1
        /// </summary>
        public int Speed;

        /// <summary>
        /// All Notes bound to Judge Angle
        /// </summary>
        public List<Interfaces.NoteBase> NoteList;

        /// <summary>s
        /// All Notes bound to Judge Angle
        /// </summary>
        public List<Interfaces.ElementEventBase> EventList;
    }
}
