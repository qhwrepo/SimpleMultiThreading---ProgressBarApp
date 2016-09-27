using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace ProcessAndCancel
{
    public partial class NormalMode : Form
    {
        CancellationTokenSource _cancellationTokenSource;
        CancellationToken _cancellationToken;
        public NormalMode()
        {
            InitializeComponent();
            lblInstruction.Text = "Instruction:\n\nNormal Mode.\n\nPress Process Button To Start\n\nPress Cancel Button To Stop";
            btnCancel.Enabled = false;
        }

        private async void btnProcess_Click(object sender, EventArgs e)
        {
            btnProcess.Enabled = false;
            btnCancel.Enabled = true;
            btnChangeMode.Enabled = false;
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;

            int completedPercent = 0;
            for (int i = 0; i < 100; i++)
            {
                if (_cancellationToken.IsCancellationRequested == true)
                    break;

                try
                {
                    await Task.Delay(200);
                    completedPercent = i + 1;
                }
                catch (TaskCanceledException)
                {
                    completedPercent = i;
                }
                progressBar.Value = completedPercent;
                lblPercent.Text = "Progress: " + completedPercent + "%";

            }

            string message = _cancellationToken.IsCancellationRequested
                ? string.Format("Process was cancelled at {0}% in Normal Mode.", completedPercent)
                : string.Format("Process completed normally in Normal Mode.");
            MessageBox.Show(message, "Completion Status");
            progressBar.Value = 0;
            lblPercent.Text = "Progress: ";
            btnCancel.Enabled = false;
            btnProcess.Enabled = true;
            btnChangeMode.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        private void btnChangeMode_Click(object sender, EventArgs e)
        {
            BackgroundWorkerMode bwm = new BackgroundWorkerMode(this);
            this.Hide();
            bwm.Show();
            MessageBox.Show("BackgroundWorker Mode");  
        }

       
    }
}
