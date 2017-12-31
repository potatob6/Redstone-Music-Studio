using System;
using System.Windows.Threading;
using System.Windows.Controls;

namespace redstone_music
{
    /// <summary>
    /// 播放类
    /// </summary>
    public class Play
    {
        
        public bool canPlay = false;
        public double canvasWidth = 0.0;
        public double nowLeft = 0.0;
        public MainWindow m;
        
        public DispatcherTimer timer = new DispatcherTimer();

        public Play(MainWindow m)
        {
            this.m = m;
            timer.Interval = TimeSpan.FromSeconds(0.06);
            timer.Tick += new EventHandler(tickfor_zzSaon);
            timer.Start();
        }

        /// <summary>
        /// 智障saon 辣鸡saon 煞笔saon 弱智saon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void tickfor_zzSaon(object sender,EventArgs e)
        {
            if (canPlay)
                m.PlayNode.Header = "停止播放[Space]";
            else {
                m.PlayNode.Header = "播放[Space]";
            }
                if (canPlay)
                {

                    double i = nowLeft+=25;
                    if (i >= m.pointerSetter.Width)
                    {
                        canPlay = false;
                        nowLeft--;
                        return;
                    }
                    m.pointer.l1.Margin = new System.Windows.Thickness(i - 5, 65, 0, 0);
                    m.pointer.l2.Margin = new System.Windows.Thickness(i, 75, 0, 0);
                    double b = (i - (i % 25)) / 25;
                    int c = (int)((i % 25) / 25 * 10);
                    nowLeft = m.pointer.l2.Margin.Left;

                    m.label1.Content = b + ":" + c;

                    //播放
                    foreach (var btn in m.canvas.Children)
                    {
                        if (btn is Button && m.pointer.l2.Margin.Left == (btn as Button).Margin.Left)
                        {
                            Button Nodebtn = btn as Button;
                            string SoundsPath = string.Format("{0}.{1}", "sounds\\" + Nodebtn.Content, "wav");
                            PlaySound ps = new PlaySound();
                            ps.Play(SoundsPath, true, 100);
                        }
                    }
                }
        }
    }
}
