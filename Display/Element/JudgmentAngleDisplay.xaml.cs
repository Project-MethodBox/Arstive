﻿using System.Windows;
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
        /// Determine the angle of rotation on the left
        /// side of the judgment angle
        /// </summary>
        public int LeftRotateAngle
        {
            get => (int)GetValue(LeftRotateAngleProperty);
            set => SetValue(LeftRotateAngleProperty, value);
        }


        /// <summary>
        /// <inheritdoc cref="LeftRotateAngle"/>
        /// </summary>
        public static readonly DependencyProperty LeftRotateAngleProperty =
            DependencyProperty.Register(nameof(LeftRotateAngle), typeof(int), typeof(JudgmentAngleDisplay), new PropertyMetadata(-45));


        /// <summary>
        /// Determine the angle of rotation on the right
        /// side of the judgment angle
        /// </summary>
        public int RightRotateAngle
        {
            get => (int)GetValue(RightRotateAngleProperty);
            set => SetValue(RightRotateAngleProperty, value);
        }

        /// <summary>
        /// <inheritdoc cref="RightRotateAngle"/>
        /// </summary>
        public static readonly DependencyProperty RightRotateAngleProperty =
            DependencyProperty.Register("LeftRotate", typeof(int), typeof(JudgmentAngleDisplay), new PropertyMetadata(45));

        /// <summary>
        /// Index of current judgment angle
        /// </summary>
        public int Index { get; set; }
    }
}