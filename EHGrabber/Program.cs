using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace EHGrabber
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, "EHGRABBER", out createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm());
                }
                else
                {
                    Process current = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id != current.Id)
                        {
                            NativeMethods.SetForegroundWindow(process.MainWindowHandle);
                            NativeMethods.FlashWindow(process.MainWindowHandle, true);
                            break;
                        }
                    }
                }
            }
        }
    }
}
