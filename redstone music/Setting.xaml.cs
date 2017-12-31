using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using MahApps.Metro.Controls;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace redstone_music
{
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : MetroWindow
    {
        public Setting()
        {
            InitializeComponent();
            
        }

        

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            /*if (this.SavePath.Text != MainWindow.path)
            {
                SavePath.Text = MainWindow.path;
            }
            else
                settingTxt();
             */
            settingTxt();
            SavePath.TextChanged += delegate { SaveXML.SavePath = SavePath.Text; };
        }

        public string settingTxt()
        {
            string appPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\Redstone MusicProject\";
            if (File.Exists(Environment.CurrentDirectory + @"\setting"))
            {
                
                return File.ReadAllText(Environment.CurrentDirectory + @"\setting", Encoding.Default);
            }
            else
            {
                FileStream fs = new FileStream(Environment.CurrentDirectory + @"\setting", FileMode.Create, FileAccess.Write);
                byte[] text = Encoding.Default.GetBytes(appPath);
                fs.Write(text, 0, text.Length);
                fs.Close();
                return settingTxt();
            }
           
        }

        private void SelectPath_Click(object sender, RoutedEventArgs e)
        {
            SavePath.Text = SelectFolder();
            //MainWindow.path = SavePath.Text;
            
        }

        public string SelectFolder()
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return fd.SelectedPath+"\\";
            }
            else
            {
                return SavePath.Text;
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FileStream fs = new FileStream(Environment.CurrentDirectory + @"\setting", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(SavePath.Text);
            sw.Close();
        }

        

       
    }
}
