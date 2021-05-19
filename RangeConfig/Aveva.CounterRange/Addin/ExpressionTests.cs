using Aveva.CounterRange.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;


namespace Aveva.CounterRange
{
    public partial class ExpressionTests : Form
    {

        public ExpressionTests()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var frm = new Form1();
            frm.Show(this);
        }

        Dictionary<string, TextBox> allParamTextboxes = new Dictionary<string, TextBox>();
        Label lblRange;
        private void button4_Click(object sender, EventArgs e)
        {
            var repository = new Repositories.Repository();
            var frm = new RuleExecutor();
            var all = new List<RuleVal>();
            var attributes = new List<string>();
            string id = ((RangeConfig)comboBox1.SelectedItem).Id;
            var rules = repository.GetRulesForConfig(id);

            allParamTextboxes.Clear();

            foreach (var rule in rules)
            {
                rule.Compile();

                var dt = new DataTable();
                dt.Columns.Add("AndOr");
                dt.Columns.Add("Attribute");
                dt.Columns.Add("Op");
                dt.Columns.Add("Value");
                dt.Columns.Add("Parameter");

                foreach (var condition in rule.Conditions)
                {
                    dt.Rows.Add(condition.AndOr, condition.AttributeUri, condition.ComparisonOperator, condition.Value, "");

                    if (!attributes.Contains(condition.AttributeUri))
                        attributes.Add(condition.AttributeUri);
                }

                Label lbl = new Label();
                lbl.Text = $"Min {rule.MinValue} Max {rule.MaxValue} Sequence {rule.Sequence}";
                lbl.AutoSize = true;
                frm.AddControl(lbl);

                //rule.MinMax = lbl.Text;

                DataGrid grd = new DataGrid();
                grd.DataSource = dt;
                grd.Width = 450;
                grd.Height = 80 + (rule.Conditions.Count * 15);
                frm.AddControl(grd);

                Button btn = new Button();
                btn.Text = "Execute";
                var ruleVal = new RuleVal() { _rule = rule, _dt = dt };
                btn.Tag = ruleVal;
                btn.Click += Btn_Click;
                frm.AddControl(btn);

                all.Add(ruleVal);
            }

            foreach (var attribute in attributes)
            {
                Label lbl = new Label();
                lbl.AutoSize = true;
                lbl.Text = attribute;
                TextBox txtBox = new TextBox();
                allParamTextboxes.Add(attribute, txtBox);

                frm.AddControl(lbl);
                frm.AddControl(txtBox);
            }

            Button btn1 = new Button();
            btn1.Text = "Execute All";
            btn1.Tag = all;
            btn1.Click += Btn1_Click;
            frm.AddControl(btn1);

            lblRange = new Label();
            lblRange.AutoSize = true;
            frm.AddControl(lblRange);

            frm.Show(Application.OpenForms[0]);
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            RuleVal ruleVal = btn.Tag as RuleVal;
            DataTable dt = ruleVal._dt;
            Aveva.CounterRange.Models.Rule rule = ruleVal._rule;

            string str = "";
            var parameters = new List<object>();
            foreach (DataRow dr in dt.Rows)
            {
                object obj = dr["Parameter"];
                if (IsNumeric(dr["Value"].ToString()))
                    parameters.Add(Convert.ToInt32(obj.ToString()));
                else
                    parameters.Add(obj.ToString());

                str += obj.ToString() + " ";
            }

            try
            {
                bool val = rule.Execute(parameters.ToArray());
                MessageBox.Show($"{val} for {str}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn1_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            var allRules = btn.Tag as List<RuleVal>;
            int i = 0;
            foreach (var ruleVal in allRules)
            {
                try
                {
                    DataTable dt = ruleVal._dt;
                    Aveva.CounterRange.Models.Rule rule = ruleVal._rule;
                    string str = "";
                    var parameters = new List<object>();

                    foreach (DataRow dr in dt.Rows)
                    {
                        string attribute = dr["Attribute"].ToString();
                        if (IsNumeric(dr["Value"].ToString()))
                        {
                            parameters.Add(Convert.ToInt32(allParamTextboxes[attribute].Text));
                        }
                        else
                        {
                            parameters.Add(allParamTextboxes[attribute].Text);
                        }

                        str += attribute + "=" + allParamTextboxes[attribute].Text + " ";
                    }

                    bool result = rule.Execute(parameters.ToArray());
                    if (result)
                    {
                        //MessageBox.Show($"'{result}' for '{str}' | Rule - '{rule.MinMax}' wins");
                        lblRange.ForeColor = System.Drawing.Color.Red;
                        lblRange.Text = "Restricted range - " + rule.MinValue.ToString() + " " + rule.MaxValue.ToString();
                        break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                i++;
            }
            if (i == allRules.Count)
            {
                lblRange.ForeColor = System.Drawing.Color.Green;
                lblRange.Text = "Unrestricted range";
            }
        }


        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }

        private void ExpressionTests_Load(object sender, EventArgs e)
        {
            var repository = new Repositories.Repository();
            var configs = repository.GetRangeConfigs();

            foreach (var config in configs)
            {
                comboBox1.Items.Add(config);
                comboBox1.DisplayMember = "DisplayName";
                comboBox1.ValueMember = "Id";
            }

            comboBox1.SelectedIndex = 0;
        }
    }
}
