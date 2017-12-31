using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading.Tasks;

namespace redstone_music
{
    public static class SaveXML
    {

        /// <summary>
        /// 工程保存路径
        /// </summary>
        public static string SavePath;
        /// <summary>
        /// 将Thickness转换为Point类型
        /// </summary>
        /// <param name="Margin">Margin值</param>
        /// <returns></returns>
        public static Point ToPoint(Thickness Margin){
            string mar = Margin.ToString();
            string[] margin = mar.Split(',');
            Point pit = new Point();
            pit.X = Convert.ToDouble(margin[0]);
            pit.Y = Convert.ToDouble(margin[1]);
            return pit;
        }
        /// <summary>
        /// SavePath变量的初始化
        /// </summary>
        /// <returns></returns>
        public static string SavePathLoad()
        {
            FileStream fs = new FileStream("setting", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            return sr.ReadToEnd();
        }

    }
}
