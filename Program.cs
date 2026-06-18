using System;
using System.Windows.Forms;

namespace PracticeWork2
{
    static class Program
    {
        /// <summary>Точка входу застосунку.</summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
