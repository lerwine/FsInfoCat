using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DevHelperGUI
{
    public partial class MultipleMatchCollectionForm : Form
    {
        public MultipleMatchCollectionForm()
        {
            InitializeComponent();
        }

        internal void SetResults(IEnumerable<MatchCollectionResult> enumerable)
        {
            throw new NotImplementedException();
        }
    }
}
