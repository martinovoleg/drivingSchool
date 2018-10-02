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
using System.Windows.Shapes;

namespace Driving_School
{
    /// <summary>
    /// Логика взаимодействия для MessageAutoClose.xaml
    /// </summary>
    public partial class MessageAutoClose : Window
    {
        public MessageAutoClose(string message)
        {
            InitializeComponent();
            Message.Text = message;
        }
    }
}