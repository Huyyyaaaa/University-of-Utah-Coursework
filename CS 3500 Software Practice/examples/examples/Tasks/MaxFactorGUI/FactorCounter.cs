using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaxFactorGUI
{
    public partial class FactorCounter : Form
    {
        private CancellationTokenSource tokenSource;

        public FactorCounter()
        {
            InitializeComponent();
        }

        private void highLimit_TextChanged(object sender, EventArgs e)
        {
            int highBound;
            if (Int32.TryParse(highLimit.Text, out highBound))
            {
                startButton1.Enabled = true;
                startButton2.Enabled = true;
                startButton3.Enabled = true;
                factorCount.Text = "";
            }
            else
            {
                startButton1.Enabled = false;
                startButton2.Enabled = false;
                startButton3.Enabled = false;
                factorCount.Text = "";
            }
        }

        /// <summary>
        /// This runs the factoring algorithm on the GUI event thread.
        /// </summary>
        private void startButton1_Click(object sender, EventArgs e)
        {
            int limit;
            if (Int32.TryParse(highLimit.Text, out limit))
            {
                startButton1.Enabled = false;
                startButton2.Enabled = false;
                startButton3.Enabled = false;
                highLimit.Enabled = false;
                Factors.MaxFactorCount counter = new Factors.MaxFactorCount();
                int count = counter.FindMaxFactors(limit, 2);
                factorCount.Text = count.ToString();
                startButton1.Enabled = true;
                startButton2.Enabled = true;
                startButton3.Enabled = true;
                highLimit.Enabled = true;
            }
        }

        /// <summary>
        /// This runs the factoring algorithm on the GUI event thread with a cancellation token
        /// </summary>
        private void startButton2_Click(object sender, EventArgs e)
        {
            int limit;
            if (Int32.TryParse(highLimit.Text, out limit))
            {
                startButton1.Enabled = false;
                startButton2.Enabled = false;
                startButton3.Enabled = false;
                highLimit.Enabled = false;
                cancelButton.Enabled = true;

                tokenSource = new CancellationTokenSource();
                Task.Run(() => DoFactorCount(limit, 2, tokenSource.Token));                
            }
        }

        /// <summary>
        /// This runs the factoring algorithm on the GUI event thread with a cancellation token, and
        /// also uses Invoke when manipulating GUI objects.
        /// </summary>
        private void startButton3_Click(object sender, EventArgs e)
        {
            int limit;
            if (Int32.TryParse(highLimit.Text, out limit))
            {
                startButton1.Enabled = false;
                startButton2.Enabled = false;
                startButton3.Enabled = false;
                highLimit.Enabled = false;
                cancelButton.Enabled = true;

                tokenSource = new CancellationTokenSource();
                Task.Run(() => DoFactorCount3(limit, 2, tokenSource.Token));
            }
        }

        private void DoFactorCount(int limit, int nTasks, CancellationToken token)
        {
            FactorsToken.MaxFactorCount counter = new FactorsToken.MaxFactorCount();
            try
            {
                int count = counter.FindMaxFactors(limit, nTasks, token);
                DisplayResult(count.ToString());
            }
            catch (OperationCanceledException)
            {
                DisplayResult("Canceled");
            }
        }

        private void DoFactorCount3(int limit, int nTasks, CancellationToken token)
        {
            FactorsToken.MaxFactorCount counter = new FactorsToken.MaxFactorCount();
            try
            {
                int count = counter.FindMaxFactors(limit, nTasks, token);
                factorCount.Invoke((Action)(() => DisplayResult(count.ToString())));
            }
            catch (OperationCanceledException)
            {
                factorCount.Invoke((Action) (() => DisplayResult("Canceled")));
            }
        }

        private void DisplayResult (string message)
        {
            factorCount.Text = message;
            startButton1.Enabled = true;
            startButton2.Enabled = true;
            startButton3.Enabled = true;
            highLimit.Enabled = true;
            cancelButton.Enabled = false;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            tokenSource.Cancel();
        }
    }
}
