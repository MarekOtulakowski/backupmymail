using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace BackupMyMail.Gui
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            string procName = Process.GetCurrentProcess().ProcessName;
            // get the list of all processes by that name

            Process[] processes = Process.GetProcessesByName(procName);

            if (processes.Length > 1)
            {
                MessageBox.Show("Another instance of the app BackupMyMail is running. Bye!",
                                "BackupMyMail",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
                Application.Run(new F_Main(args));
            else
                Application.Run(new F_Main());
        }
    }
}
