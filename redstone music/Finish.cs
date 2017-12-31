using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace redstone_music
{
    class Finish
    {
        public Button btn;
        public MainWindow m;

        public void retoFinish(object sender, EventArgs e)
        {
            Button mobj = sender as Button;
            int dis = m.doNot(btn.Margin.Top);
            if (dis > 8)
            {
                m.canvas.Children.Remove(btn);
                m.selectKeys.Remove(btn);

                return;
            }

            if (btn.Margin.Left< 0)
            {
                m.selectKeys.Remove(btn);
                m.canvas.Children.Remove(btn);
            }
                        
            m.pitch(btn);
            m.cover(btn);
            m.findTheMax();
        }
    }
}
