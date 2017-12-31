using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows;

namespace redstone_music
{
    public class PlaySound
    { 
        #region 播放器
        MediaPlayer[] player = new MediaPlayer[7]; bool[] py = new bool[7]{true,true,true,true,true,true,true};
        #endregion
        public PlaySound()
        {

            for (int i = 0; i < 7; i++)
            {
                player[i] = new MediaPlayer();
            }
        }
        
       
        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="SoundPath">声音文件的路径</param>
        /// <param name="RelativePath">是否为相对路径</param>
        /// <param name="Volume">音量</param>
        public void Play(string SoundPath,bool RelativePath,double Volume)
        {
            UriKind urikind;
            #region 播放器赋值
            object PlayerIsPlay = new object();
            player[0].MediaEnded += delegate { py[0] = true; }; player[0].BufferingEnded += delegate { py[0] = false; };
            player[1].MediaEnded += delegate { py[1] = true; }; player[1].BufferingEnded += delegate { py[1] = false; };
            player[2].MediaEnded += delegate { py[2] = true; }; player[2].BufferingEnded += delegate { py[2] = false; };
            player[3].MediaEnded += delegate { py[3] = true; }; player[3].BufferingEnded += delegate { py[3] = false; };
            player[4].MediaEnded += delegate { py[4] = true; }; player[4].BufferingEnded += delegate { py[4] = false; };
            player[5].MediaEnded += delegate { py[5] = true; }; player[5].BufferingEnded += delegate { py[5] = false; };
            player[6].MediaEnded += delegate { py[6] = true; }; player[6].BufferingEnded += delegate { py[6] = false; };
            int booll = 0;
            #region 给playerisplay赋值
            foreach (bool bol in py)
            {
                if (bol)
                {
                    PlayerIsPlay = player[booll];
                    break;
                }
                booll++;
            }
            #endregion
                #endregion
            if (RelativePath)
                urikind = UriKind.Relative;
            else
            {
                urikind = UriKind.Absolute;
            }
            (PlayerIsPlay as MediaPlayer).Open(new Uri(SoundPath, urikind));
            (PlayerIsPlay as MediaPlayer).Volume = Volume;
            (PlayerIsPlay as MediaPlayer).Play();
            
        }
        
    }
}
