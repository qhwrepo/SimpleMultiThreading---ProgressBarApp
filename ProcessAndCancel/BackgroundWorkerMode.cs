using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace ProcessAndCancel
{
    public partial class BackgroundWorkerMode : Form
    {
        //Definition of bgw object
        BackgroundWorker bgWorker = new BackgroundWorker();

        public BackgroundWorkerMode(Form parent)
        {
            InitializeComponent();
            lblInstruction.Text = "Instruction:\n\nBackgroundWorker Mode.\n\nPress Process Button To Start\n\nPress Cancel Button To Stop";
            this.Owner = parent;
            btnCancel.Enabled = false;

            //Set BackgroundWorker properties
            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;

            //Connect handlers to BackgroundWorker object
            bgWorker.DoWork += DoWork_Handler;
            bgWorker.ProgressChanged += ProgressChanged_Handler;
            bgWorker.RunWorkerCompleted += RunWorkerCompleted_Handler;
        }

        private void RunWorkerCompleted_Handler(object sender, RunWorkerCompletedEventArgs args)
        {

            if (args.Cancelled)
                MessageBox.Show("Process was cancelled at " + progressBar.Value + "% in BackgroundWorker Mode.");
            else
                MessageBox.Show("Process completed normally in BackgroundWorker Mode.");
            progressBar.Value = 0;
            lblPercent.Text = "Progress: ";
            btnChangeMode.Enabled = true;
            btnCancel.Enabled = false;
            btnProcess.Enabled = true;
        }

        private void ProgressChanged_Handler(object sender, ProgressChangedEventArgs args)
        {
            progressBar.Value = args.ProgressPercentage;
            lblPercent.Text = "Progress: " + args.ProgressPercentage;
        }

        private void DoWork_Handler(object sender, DoWorkEventArgs args)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            for(int i = 1; i <= 100; i++)
            {
                if (worker.CancellationPending)
                {
                    args.Cancel = true;
                    break;
                }
                else
                {
                    worker.ReportProgress(i);
                    Thread.Sleep(200);
                }
            }
        }

        private void btnChangeMode_Click(object sender, EventArgs e)
        {
            this.Owner.Show();
            this.Dispose();
            this.Close();
            MessageBox.Show("Normal Mode");

        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (!bgWorker.IsBusy)
                bgWorker.RunWorkerAsync();
            btnChangeMode.Enabled = false;
            btnProcess.Enabled = false;
            btnCancel.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bgWorker.CancelAsync();
        }
    }
}
