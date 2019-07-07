using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TipCalculator
{
    public partial class TipCalc : Form
    {
        public TipCalc()
        {
            InitializeComponent();
        }

        private void calcBtn_Click(object sender, EventArgs e)
        {
            double.TryParse(checkBox.Text, out double c);
            double.TryParse(tipBox.Text, out double t);
            double total;
            double temp;
            t = t / 100;

            temp = t * c;

            total = c + temp;

            textBox3.Text = total.ToString();

        }

        private void calcBtn_MouseEnter(object sender, EventArgs e)
        {
            calcBtn.BackColor = Color.Red;
        }

        private void calcBtn_MouseLeave(object sender, EventArgs e)
        {
            calcBtn.BackColor = Color.LightGray;
        }
    }
}
