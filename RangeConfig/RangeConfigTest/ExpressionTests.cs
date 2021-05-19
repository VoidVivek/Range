using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using Aveva.CounterRange.ViewModels;

namespace RangeConfigTest
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
            var fakeRep = new FakeRepository();
            var frm = new RuleExecutor();
            var all = new List<RuleVal>();
            var attributes = new List<string>();

            allParamTextboxes.Clear();

            var rules = fakeRep.GetRulesForConfig("1");
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

            frm.Show(this);
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


            bool val = rule.Execute(parameters.ToArray());
            MessageBox.Show($"{val} for {str}");
        }

        private void Btn1_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            var allRules = btn.Tag as List<RuleVal>;
            int i = 0;
            foreach (var ruleVal in allRules)
            {
                //MessageBox.Show(i.ToString());

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
    }
}
