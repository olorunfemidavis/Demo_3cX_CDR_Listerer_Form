using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_3cX_CDR_Listerer_Form
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            bool result;
            var mutex = new Mutex(true, "71C8A523-A276-46F3-908F-E5B944CBBCE5", out result);

            if (!result)
            {
                // MessageBox.Show("Another instance of the application is already running.");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Listener());

            GC.KeepAlive(mutex);

        }
    }
}
