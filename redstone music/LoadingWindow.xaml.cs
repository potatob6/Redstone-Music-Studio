using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.IO.Compression;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace redstone_music
{
    /// <summary>
    /// LoadingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingWindow : MetroWindow
    {
        public LoadingWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
             
        }

        private void DownLoad(string Url,string FileName)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFileAsync(new Uri(Url.Trim()),FileName);
                client.DownloadFileCompleted += delegate 
                {
                    lb.Content = "下载完成";
                   ZipFile.ExtractToDirectory(FileName, Environment.CurrentDirectory);
                   File.Delete("sounds.zip");
                   
                };
            }
        }

        public void WriteSetting()
        {
            if (File.Exists("setting") == false)
            {
                lb.Content = "检测到资源文件缺失";
                string appPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\Redstone MusicProject\";
                FileStream fs = new FileStream("setting", FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(appPath);
                sw.Close();
            }
        }

        public bool JianCha()
        {
            WriteSetting();
            bool QueS = false;
            //音高表
            string[] pitchExcel = new string[] { "A#", "A", "A1", "B#", "B", "C#", "C", "D#", "D", "E#", "E", "F#", "F#1", "F", "G#", "G#1", "G", "G1" };
            for (int i = 0; i <= 13; i++)
            {
                if (Directory.Exists("sounds") == false)
                {

                    QueS = true;
                    break;
                }
                if (File.Exists("sounds\\" + pitchExcel[i] + ".wav") == false)
                {
                    if (Directory.Exists("sounds"))
                        Directory.Delete("sounds", true);

                    QueS = true;
                    break;
                }
            }
            
            if (QueS)
            {
                DownLoad("http://yqall02.baidupcs.com/file/6054fba8930202fe9e2613dbd3283cea?bkt=p3-14006054fba8930202fe9e2613dbd3283cea2987d68e00000016d18c&fid=88491768-250528-1028976361705597&time=1473246437&sign=FDTAXGERLBH-DCb740ccc5511e5e8fedcff06b081203-m78E1%2BqlfLKFgwPaw89RbHHHGXc%3D&to=yqhb&fm=Yan,B,T,t&sta_dx=1495436&sta_cs=2&sta_ft=zip&sta_ct=0&sta_mt=0&fm2=Yangquan,B,T,t&newver=1&newfm=1&secfm=1&flow_ver=3&pkey=14006054fba8930202fe9e2613dbd3283cea2987d68e00000016d18c&sl=68616271&expires=8h&rt=sh&r=677855553&mlogid=5816684075913249962&vuk=88491768&vbdid=957777349&fin=sounds.zip&fn=sounds.zip&slt=pm&uta=0&rtype=1&iv=0&isw=0&dp-logid=5816684075913249962&dp-callid=0.1.1&csl=456&csign=KHHscHl6R6u%2BxzjvWcyuUlwTExo%3D", "sounds.zip");
                lb.Content = "检测到资源文件缺失";
            }
            else
            {
                lb.Content = "一切正常，正在启动";             
            }
            return QueS;
        }


    }
}
