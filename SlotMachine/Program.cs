using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Sharp Slot Machine
/// Original Framework provided by Tom Siliopoulos
/// Modifed by David McNiven
/// Student # 200330143
/// Created on December 8th, 2016
/// A basic 3 reel slot machine simulator 
/// Music is property of Square Enix
/// </summary>
namespace SlotMachine
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
            Application.Run(new SlotMachineForm());
        }
    }
}
