using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RangeConfigTest
{
    public partial class RuleExecutor : Form
    {
        public RuleExecutor()
        {
            InitializeComponent();
        }

        private void RuleExecutor_Load(object sender, EventArgs e)
        {
           
        }

        public void AddControl(Control ctrl)
        {
            flowLayoutPanel1.Controls.Add(ctrl);
        }
    }
}
