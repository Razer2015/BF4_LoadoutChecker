using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BF4_LoadoutChecker
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
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
            Application.Run(new bf4_loadoutchecker());
        }

        private static void AllUnhandledExceptions(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            using (StreamWriter sw = new StreamWriter("exceptions.log"))
                sw.WriteLine("Main : " + ex.Message, typeof(Program));
            MessageBox.Show(ex.Message + ex.StackTrace);
            Environment.Exit(1);
            Environment.Exit(System.Runtime.InteropServices.Marshal.GetHRForException(ex));
        }
    }
}
