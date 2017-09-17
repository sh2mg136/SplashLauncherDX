using System;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using System.Reflection;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Diagnostics;

namespace SplashLauncherDX
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DevExpress.Skins.SkinManager.EnableFormSkins();
            UserLookAndFeel.Default.SetSkinStyle("DevExpress Dark Style");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var res = SplashLauncherDX.Properties.Resources.label_blue_new;
            List<string> errors = new List<string>();
            string appName = string.Empty;
            try
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(SplashLauncherDX.WaitForm1));
                //var splashScreenImage = new System.Drawing.Bitmap(CurrentAssembly.GetManifestResourceStream("SplashLauncherDX.Properties.Resources.label_blue_new"));
                //var splashScreenImage = SplashLauncherDX.Properties.Resources.label_blue_new;
                //DevExpress.XtraSplashScreen.SplashScreenManager.ShowImage(splashScreenImage, true, false);
                System.Threading.Thread.Sleep(1000);

                appName = ConfigurationManager.AppSettings["LoaderName"];

                if (string.IsNullOrWhiteSpace(appName))
                    throw new InvalidOperationException("Не удалось прочитать имя сборки");

                FileInfo finfo = new FileInfo(CurrentAssembly.Location);

                var dirPath = finfo.Directory.FullName;
                var loaderPath = Path.Combine(dirPath, appName);

                finfo = new FileInfo(loaderPath);

                if (!finfo.Exists)
                    finfo = new FileInfo(appName);

                if (!finfo.Exists)
                    throw new InvalidOperationException(string.Format("Сборка  {0}  не найдена в директории \r\n '{1}'",
                        loaderPath,
                        dirPath));

                StartAndWait(finfo.FullName);

                DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);

            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                var form = new MainForm(errors);
                Application.Run(form);
            }

        }


        static bool StartAndWait(string processPath)
        {
            Timer timer = new Timer();
            timer.Interval = 2000;
            timer.Tick += timer_Tick;
            timer.Start();

            var myProcess = new Process { StartInfo = new ProcessStartInfo(processPath) };
            myProcess.Start();
            // myProcess.WaitForExit();
            myProcess.WaitForInputIdle(20000);
            return true;
        }

        static void timer_Tick(object sender, EventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false);
            Application.Exit();
        }


        static Assembly currentAssemblyCore;
        static Assembly CurrentAssembly
        {
            get
            {
                if (currentAssemblyCore == null)
                    currentAssemblyCore = Assembly.GetExecutingAssembly();
                return currentAssemblyCore;
            }
        }
    }
}
