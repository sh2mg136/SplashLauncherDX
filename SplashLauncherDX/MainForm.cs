using DevExpress.XtraEditors;
using System.Collections.Generic;

namespace SplashLauncherDX
{
    public partial class MainForm : XtraForm
    {

        List<string> _errors = new List<string>();

        public MainForm(List<string> errors)
        {
            InitializeComponent();

            _errors = errors;
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.HideImage();
            if (_errors.Count > 0)
            {
                richTextBox1.Clear();

                foreach (var err in _errors)
                {
                    richTextBox1.AppendText(err);
                    richTextBox1.AppendText("\r\n");
                }
            }
        }

        private void ExitButton_Click(object sender, System.EventArgs e)
        {
            Close();
        }

    }

}