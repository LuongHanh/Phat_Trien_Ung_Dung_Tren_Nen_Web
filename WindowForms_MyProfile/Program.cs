using Microsoft.SqlServer.Server;
using System;
using System.Windows.Forms;

namespace WindowForms_MyProfile
{
    internal static class Program
    {
        [STAThread]   // bắt buộc cho WinForms
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Form1());   // chạy Form1
        }
    }
}
