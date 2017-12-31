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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace redstone_music
{
    /// <summary>
    /// Welcome.xaml 的交互逻辑
    /// </summary>
    public partial class Welcome : MetroWindow
    {
        public Welcome()
        {
            InitializeComponent();
            //
            this.WindowStyle = WindowStyle.None;
            animation();
        }

        private void animation() {
            Label l = new Label();
            l.Content = "您好!";
            l.Foreground = Brushes.White;
            l.FontSize = 36;
           
            this.canvas.Children.Add(l);
            double startx = 0-l.ActualWidth;
            //MessageBox.Show(startx.ToString());
            l.Margin = new Thickness(0, this.Height * 0.3, 0, 0);
            double deltax = 0;
            double targetx = this.Width/2-50;
            #region TimerAnimation
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(10);
            dt.Tick += delegate
            {
                if (l.Margin.Left < (targetx - startx) / 2) deltax += 0.01;
                if (l.Margin.Left > (targetx - startx) / 2) deltax -= 0.01;
                if (deltax < 0) deltax = 0;
                if (l.Margin.Left > (targetx - startx) / 2 && deltax < 0.1) abc(dt);

                //double deltax = (targetx - l.Margin.Left)/60;
                l.Margin = new Thickness(l.Margin.Left + deltax, this.Height*0.3, 0, 0);
            };
            dt.Start();
            #endregion
            
        }

        public void abc(DispatcherTimer r)
        {
            r.Stop();
            WCP w = new WCP();
            w.Show();
            this.Close();
        }
    }
}
