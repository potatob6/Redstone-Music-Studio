using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace redstone_music
{
    public static class OOC
    {
        public static string[] gcOOC(string[] command)
        {
            List<string> cmd = new List<string>();
            List<string> CmdList = new List<string>();
            string SaveCmd = @"/summon FallingSand ~ ~1.5 ~ {Time:1,Block:minecraft:redstone_block,Motion:[0d,-1d,0d],Passengers:[{id:FallingSand,Time:1,Block:minecraft:activator_rail,Passengers:[{id:MinecartCommandBlock,Command:blockdata ~ ~-2 ~ {auto:0b,Command:""}}";
            int Len = SaveCmd.Length;
            int pos = 1;
            #region 给cmdlist赋值
            foreach (string CommandArray in command)
            {
                CmdList.Add(CommandArray);
            }
            #endregion
            for (int i = 0; i < CmdList.Count; i++)
            {
               
                SaveCmd += ",{id:MinecartCommandBlock,Command:setblock ~" + pos + " ~1 ~ command_block 1 replace {Command:"+CmdList[i]+",auto:1}}";
                Len = SaveCmd.Length;
                if (i < CmdList.Count - 1)
                {
                    if (SaveCmd.Length + CmdList[i + 1].Length+192 > 32767)
                    {
                        cmd.Add(SaveCmd + ",{id:MinecartCommandBlock,Command:setblock ~ ~1 ~ command_block 0 replace {auto:1b,Command:fill ~ ~ ~ ~ ~-2 ~ air}},{id:MinecartCommandBlock,Command:kill @e[type=MinecartCommandBlock,r=1]}]}]}");
                        SaveCmd = @"/summon FallingSand ~ ~1.5 ~ {Time:1,Block:minecraft:redstone_block,Motion:[0d,-1d,0d],Passengers:[{id:FallingSand,Time:1,Block:minecraft:activator_rail,Passengers:[{id:MinecartCommandBlock,Command:blockdata ~ ~-2 ~ {auto:0b,Command:""}}";
                        Len = SaveCmd.Length;
                    }
                }
                
                
                pos++;
            }
            if (cmd.Count < 1)
            {
                cmd.Add(SaveCmd + ",{id:MinecartCommandBlock,Command:setblock ~ ~1 ~ command_block 0 replace {auto:1b,Command:fill ~ ~ ~ ~ ~-2 ~ air}},{id:MinecartCommandBlock,Command:kill @e[type=MinecartCommandBlock,r=1]}]}]}");
            }//end if
            return cmd.ToArray();
        }
    }
}
