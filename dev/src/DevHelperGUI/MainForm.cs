using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DevHelperGUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void SingleMatchRegexButton_Click(object sender, EventArgs e)
        {
            using SingleMatchRegexForm form = new SingleMatchRegexForm();
            form.ShowDialog(this);
        }

        private void MatchAllRegexButton_Click(object sender, EventArgs e)
        {
            using MatchAllRegexForm form = new MatchAllRegexForm();
            form.ShowDialog(this);
        }

        private void SplitRegexButton_Click(object sender, EventArgs e)
        {
            using SplitRegexForm form = new SplitRegexForm();
            form.ShowDialog(this);
        }
    }
}
