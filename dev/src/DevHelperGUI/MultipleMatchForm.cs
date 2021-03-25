using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DevHelperGUI
{
    public partial class MultipleMatchForm : Form
    {
        public MultipleMatchForm()
        {
            InitializeComponent();
        }

        internal void SetResults(IEnumerable<MatchResult> enumerable)
        {
            throw new NotImplementedException();
        }
    }
}
