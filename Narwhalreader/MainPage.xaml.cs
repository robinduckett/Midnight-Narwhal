using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using System.Collections.ObjectModel;

namespace Narwhalreader
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor

        public MainPage()
        {
            InitializeComponent();
            
            // Set the data context of the listbox control to the sample data
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            messaging_header.Foreground = new SolidColorBrush() {
                Color = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF)
            };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            messaging_header.Foreground = new SolidColorBrush()
            {
                Color = Color.FromArgb(0xFF, 0xFF, 0x45, 0)
            };
        }
    }
}