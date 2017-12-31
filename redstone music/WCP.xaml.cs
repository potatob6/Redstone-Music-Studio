using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace redstone_music
{
    /// <summary>
    /// WCP.xaml 的交互逻辑
    /// </summary>
    public partial class WCP : MetroWindow
    {
        public WCP()
        {
            InitializeComponent();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(1);
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
        }

        private void Rectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            Rectangle r = sender as Rectangle;
            r.Fill = Brushes.LightBlue;
            
        }

        private void Rectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            Rectangle r = sender as Rectangle;
            r.Fill = Brushes.White;

        }
    }
}
