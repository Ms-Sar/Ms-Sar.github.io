using System;
using System.Windows.Forms;
using STRmodsWF2SuspensionEditor.UI;

namespace STRmodsWF2SuspensionEditor
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}