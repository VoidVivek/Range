using Aveva.CounterRange.Repositories;
using System;
using System.Collections.Generic;
using System.Windows.Forms;



namespace RangeConfigTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            rangeConfigsControl1.DataContext = new Aveva.CounterRange.ViewModels.RangeConfigsControlViewModel(new  FakeRepository());
        }
    }

    

}
