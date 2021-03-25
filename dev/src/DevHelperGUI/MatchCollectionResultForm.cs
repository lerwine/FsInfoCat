using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DevHelperGUI
{
    public partial class MatchCollectionResultForm : Form
    {
        public MatchCollectionResultForm()
        {
            InitializeComponent();
        }

        internal void SetResults(IEnumerable<MatchResult> enumerable)
        {
            throw new NotImplementedException();
        }
    }
}
