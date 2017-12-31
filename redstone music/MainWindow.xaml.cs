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
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        PlaySound ps = new PlaySound();
        //路径[你这什么鬼。。。路径不是固定的啊fuck]
        //public static string path = "C:\\Users\\Administrator\\Redstone MusicProject\\";
        //显示行用的按钮
        Label lb;

        //当前指针
        public m_Postion pointer;
        //当前指针控制器
        public PointerSetter pointerSetter;
        //播放类
        public Play play;

        public MainWindow()
        {
            InitializeComponent();
            //WCP wcp = new WCP();
            //wcp.Show();
            //this.Close();
            button1.Background = new SolidColorBrush(Color.FromArgb(100, 89, 24, 255));

            //指针控制器
            PointerSetter ps = new PointerSetter(this);
            this.pointerSetter = ps;


            m_Postion p = new m_Postion(this);
            this.pointer = p;

            play = new Play(this);
            //findTheMax1();
            //画格线
            for (int i = 0; i <= canvas.Width+1000; i += 25)
            {
                Label l = new Label();
                l.Background = new SolidColorBrush(Color.FromArgb(40, 255, 255, 255));
                //l.Background = Brushes.LightBlue;
                l.Width = 1;l.Height = canvas.Height-100;
                l.Margin = new Thickness(i, 75, 0, 0);
                //MessageBox.Show(l.Margin.Left.ToString());
                if ((l.Margin.Left/25) % 10 == 0)
                {
                    l.Background = Brushes.Purple;
                }
                canvas.Children.Add(l);

                Label l1 = new Label();
                l1.FontSize = 8;
                l1.Content = (l.Margin.Left / 25);
                if (int.Parse(l1.Content.ToString()) % 10 == 0)
                {
                    l1.Foreground = Brushes.Purple;
                }
                l1.Background = null;l1.Foreground = Brushes.White;
                l1.Margin = new Thickness(l.Margin.Left - l1.ActualWidth / 2, 50, 0, 0);
                canvas.Children.Add(l1);
            }
        }
        

        public void readButton(int x,int y)
        {
            Button btn = new Button();
            btn.Margin = new Thickness(x, y, 0, 0);
            btn.Height = 25;btn.Width = 25;
            btn.Background = null;
            pitch(btn);
            cover(btn);
            findTheMax();
        }


        //目前最大的位置
        public static double max = 803;
        /// <summary>
        /// 长宽自动对齐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.label.Width = this.Width+30;
            scrollBar.Width = this.Width-75;
            scrollBar.Margin = new Thickness(75, this.Height-30 - scrollBar.Height, 0, 0);
            Canvas.SetLeft(this.label1, 0);
         
        }
        /// <summary>
        /// 面页滑动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            scrollBar.Maximum = this.canvas.Width;
            canvas.Margin = new Thickness(canvas.Margin.Left - (e.NewValue - e.OldValue), canvas.Margin.Top,0,0);
            gaol.Margin = canvas.Margin;
        }

        bool helping = false;

        //刷子移动状态
        bool brushStartMove = false;
        
        //新的音符键
        public void MetroWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {

            
            
                if (pencil)
                {
                    if ((e.LeftButton == MouseButtonState.Pressed))
                    {

                        if (retoZ == true)
                        {
                            //睾亮取消
                            retoZ = false;
                            clearRetoLab();
                            center = null;
                            foreach (object obj in canvas.Children)
                            {
                                if (obj is Button)
                                {
                                    ((Button)obj).Background = null;
                                }
                            }
                            selectKeys.Clear();
                            return;
                        }

                        if (helping || leftCtrl == true)
                        {
                            return;
                        }



                        Point p = e.GetPosition(this.grid);
                        Button btn = new Button();
                        btn.Background = Brushes.Gray;
                        btn.Foreground = Brushes.Orange;
                        btn.BorderBrush = Brushes.Black;
                        btn.Width = 25;
                        btn.Height = 25;
                        //对齐

                        Point p1 = ali(p);
                        btn.Margin = new Thickness(p1.X, p1.Y, 0, 0);
                        btn.Background = null;
                        //按钮取消放置事件
                        btn.Click += new RoutedEventHandler(destroy);
                        //移动开始事件
                        btn.MouseDown += new MouseButtonEventHandler(moveStart);
                        //移动结束事件


                        //抬起事件
                        btn.MouseLeftButtonUp += new MouseButtonEventHandler(btnUp);


                        pitch(btn);

                        //音高计算
                        int dis = doNot(btn.Margin.Top);


                        canvas.Children.Add(btn);
                        if (dis >= 9)
                        {
                            canvas.Children.Remove(btn);
                        }
                        canvas.Width = max;
                        //覆盖检测
                        cover(btn);
                        findOnOneLine(btn);
                        findTheMax();
                    }
            }

        }

        public void btnUp(object sender,MouseButtonEventArgs e)
        {
            if (e.LeftButton== MouseButtonState.Released)
            {
                brushStartMove = false;
            }
        }

        public void findTheMax1()
        {
            double max1 = 0;
            foreach (object btn in canvas.Children)
            {
                if (btn is Button)
                {
                    Button bt1 = ((Button)btn);
                    if (bt1.Margin.Left > max1)
                    {
                        max1 = bt1.Margin.Left;
                    }
                }
            }
            max1 += 50;
            if (max1 <= 803)
            {
                max = 803;
                canvas.Width = max;
                gaol.Width = max;
                scrollBar.Maximum = max;
            }
            else
            {
                max = max1;
                canvas.Width = max;
                gaol.Width = max;
                scrollBar.Maximum = max;
            }
            canvas.Width = max;
        }

        public void findTheMax()
        {
            double max1 = 0;
           
            foreach (object btn in canvas.Children)
            {
                if(btn is Button)
                {
                    Button bt1 = ((Button)btn);
                    if (bt1.Margin.Left > max1)
                    {
                        max1 = bt1.Margin.Left;
                    }
                }
            }
            max1 += 50;
            if(max1 <= 803)
            {
                max = 803;
                canvas.Width = max;
                gaol.Width = max;
                scrollBar.Maximum = max;
            }else
            {
                max = max1;
                canvas.Width = max;
                gaol.Width = max;
                scrollBar.Maximum = max;
            }
            canvas.Width = max;


            //画格线
            for (int i = 0; i <= canvas.Width+1000; i += 25)
            {
                foreach(object o in canvas.Children)
                {
                    if(o is Label)
                    {
                        Label l1 = o as Label;
                        if (l1.Margin.Left == i)
                        {
                            goto x;
                        }
                    }
                }



                Label l = new Label();
                l.Background = new SolidColorBrush(Color.FromArgb(40, 255, 255, 255));
                l.Width = 1; l.Height = canvas.Height - 100;
                l.Margin = new Thickness(i, 75, 0, 0);
                if ((l.Margin.Left/25) % 10 == 0)
                {
                    l.Background = Brushes.Purple;
                }
                //MessageBox.Show(l.Margin.Left.ToString());
                canvas.Children.Add(l);


                Label l2 = new Label();
                l2.FontSize = 8;
                double c = (l.Margin.Left / 25);
                if (c % 10 == 0)
                {
                    l2.Foreground = Brushes.Purple;
                }
                l2.Content = c.ToString();
                l2.Background = null; l2.Foreground = Brushes.White;
                l2.Margin = new Thickness(l.Margin.Left - l2.ActualWidth / 2, 50, 0, 0);
                canvas.Children.Add(l2);
            x: continue;
            }

            //待清除的Lable
            List<Label> cls = new List<Label>();

            //清楚格线
            foreach(object i in canvas.Children)
            {
                if(i is Label)
                {
                    Label l = i as Label;
                    if (l.Margin.Left > canvas.Width+1000)
                    {
                        cls.Add(l);
                    }
                }
            }

            foreach(Label l in cls)
            {
                canvas.Children.Remove(l);
            }
            pointerSetter.Width = canvas.Width;


            if (pointer.l2.Margin.Left > this.canvas.Width)
            {
                pointer.l1.Margin = new Thickness(canvas.Width-5, 65, 0, 0);
                pointer.l2.Margin = new Thickness(canvas.Width, 75, 0, 0);

                double b = (canvas.Width - (canvas.Width % 25)) / 25;
                int c = (int)((canvas.Width % 25) / 25 * 10);

                label1.Content = b + ":" + c;
            }

            play.canvasWidth = this.canvas.Width;
            cls.Clear();
        }

        /*
         * y =  275为do基准线
         */

        //音高表
        string[] pitchExcel = new string[13] { "D", "D#", "E", "E#", "F", "F#", "G", "G#","H","H#","A","A#","B"};

        public int pitch(Button btn)
        {

            foreach (object o in pitches.Children)
            {
                Button o1 = o as Button;
                if (btn.Margin.Top == o1.Margin.Top)
                {
                    btn.Content = o1.Content;
                    
                }
            }

            //与300的差值
            int distance = ((((int)btn.Margin.Top - 300) * -1) / 25)-1;
            
            if (distance <= 0)
            {
                return distance - 1;
            }else
            {
                return distance + 1;
            }
           
            
        }

        private int pitch1(Button btn)
        {
            int dis = (((int)btn.Margin.Top - 300) / 25) * -1;
            if (dis <= 0)
            {
                dis -= 1;
            }
            return dis;
        }

        public void cover(Button btn)
        {
            if (btn.Margin.Top < 75)
            {
                return;
            }
            foreach (object bt in canvas.Children)
            {
                if (bt is Button && bt != btn)
                {
                    Button btn1 = (Button)bt;
                    if ((btn1.Margin.Equals(btn.Margin) || btn1.Margin.Left == btn.Margin.Left))
                    {
                        canvas.Children.Remove(btn1);
                        return;
                    }
                    
                    
                }
            }
        }

        /// <summary>
        /// 取消放置
        /// </summary>
        public void destroy(object sender, RoutedEventArgs evt)
        {
            if (!retoZ)
            {
                canvas.Children.Remove(((Button)sender));

                findTheMax();
            }
            else
            {

                //取消高亮
                foreach (object obj in canvas.Children)
                {
                    if (obj is Button)
                    {
                        ((Button)obj).Background = null;
                    }
                }
                retoZ = false;
                clearRetoLab();
                center = null;
                selectKeys.Clear();
            }
            
        }


        //是否移动状态
        bool moving = false;
        //亚透明长方形条
        Label retengel = null;
        //终点位置
        Point end = new Point();
        //待移动物体
        Button movObj = null;

        /// <summary>
        /// 移动开始
        /// </summary>
        public void moveStart(object sender,MouseButtonEventArgs evt)
        {
            if (retoZ == false)
            {
                movObj = ((Button)sender);
                moving = true;
                Label l = new Label();
                l.Opacity = 0.4;
                l.Width = 25; l.Height = 25; l.Background = new SolidColorBrush(Colors.Green);
                retengel = l;
                retengel.Margin = new Thickness(((Button)sender).Margin.Left, ((Button)sender).Margin.Top, 0, 0);
                canvas.Children.Add(retengel);

                end.X = ((Button)sender).Margin.Left + scrollBar.Value;
                end.Y = ((Button)sender).Margin.Top;
            }
            else
            {
                //整体拖放
                center = sender as Button;
                //亚透明长方体
                foreach(Button btnn in selectKeys)
                {
                    retoLable l = new retoLable();
                    l.old = btnn;
                    l.Margin = btnn.Margin;
                    l.Width = 25;l.Height = 25;l.Opacity = 0.4;
                    l.Background = Brushes.Green;
                    //计算相对
                    getPtP(l);
                    

                    retoLables.Add(l);
                    canvas.Children.Add(l);
                    
                }

                //找中心lab
                findLab();
                isFindLab = true;
            }
            
            
        }

        bool isFindLab = false;

        private void findLab()
        {
            if (center != null)
            {
                foreach (retoLable rt in retoLables)
                {
                    if (rt.Margin == center.Margin)
                    {
                        centerLab = rt;
                    }
                }
            }
        }

        private void getPtP(retoLable rt)
        {
            if (center != null)
            {
                Point p = new Point(rt.Margin.Left, rt.Margin.Top);
                Point cp = new Point(center.Margin.Left, center.Margin.Top);
                double tox = cp.X - p.X;
                double toy = cp.Y - p.Y;
                rt.toX = tox;
                rt.toY = toy;
            }
        }

        //整体拖动透明条
        public List<retoLable> retoLables = new List<retoLable>();

        //相对点  中心
        Button center;


        /// <summary>
        /// 对齐
        /// </summary>
        private Point ali(Point p)
        {
            Point p1 = new Point();
            //x%75取多余部分
            double l = (scrollBar.Value + p.X )% 25;
            //取倍数
            int c = (int)((scrollBar.Value + p.X - l) / 25);
            //对齐20倍数线
            p1.X = c * 25-75;
            

            //同理,下面是y
            double l1 = p.Y % 25;
            int c1 = (int)(p.Y - l1) / 25;
            p1.Y = c1 * 25;



            return p1;
        }

        /// <summary>
        /// 移动过程中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        retoLable centerLab;

        private void MetroWindow_MouseMove(object sender, MouseEventArgs e)
        {

            

            if (retoZ&&isFindLab)
            {
                Point p = e.GetPosition(this.grid);
                Point c = ali(p);
                centerLab.Margin = new Thickness(c.X, c.Y, 0, 0);
                foreach (retoLable rt in retoLables)
                {
                    if (true)
                    {
                        double x = c.X - rt.toX;
                        double y = c.Y - rt.toY;
                        rt.Margin = new Thickness(x, y, 0, 0);

                        int dis = doNot(rt.Margin.Top);
                        if (dis > 8)
                        {
                            rt.Visibility = Visibility.Hidden;
                        }else
                        {
                            rt.Visibility = Visibility.Visible;
                        }
                    }
                }
            }

            if(e.LeftButton == MouseButtonState.Pressed)
            {
                if (pencil == false && leftCtrl == false && retoZ == false && center==null)
                {
                    brushPut(e.GetPosition(this.grid));
                    return;
                }
            }

            if(e.LeftButton == MouseButtonState.Released)
            {
                pointerSetter.isMoveSetter = false;
                if (isOK)
                {
                    isOK = false;
                    if (reto != null)
                    {
                        //寻找括选键

                        findKeys();
                        DoubleAnimation da = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.3));
                        reto.BeginAnimation(Label.OpacityProperty, da);
                        da.Completed += new EventHandler(delete);
                    }
                }
            }
            
            if (moving)
            {
                //鼠标位置
                Point p = e.GetPosition(this.grid);
                //对齐
                Point p1 = ali(p);
                retengel.Margin = new Thickness(p1.X, p1.Y, 0, 0);
                end.X = p1.X;
                end.Y = p1.Y;
                int p2 = doNot(retengel.Margin.Top);
                if (doNot(p1.Y) > 8)
                {
                    retengel.Background = new SolidColorBrush(Colors.Red);
                }else
                {
                    retengel.Background = new SolidColorBrush(Colors.Green);
                }
            }

            if (leftCtrl&&isOK)
            {
                Point now = e.GetPosition(this.canvas);
                if (now.X < 0)
                {
                    now.X = 0;
                }

                retoZ = true;
                //寻找方位
                
                find(startP, now, reto);
                findKeys();
            }

            

            
            //显示行
            Point mousePosition = e.GetPosition(this.canvas);
            Point point1 = ali(mousePosition);
            if(point1.Y>=75 && point1.Y<=500)
            lb.Margin = new Thickness(0, point1.Y, 0, 0);
            lb.Width = this.Width;

            //检测是否最大化
            if (this.WindowState == WindowState.Maximized)
            {
                this.label.Width = this.Width + 30;
                scrollBar.Width = this.Width - 75;
                scrollBar.Margin = new Thickness(75, this.Height - 50 - scrollBar.Height, 0, 0);
            }
            
        }

        private void find(Point start,Point now,Label l)
        {
            //相距
            double x = now.X - start.X;
            double y = now.Y - start.Y;
            if (x < 0 && y < 0)
            {
                l.Margin = new Thickness(now.X, now.Y, 0, 0);
                l.Width = x * -1;
                l.Height = y * -1;
                return;
            }

            if (x < 0 && y > 0)
            {
                l.Margin = new Thickness(now.X, start.Y, 0, 0);
                l.Width = x * -1;
                l.Height = y;
                return;
            }

            if (x >= 0 && y >= 0)
            {
                l.Margin = new Thickness(start.X, start.Y, 0, 0);
                l.Width = x;
                l.Height = y;
                return;
            }

            if (x > 0 && y < 0)
            {
                l.Margin = new Thickness(start.X, now.Y, 0, 0);
                l.Width = x;
                l.Height = y * -1;
                return;
            }
        }

        private void About(object sender, RoutedEventArgs e)
        {
            helping = true;
            var result = this.ShowMessageAsync("关于 Redstone红石音乐", "作者：\n\tSao_n\tPB\n\n版本:1.0\n适合的mc版本:1.9+\n\n操作说明:\n左键空白创建音符,音符点击左键移除,或者括选+Delete移除\n右键音符可拖动,Ctrl+左键可以括选"+
                "\n\n本软件最后生成的是ooc\n输入到cb就行了√\n\n预计v1.1版本会做多音轨和多音色",
                MessageDialogStyle.Affirmative,
                new MetroDialogSettings() { AffirmativeButtonText = "好的"});
            helping = false;
        }

        private void retoFinish(object sender,EventArgs e)
        {
            Button mobj = sender as Button;
            pitch(mobj);
            cover(mobj);
            findTheMax();
        }

        private void MetroWindow_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (retoZ && isFindLab)
            {
                foreach(retoLable lb in retoLables)
                {
                    ThicknessAnimation da = new ThicknessAnimation();
                    da.From = lb.old.Margin;
                    da.To = lb.Margin;
                    da.Duration = TimeSpan.FromSeconds(0.2);
                    Finish f = new Finish();
                    f.btn = lb.old;f.m = this;
                    da.Completed += new EventHandler(f.retoFinish);
                    lb.old.BeginAnimation(MarginProperty, da);

                    canvas.Children.Remove(lb);
                }

                retoLables.Clear();
            }
            if (moving)
            {
                //关闭透明条
                canvas.Children.Remove(retengel);
              
                moving = false;
               
                ThicknessAnimation ta = new ThicknessAnimation();
                ta.From = new Thickness(movObj.Margin.Left, movObj.Margin.Top, 0, 0);
                ta.To = new Thickness(end.X, end.Y, 0, 0);
                ta.Duration = TimeSpan.FromSeconds(0.2);
                ta.Completed += new EventHandler(setPitch);
                movObj.BeginAnimation(Button.MarginProperty, ta);
                findTheMax();
                
            }
        }
        /// <summary>
        /// 越界提醒
        /// </summary>
        public int doNot(double y)
        {
            int distance = ((((int)y - 300) * -1) / 25) - 1;
            if(distance < 0)
            {
                distance += 1;
            }
                return Math.Abs(distance);
        }

        private void setPitch(object sender,EventArgs e)
        {
            cover(movObj);
            pitch(movObj);
            if (movObj.Margin.Left < 0)
            {
                canvas.Children.Remove(movObj);
            }
            int i = doNot(movObj.Margin.Top);
            if (i >= 9)
            {
                canvas.Children.Remove(movObj);
            }
        }

        //清除按钮
        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Button> lll = new List<Button>();
            foreach(object o in canvas.Children)
            {
                if(o is Button)
                {
                    lll.Add(o as Button);
                }
            }

            foreach(Button b in lll)
            {
                canvas.Children.Remove(b);
            }

            lll.Clear();
            findTheMax();
            scrollBar.Value = 0;
        }
        /// <summary>
        /// 一些初始化工作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

            if (new LoadingWindow().JianCha())
            {
                this.ShowMessageAsync("检测到资源文件的缺失", "缺少资源文件，我们会马上进行修复");
            }

            //显示行
            Label lbel = new Label();
            lbel.Width = this.Width;
            lbel.Height = 25;
            lbel.Opacity = 0.3;
            lbel.Background = new SolidColorBrush(Colors.DimGray); lbel.Foreground = new SolidColorBrush(Colors.White);
            lbel.Margin = new Thickness(0, -100, 0, 0);
            lb = lbel;
            canvas_Copy.Children.Add(lb);

            //初始化
            string appPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\Redstone MusicProject\";
            if (Directory.Exists(appPath)==false)
            {
                Directory.CreateDirectory(appPath);
            }
            SaveXML.SavePath = SaveXML.SavePathLoad();
        }

        bool leftCtrl = false;

        /// <summary>
        /// 清空整体长方体
        /// </summary>
        private void clearRetoLab()
        {
            foreach(Label l in retoLables)
            {
                canvas.Children.Remove(l);
            }

            retoLables.Clear();
        }
        

        private void MetroWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Space)){
                if (play.canPlay)
                {
                    play.canPlay = false;
                    PlayNode.Header = "播放[Space]";
                }
                else
                {
                    play.canPlay = true;
                    PlayNode.Header = "停止播放[Space]";
                }
            }

            if (Keyboard.IsKeyDown(Key.Delete))
            {
                foreach(Button btn in selectKeys)
                {
                    canvas.Children.Remove(btn);
                    
                }
                findTheMax();
                retoZ = false;
                clearRetoLab();
                center = null;
            }

            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.J))
            {
                play.canPlay = false;
                for (int i = canvas.Children.Count - 1; i > 0; i--)
                {
                    if (canvas.Children[i] is Button)
                    {
                        
                        canvas.Children.Remove(canvas.Children[i]);
                    }
                }
                findTheMax();
                scrollBar.Value = 0;
                retoZ = false;
                clearRetoLab();
                center = null;
            }

            if(Keyboard.IsKeyDown(Key.LeftCtrl)&& Keyboard.IsKeyDown(Key.A))
            {
                selectKeys.Clear();
                retoZ = true;
                foreach (object obj in canvas.Children)
                {
                    if(obj is Button)
                    {
                        ((Button)obj).Background = new SolidColorBrush(Colors.CadetBlue);
                        selectKeys.Add((Button)obj);
                    }
                }
            }

            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                
                leftCtrl = true;
            }
            
        }

        private void MetroWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyUp(Key.LeftCtrl))
            {
                leftCtrl = false;
                if (isOK)
                {
                    isOK = false;
                    if (reto != null)
                    {
                        //寻找括选键
                        findKeys();
                        DoubleAnimation da = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.3));
                        reto.BeginAnimation(Label.OpacityProperty, da);
                        da.Completed += delegate
                        {
                            MessageBox.Show("1");
                            gaol.Children.Remove(reto);
                            reto = null;
                        };
                    }
                }
            }
        }

        private void findOnOneLine(Button btn)
        {
        }

        //括选状态
        bool retoZ = false;
        //括选区域
        Label reto;
        //开始位置
        Point startP;
        //可以设置
        bool isOK = false;

        private void MetroWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            if (retoZ && pencil==false && leftCtrl == false)
            {
                foreach (Button btn in selectKeys)
                {
                    btn.Background = null;
                }
                retoZ = false;
                clearRetoLab();
                center = null;
                return;
            }

            
            if (pencil==false&&leftCtrl==false)
            {
                if (pencil == false && leftCtrl == false && retoZ == false && center == null)
                {
                    brushPut(e.GetPosition(this.grid));
                    return;
                }
            }
            
            brushStartMove = true;
            if (leftCtrl)
            {
                gaol.Children.Clear();
                reto = new Label();
                reto.Background = new SolidColorBrush(Color.FromArgb(40, 6, 128, 251));
                reto.BorderBrush = new SolidColorBrush(Colors.LightBlue);
                reto.BorderThickness = new Thickness(1, 1, 1, 1);
                gaol.Children.Add(reto);
                reto.Opacity = 0;
                startP = e.GetPosition(this.canvas);
                if (startP.X < 0)
                {
                    startP.X = 0;
                }
                reto.Opacity = 1;
                reto.Margin = new Thickness(startP.X,startP.Y,0,0);
                isOK = true;
            }

        }


        /// <summary>
        /// 放入琴键
        /// </summary>
        private void brushPut(Point p)
        {
            Button btn = new Button();
            btn.Background = null;btn.Width = 25;btn.Height = 25;
            btn.BorderBrush = Brushes.Black;
            btn.Foreground = new SolidColorBrush(Colors.Orange);
            
            Point c = ali(p);
            if (c.X < 0)
            {
                c.X = 0;
            }
            btn.Margin = new Thickness(c.X, c.Y, 0, 0);
            int dis = doNot(btn.Margin.Top);
            if (dis >= 9)
            {
                canvas.Children.Remove(btn);
                return;
            }
            pitch(btn);
            cover(btn);
            findTheMax();
            
            btn.MouseDown += new MouseButtonEventHandler(moveStart);
            btn.Click += new RoutedEventHandler(destroy);
            //抬起事件
            btn.MouseLeftButtonUp += new MouseButtonEventHandler(btnUp);

            canvas.Children.Add(btn);
        }

        private void MetroWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pointerSetter.isMoveSetter = false;
            if (e.LeftButton == MouseButtonState.Released)
            {
                brushStartMove = false;
            }
            
            if (isOK)
            {
                isOK = false;
                if (reto != null)
                {
                    //寻找括选键

                    findKeys();
                    DoubleAnimation da = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.3));
                    reto.BeginAnimation(Label.OpacityProperty, da);
                    da.Completed += delegate
                    {
                        MessageBox.Show("1");
                        gaol.Children.Remove(reto);
                        reto = null;
                    };
                }
            }
           
        }

        private void delete(object sender, EventArgs e)
        {
            MessageBox.Show("1");
            gaol.Children.Remove(reto);
            reto = null;
        }

        /// <summary>
        /// 括选键搜寻
        /// </summary>
        /// 

        //被选择琴键

        public List<Button> selectKeys = new List<Button>();
        private void findKeys()
        {
            //清空备选项目
            selectKeys.Clear();
            retoZ = true;
            //计数
            int count = 0;
            foreach(object obj in canvas.Children)
            {
                if(obj is Button)
                {
                    Button btn1 = (Button)obj;
                    if (btn1.Margin.Left >= reto.Margin.Left && btn1.Margin.Top >= reto.Margin.Top
                        &&btn1.Margin.Left+btn1.Width<=reto.Margin.Left+reto.Width&&btn1.Margin.Top+btn1.Height<=reto.Margin.Top+reto.Height)
                    {
                        btn1.Background = new SolidColorBrush(Colors.CadetBlue);
                        selectKeys.Add(btn1);
                        count++;
                    }else
                    {
                        btn1.Background = null;
                    }
                }

            }

            if (count <= 0)
            {
                //括选状态去除
                retoZ = false;
                clearRetoLab();
                center = null;
                selectKeys.Clear();
            }
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            retoZ = true;
            foreach (object obj in canvas.Children)
            {
                if (obj is Button)
                {
                    ((Button)obj).Background = new SolidColorBrush(Colors.CadetBlue);
                }
            }
        }

        private void MetroWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (isOK == false)
            {
                scrollBar.Value += e.Delta * 0.1;
            }
        }

        //刷子或铅笔状态
        bool pencil = true;

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        public string Save()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                //创建xml声明部分
                XmlDeclaration Declaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                
                //根节点
                XmlNode rootNode = xmlDoc.CreateElement("RedstoneMusic");
                //子节点
                List<XmlNode> Node = new List<XmlNode>();

                XmlNode MusicNode;
                XmlAttribute MusicAttribute;
                XmlAttribute NodeAttribute;
                foreach (object Button2 in canvas.Children)
                {
                    if (Button2 is Button) {
                        Button Btnn = Button2 as Button;
                    string content = Btnn.Content.ToString();
                            MusicNode = xmlDoc.CreateElement("Node");
                            MusicAttribute = xmlDoc.CreateAttribute("point");
                            NodeAttribute = xmlDoc.CreateAttribute("Content");
                            NodeAttribute.Value = content;
                            MusicAttribute.Value = SaveXML.ToPoint(Btnn.Margin).ToString();
                            MusicNode.Attributes.Append(MusicAttribute);
                            MusicNode.Attributes.Append(NodeAttribute);
                            Node.Add(MusicNode);
                    }
                }
                //遍历Node
                foreach (XmlNode NodeCopy in Node)
                {
                    rootNode.AppendChild(NodeCopy);
                }
                
                xmlDoc.AppendChild(rootNode);
                xmlDoc.InsertBefore(Declaration, xmlDoc.DocumentElement);
                //保存
                Setting set = new Setting();
                set.settingTxt();
                xmlDoc.Save(SaveXML.SavePath+ ProjectName.Text + ".xml");
                
                return "";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 基本设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setting_Click(object sender, RoutedEventArgs e)
        {
            //new Setting().ShowDialog();
            SettingFlyout.IsOpen = true;
        }

        private void Save_File_Click(object sender, RoutedEventArgs e)
        {
            if (Save() != "")
            {
                this.ShowMessageAsync("在保存的过程中发生了一些错误", Save(), MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "好的" });
            }
            else
            {
                Setting set = new Setting();
                set.settingTxt();
                this.ShowMessageAsync("您的存档已经保存√", "路径为：" + SaveXML.SavePath, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "好的" });
            }
                
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            pencil = true;
            button1.Background = new SolidColorBrush(Color.FromArgb(100,89,24,255));
            button1_Copy.Background = null;
            Button btn = new Button();
            canvas.Children.Add(btn);
            btn.Focus();
            canvas.Children.Remove(btn);
        }

        private void button1_Copy_Click(object sender, RoutedEventArgs e)
        {
            pencil = false;
            button1.Background = null;
            button1_Copy.Background = new SolidColorBrush(Color.FromArgb(100, 89, 24, 255));
            Button btn = new Button();
            canvas.Children.Add(btn);
            btn.Focus();
            canvas.Children.Remove(btn);
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            brushStartMove = false;
        }

        bool canvasDown = true;

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            /*
            if (pencil == false)
            {
                canvasDown = true;
            }
            */
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //canvasDown = false;
        }

        private void canvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void MetroWindow_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }
        /// <summary>
        /// 载入文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Load_File_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfiledialog = new OpenFileDialog();
            openfiledialog.Title = "请选择RMS工程文件";
            openfiledialog.Filter = "RMS工程文件(*.xml)|*.xml";
            if (openfiledialog.ShowDialog() == true)
            {
                /*foreach(object o in canvas.Children)
                {
                    if(o is Button)
                    {
                        Button btn = o as Button;
                        canvas.Children.Remove(btn);
                    }
                }*/
                for (int i = canvas.Children.Count-1; i > 0; i--)
                {
                    if (canvas.Children[i] is Button)
                    {
                        //MessageBox.Show((canvas.Children[i] as Button).Content.ToString(),i.ToString());
                        canvas.Children.Remove(canvas.Children[i]);
                    }
                }
                LoadXML loadxml = new LoadXML(this);
                retoZ = false;
                selectKeys.Clear();
                loadxml.LoadXml(openfiledialog.FileName, canvas);
            }
        }
        Setting seting = new Setting();
        private void SettingFlyout_Loaded(object sender, RoutedEventArgs e)
        {
            Savepath.Text = seting.settingTxt();
            Savepath.TextChanged += delegate
            { 
                SaveXML.SavePath=Savepath.Text;
                FileStream fs = new FileStream(Environment.CurrentDirectory + @"\setting", FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(Savepath.Text);
                sw.Close();
            };
            Savepath.LostFocus += delegate
            {
                
                
            };
            selectPath.Click += delegate { string folder = seting.SelectFolder(); if (folder != "")Savepath.Text = folder; };
           
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            //Environment.Exit(0);
        }

        
        //这里我只是写着试验
        private void auditionBtn(object sender, RoutedEventArgs e)
        {
            try { 
                string Soundspath = "sounds\\" + ((Button)sender).Content.ToString() + ".wav";
                ps.Play(Soundspath, true, 100);
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("程序发生了位置的错误：", ex.Message);
            }
        }

        private void PlayNode_Click(object sender, RoutedEventArgs e)
        {
            if (PlayNode.Header.Equals("播放[Space]"))
            {
                PlayNode.Header = "停止播放[Space]";
                play.canPlay = true;
            }
            else if (PlayNode.Header.Equals("停止播放[Space]"))
            {
                PlayNode.Header = "播放[Space]";
                play.canPlay = false;
            }
        }
    }
}
