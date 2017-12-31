using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace redstone_music
{
    public class LoadXML:Button
    {
        
        public LoadXML(MainWindow m)
        {
            this.mainwindow = m;
        }
        MainWindow mainwindow = new MainWindow();
        /// <summary>
        /// 打开工程文件
        /// </summary>
        /// <param name="XmlPath">工程文件路径</param>
        /// <param name="canvas">画布</param>
        /// <returns></returns>
        public string LoadXml(string XmlPath, Canvas canvas)
        {
            try
            {
                string BtnPoint = "";
                string BtnContent = "";


                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(XmlPath);

                XmlElement NodeElement = xmlDoc.DocumentElement;//获取根节点
                XmlNodeList Nodes = NodeElement.GetElementsByTagName("Node");
                /*foreach(object o in canvas.Children)
                {
                    if(o is Button)
                    {
                        Button btn = o as Button;
                        canvas.Children.Remove(btn);
                    }
                }*/
                
                foreach (XmlNode node in Nodes)
                {
                    Button btn = new Button();
                    Point btnPoint = new Point();
                    BtnPoint = ((XmlElement)node).GetAttribute("point");
                    BtnContent = ((XmlElement)node).GetAttribute("Content");
                    btnPoint = Point.Parse(BtnPoint);
                    btn.Margin = new Thickness(btnPoint.X, btnPoint.Y, 0, 0);
                    //按钮取消放置事件
                    btn.Click += new RoutedEventHandler(mainwindow.destroy);
                    //移动开始事件
                    btn.MouseDown += new MouseButtonEventHandler(mainwindow.moveStart);
                    //移动结束事件
                    //抬起事件
                    btn.MouseLeftButtonUp += new MouseButtonEventHandler(mainwindow.btnUp);
                    mainwindow.pitch(btn);
                    btn.Background = null;
                    btn.Foreground = Brushes.White;
                    btn.Width = 25;
                    btn.Height = 25;
                    btn.Content = BtnContent;
                    canvas.Children.Add(btn);
                    mainwindow.cover(btn);
                    mainwindow.findTheMax();
                    
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        
    }
}
