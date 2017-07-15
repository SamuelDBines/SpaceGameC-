using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestBuild
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //new MainMenu().Show();
            if (Settings.QuickLoad)
            {
                Settings.sendPort = "8002";
                Settings.recvPort = "8003";
                Settings.Port = "8001";
                Settings.Ip = "";
                Settings.UserName = "";
                Application.Run(new GameScreen());
            }
            else
            {
                Application.Run(new MainMenu());
            }


        }
    }
}
