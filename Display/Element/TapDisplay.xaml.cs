﻿using System;
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
    /// TapDisplay.xaml 的交互逻辑
    /// </summary>
    public partial class TapDisplay : UserControl, INoteDisplay
    {
        public TapDisplay()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The Index of note relative to the current judgment angle
        /// </summary>
        public int Index { get; set; }
    }
}