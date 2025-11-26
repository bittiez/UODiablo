using Server;
using Server.Accounting;
using Server.Mobiles;
using System;
using System.Collections.Generic;

namespace Bittiez.FlexCap
{
    public class FlexCap
    { //Version 1.0.0

        //This is for total game time of all characters
        /// <summary>
        /// {Time in minutes, Skill cap}
        /// 1x gm is 1000 skill points
        /// </summary>
        public static Dictionary<int, int> skillCap = new Dictionary<int, int> {
            {0, 7000},
            {20*60, 8000},
            {40*60, 9000},
            {60*60, 10000},
            {80*60, 11000},
            {100*60, 12000},
            {120*60, 13000},
        };

        /// <summary>
        /// {Time in minutes, Stat cap}
        /// 1x gm is 1000 skill points
        /// </summary>
        public static Dictionary<int, int> statCap = new Dictionary<int, int> {
            //Time in minutes, statCap
            {0, 150},
            {4*60, 175},
            {6*60, 200},
            {10*60, 225},
            {80*60, 250},
            {100*60, 275},
            {120*60, 300},
        };

        public static void Initialize()
        {

            EventSink.Login += new LoginEventHandler(On_Login);
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("FlexCap system loaded.");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void On_Login(LoginEventArgs e)
        {
            PlayerMobile who = (PlayerMobile)e.Mobile;
            Account ac = (Account)who.Account;

            if (who.AccessLevel > AccessLevel.Player) //Set staff members with an absurd skill/stat cap
            {
                who.Skills.Cap = 1000 * 60;
                who.StatCap = 1000;
                Console.WriteLine($"{who.Name} - Skill cap {who.SkillsCap} - Stat cap {who.StatCap}");
                return;
            }

            TimeSpan gameTime = ac.TotalGameTime;
            int newSkillCap = 0;
            foreach (KeyValuePair<int, int> entry in skillCap)
            {
                if (gameTime.TotalMinutes >= entry.Key)
                {
                    newSkillCap = entry.Value;
                }
            }
            who.SkillsCap = newSkillCap;

            int newStatCap = 0;
            foreach (KeyValuePair<int, int> entry in statCap)
            {
                if (gameTime.TotalMinutes >= entry.Key)
                {
                    newStatCap = entry.Value;
                }
            }
            who.StatCap = newStatCap;

            Console.WriteLine($"{who.Name} - Skill cap {who.SkillsCap} - Stat cap {who.StatCap}");
        }
    }
}
