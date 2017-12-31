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
    public class m_Postion
    {
        public Label l1 = new Label();
        public Label l2 = new Label();
        MainWindow m;

        public m_Postion(MainWindow _m)
        {
            this.m = _m;
            //set backGround 
            setBackGround(null, null);

            l2.Width = 1;l2.Height = m.Height-150;
            l1.Width = 10;l1.Height = 10;
            l1.Margin = new Thickness(-5,65, 0, 0);
            l2.Margin = new Thickness(0, 75, 0, 0);
            m.canvas.Children.Add(l1);
            m.canvas.Children.Add(l2);
            
        }


        public void setBackGround(Brushes b1,Brushes b2)
        {
            l1.Background = Brushes.LightBlue;
            l2.Background = Brushes.Red;

        }
    }
}
