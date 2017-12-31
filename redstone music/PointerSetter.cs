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
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Windows.Media.Animation;
using System.Xml;
using MahApps.Metro.Controls.Dialogs;
namespace redstone_music
{
    public class PointerSetter:Label
    {
       public  MainWindow m;
       public PointerSetter(MainWindow m)
        {
            this.Background = Brushes.Orange;
            this.Opacity = 0.7;
            this.m = m;
            this.Margin = new Thickness(0, 65, 0, 0);
            this.Width = m.canvas.Width*2;
            this.Height = 10;
            m.canvas.Children.Add(this);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(left);
            this.MouseMove += new MouseEventHandler(move);
            

        }

        public bool isMoveSetter = false;

        public void left(object sender,MouseButtonEventArgs e)
        {
            isMoveSetter = true;
            
        }
        
        public void move(object sender,MouseEventArgs e)
        {
            if (isMoveSetter)
            {
                Point p = e.GetPosition(this);
                int x = (int)(p.X / 25);
                m.pointer.l1.Margin = new Thickness(x * 25 - 5, 65, 0, 0);
                m.pointer.l2.Margin = new Thickness(x * 25, 75, 0, 0);
                m.play.nowLeft = m.pointer.l2.Margin.Left;

                double b = (p.X - (p.X % 25)) / 25;
                //int c = (int)((p.X % 25) / 25 * 10);

                m.label1.Content = b + ":0" ;
            }
            
        }
    }
}
