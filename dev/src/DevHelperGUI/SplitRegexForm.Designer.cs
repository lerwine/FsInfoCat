
namespace DevHelperGUI
{
    partial class SplitRegexForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplitRegexForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.openSessionFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveSessionFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.importPatternFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.savePatternFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.importInputFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveInputFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.saveResultsFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importPatternToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importInputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savePatternToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveInputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inputModesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inputModeAsIsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inputModeBackslashedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inputModeUriEncodedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inputModeXmlEncodedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separateLinesToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.separateLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inputModeWordWrapToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.inputModeWordWrapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.patternAcceptsTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inputAcceptsTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resultModesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rawValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.escapedValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.escapedValueNormalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.escapedValueTnlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quotedLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quotedLinesNormalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quotedLinesTnlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uriEncodedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uriEncodedNormalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uriEncodedNlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xmlEncodedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xmlEncodedNormalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xmlEncodedTnlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hexidecimalValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewModeWordWrapToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.viewModeWordWrapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.evaluationInputs = new DevHelperGUI.EvaluationInputsControl();
            this.resultSplitContainer = new System.Windows.Forms.SplitContainer();
            this.lineResultsDataGridView = new System.Windows.Forms.DataGridView();
            this.resultsDataGridView = new System.Windows.Forms.DataGridView();
            this.tabKeyToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.compiledToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ecmaScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightToLeftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ignorePatternWhitespaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.explicitCaptureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.multilineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.singleLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cultureInvariantToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ignoreCaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultSplitContainer)).BeginInit();
            this.resultSplitContainer.Panel1.SuspendLayout();
            this.resultSplitContainer.Panel2.SuspendLayout();
            this.resultSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lineResultsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // openSessionFileDialog
            // 
            this.openSessionFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            this.openSessionFileDialog.Title = "Open Session Config File";
            // 
            // saveSessionFileDialog
            // 
            this.saveSessionFileDialog.DefaultExt = "xml";
            this.saveSessionFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            this.saveSessionFileDialog.Title = "Save Session Config File";
            // 
            // importPatternFileDialog
            // 
            this.importPatternFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            this.importPatternFileDialog.Title = "Import Pattern";
            // 
            // savePatternFileDialog
            // 
            this.savePatternFileDialog.DefaultExt = "txt";
            this.savePatternFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            this.savePatternFileDialog.Title = "Save Pattern";
            // 
            // importInputFileDialog
            // 
            this.importInputFileDialog.Title = "Import Input Text";
            // 
            // saveInputFileDialog
            // 
            this.saveInputFileDialog.Title = "Save Input Text";
            // 
            // saveResultsFileDialog
            // 
            this.saveResultsFileDialog.DefaultExt = "xml";
            this.saveResultsFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            this.saveResultsFileDialog.Title = "Save Results";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.inputModesToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.resultModesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "Main Menu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.importPatternToolStripMenuItem,
            this.importInputToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.savePatternToolStripMenuItem,
            this.saveInputToolStripMenuItem,
            this.saveResultsToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.newToolStripMenuItem.Text = "&New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.openToolStripMenuItem.Text = "&Open";
            // 
            // importPatternToolStripMenuItem
            // 
            this.importPatternToolStripMenuItem.Name = "importPatternToolStripMenuItem";
            this.importPatternToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.importPatternToolStripMenuItem.Text = "Import Pattern T&ext";
            // 
            // importInputToolStripMenuItem
            // 
            this.importInputToolStripMenuItem.Name = "importInputToolStripMenuItem";
            this.importInputToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.importInputToolStripMenuItem.Text = "Import Input &Text";
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(172, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            // 
            // savePatternToolStripMenuItem
            // 
            this.savePatternToolStripMenuItem.Name = "savePatternToolStripMenuItem";
            this.savePatternToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.savePatternToolStripMenuItem.Text = "Save &Pattern As";
            // 
            // saveInputToolStripMenuItem
            // 
            this.saveInputToolStripMenuItem.Name = "saveInputToolStripMenuItem";
            this.saveInputToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.saveInputToolStripMenuItem.Text = "Save &Input As";
            // 
            // saveResultsToolStripMenuItem
            // 
            this.saveResultsToolStripMenuItem.Name = "saveResultsToolStripMenuItem";
            this.saveResultsToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.saveResultsToolStripMenuItem.Text = "Save &Results";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(172, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator3,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator4,
            this.selectAllToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.undoToolStripMenuItem.Text = "&Undo";
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.redoToolStripMenuItem.Text = "&Redo";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(161, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
            this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.cutToolStripMenuItem.Text = "Cu&t";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
            this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
            this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.pasteToolStripMenuItem.Text = "&Paste";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(161, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.selectAllToolStripMenuItem.Text = "Select &All";
            // 
            // inputModesToolStripMenuItem
            // 
            this.inputModesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inputModeAsIsToolStripMenuItem,
            this.inputModeBackslashedToolStripMenuItem,
            this.inputModeUriEncodedToolStripMenuItem,
            this.inputModeXmlEncodedToolStripMenuItem,
            this.separateLinesToolStripSeparator,
            this.separateLinesToolStripMenuItem,
            this.inputModeWordWrapToolStripSeparator,
            this.inputModeWordWrapToolStripMenuItem});
            this.inputModesToolStripMenuItem.Name = "inputModesToolStripMenuItem";
            this.inputModesToolStripMenuItem.Size = new System.Drawing.Size(86, 20);
            this.inputModesToolStripMenuItem.Text = "Input Modes";
            // 
            // inputModeAsIsToolStripMenuItem
            // 
            this.inputModeAsIsToolStripMenuItem.Checked = true;
            this.inputModeAsIsToolStripMenuItem.CheckOnClick = true;
            this.inputModeAsIsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.inputModeAsIsToolStripMenuItem.Enabled = false;
            this.inputModeAsIsToolStripMenuItem.Name = "inputModeAsIsToolStripMenuItem";
            this.inputModeAsIsToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.inputModeAsIsToolStripMenuItem.Text = "As-Is";
            // 
            // inputModeBackslashedToolStripMenuItem
            // 
            this.inputModeBackslashedToolStripMenuItem.CheckOnClick = true;
            this.inputModeBackslashedToolStripMenuItem.Name = "inputModeBackslashedToolStripMenuItem";
            this.inputModeBackslashedToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.inputModeBackslashedToolStripMenuItem.Text = "Backslash-Escaped";
            // 
            // inputModeUriEncodedToolStripMenuItem
            // 
            this.inputModeUriEncodedToolStripMenuItem.CheckOnClick = true;
            this.inputModeUriEncodedToolStripMenuItem.Name = "inputModeUriEncodedToolStripMenuItem";
            this.inputModeUriEncodedToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.inputModeUriEncodedToolStripMenuItem.Text = "URI-Encoded";
            // 
            // inputModeXmlEncodedToolStripMenuItem
            // 
            this.inputModeXmlEncodedToolStripMenuItem.CheckOnClick = true;
            this.inputModeXmlEncodedToolStripMenuItem.Name = "inputModeXmlEncodedToolStripMenuItem";
            this.inputModeXmlEncodedToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.inputModeXmlEncodedToolStripMenuItem.Text = "XML-Encoded";
            // 
            // separateLinesToolStripSeparator
            // 
            this.separateLinesToolStripSeparator.Name = "separateLinesToolStripSeparator";
            this.separateLinesToolStripSeparator.Size = new System.Drawing.Size(202, 6);
            // 
            // separateLinesToolStripMenuItem
            // 
            this.separateLinesToolStripMenuItem.CheckOnClick = true;
            this.separateLinesToolStripMenuItem.Name = "separateLinesToolStripMenuItem";
            this.separateLinesToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.separateLinesToolStripMenuItem.Text = "Evaluate Lines Separately";
            // 
            // inputModeWordWrapToolStripSeparator
            // 
            this.inputModeWordWrapToolStripSeparator.Name = "inputModeWordWrapToolStripSeparator";
            this.inputModeWordWrapToolStripSeparator.Size = new System.Drawing.Size(202, 6);
            // 
            // inputModeWordWrapToolStripMenuItem
            // 
            this.inputModeWordWrapToolStripMenuItem.Checked = true;
            this.inputModeWordWrapToolStripMenuItem.CheckOnClick = true;
            this.inputModeWordWrapToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.inputModeWordWrapToolStripMenuItem.Name = "inputModeWordWrapToolStripMenuItem";
            this.inputModeWordWrapToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.inputModeWordWrapToolStripMenuItem.Text = "Word Wrap";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noneToolStripMenuItem,
            this.ignoreCaseToolStripMenuItem,
            this.cultureInvariantToolStripMenuItem,
            this.singleLineToolStripMenuItem,
            this.multilineToolStripMenuItem,
            this.explicitCaptureToolStripMenuItem,
            this.ignorePatternWhitespaceToolStripMenuItem,
            this.rightToLeftToolStripMenuItem,
            this.ecmaScriptToolStripMenuItem,
            this.compiledToolStripMenuItem,
            this.tabKeyToolStripSeparator,
            this.patternAcceptsTabToolStripMenuItem,
            this.inputAcceptsTabToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // patternAcceptsTabToolStripMenuItem
            // 
            this.patternAcceptsTabToolStripMenuItem.Name = "patternAcceptsTabToolStripMenuItem";
            this.patternAcceptsTabToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.patternAcceptsTabToolStripMenuItem.Text = "Pattern Accepts Tab Key";
            // 
            // inputAcceptsTabToolStripMenuItem
            // 
            this.inputAcceptsTabToolStripMenuItem.Name = "inputAcceptsTabToolStripMenuItem";
            this.inputAcceptsTabToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.inputAcceptsTabToolStripMenuItem.Text = "Input Accepts Tab Key";
            // 
            // resultModesToolStripMenuItem
            // 
            this.resultModesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rawValueToolStripMenuItem,
            this.escapedValueToolStripMenuItem,
            this.quotedLinesToolStripMenuItem,
            this.uriEncodedToolStripMenuItem,
            this.xmlEncodedToolStripMenuItem,
            this.hexidecimalValuesToolStripMenuItem,
            this.viewModeWordWrapToolStripSeparator,
            this.viewModeWordWrapToolStripMenuItem});
            this.resultModesToolStripMenuItem.Name = "resultModesToolStripMenuItem";
            this.resultModesToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
            this.resultModesToolStripMenuItem.Text = "Result Modes";
            // 
            // rawValueToolStripMenuItem
            // 
            this.rawValueToolStripMenuItem.Checked = true;
            this.rawValueToolStripMenuItem.CheckOnClick = true;
            this.rawValueToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rawValueToolStripMenuItem.Enabled = false;
            this.rawValueToolStripMenuItem.Name = "rawValueToolStripMenuItem";
            this.rawValueToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.rawValueToolStripMenuItem.Text = "Raw Value";
            // 
            // escapedValueToolStripMenuItem
            // 
            this.escapedValueToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.escapedValueNormalToolStripMenuItem,
            this.escapedValueTnlToolStripMenuItem});
            this.escapedValueToolStripMenuItem.Name = "escapedValueToolStripMenuItem";
            this.escapedValueToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.escapedValueToolStripMenuItem.Text = "Escaped Value";
            // 
            // escapedValueNormalToolStripMenuItem
            // 
            this.escapedValueNormalToolStripMenuItem.CheckOnClick = true;
            this.escapedValueNormalToolStripMenuItem.Name = "escapedValueNormalToolStripMenuItem";
            this.escapedValueNormalToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.escapedValueNormalToolStripMenuItem.Text = "Normal Escaped";
            // 
            // escapedValueTnlToolStripMenuItem
            // 
            this.escapedValueTnlToolStripMenuItem.CheckOnClick = true;
            this.escapedValueTnlToolStripMenuItem.Name = "escapedValueTnlToolStripMenuItem";
            this.escapedValueTnlToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.escapedValueTnlToolStripMenuItem.Text = "Escape Line Separators and Tabs";
            // 
            // quotedLinesToolStripMenuItem
            // 
            this.quotedLinesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quotedLinesNormalToolStripMenuItem,
            this.quotedLinesTnlToolStripMenuItem});
            this.quotedLinesToolStripMenuItem.Name = "quotedLinesToolStripMenuItem";
            this.quotedLinesToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.quotedLinesToolStripMenuItem.Text = "Quoted Lines";
            // 
            // quotedLinesNormalToolStripMenuItem
            // 
            this.quotedLinesNormalToolStripMenuItem.CheckOnClick = true;
            this.quotedLinesNormalToolStripMenuItem.Name = "quotedLinesNormalToolStripMenuItem";
            this.quotedLinesNormalToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.quotedLinesNormalToolStripMenuItem.Text = "Normal Quoted";
            // 
            // quotedLinesTnlToolStripMenuItem
            // 
            this.quotedLinesTnlToolStripMenuItem.CheckOnClick = true;
            this.quotedLinesTnlToolStripMenuItem.Name = "quotedLinesTnlToolStripMenuItem";
            this.quotedLinesTnlToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.quotedLinesTnlToolStripMenuItem.Text = "Escape Line Separators and Tabs";
            // 
            // uriEncodedToolStripMenuItem
            // 
            this.uriEncodedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uriEncodedNormalToolStripMenuItem,
            this.uriEncodedNlToolStripMenuItem});
            this.uriEncodedToolStripMenuItem.Name = "uriEncodedToolStripMenuItem";
            this.uriEncodedToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.uriEncodedToolStripMenuItem.Text = "URI-Encoded";
            // 
            // uriEncodedNormalToolStripMenuItem
            // 
            this.uriEncodedNormalToolStripMenuItem.CheckOnClick = true;
            this.uriEncodedNormalToolStripMenuItem.Name = "uriEncodedNormalToolStripMenuItem";
            this.uriEncodedNormalToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.uriEncodedNormalToolStripMenuItem.Text = "Normal Encoded";
            // 
            // uriEncodedNlToolStripMenuItem
            // 
            this.uriEncodedNlToolStripMenuItem.CheckOnClick = true;
            this.uriEncodedNlToolStripMenuItem.Name = "uriEncodedNlToolStripMenuItem";
            this.uriEncodedNlToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.uriEncodedNlToolStripMenuItem.Text = "Encode Line Separators";
            // 
            // xmlEncodedToolStripMenuItem
            // 
            this.xmlEncodedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xmlEncodedNormalToolStripMenuItem,
            this.xmlEncodedTnlToolStripMenuItem});
            this.xmlEncodedToolStripMenuItem.Name = "xmlEncodedToolStripMenuItem";
            this.xmlEncodedToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.xmlEncodedToolStripMenuItem.Text = "XML-Encoded";
            // 
            // xmlEncodedNormalToolStripMenuItem
            // 
            this.xmlEncodedNormalToolStripMenuItem.CheckOnClick = true;
            this.xmlEncodedNormalToolStripMenuItem.Name = "xmlEncodedNormalToolStripMenuItem";
            this.xmlEncodedNormalToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.xmlEncodedNormalToolStripMenuItem.Text = "Normal Encoded";
            // 
            // xmlEncodedTnlToolStripMenuItem
            // 
            this.xmlEncodedTnlToolStripMenuItem.Name = "xmlEncodedTnlToolStripMenuItem";
            this.xmlEncodedTnlToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.xmlEncodedTnlToolStripMenuItem.Text = "Encode Line Separators and Tabs";
            // 
            // hexidecimalValuesToolStripMenuItem
            // 
            this.hexidecimalValuesToolStripMenuItem.Name = "hexidecimalValuesToolStripMenuItem";
            this.hexidecimalValuesToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.hexidecimalValuesToolStripMenuItem.Text = "Hexidecimal Values";
            // 
            // viewModeWordWrapToolStripSeparator
            // 
            this.viewModeWordWrapToolStripSeparator.Name = "viewModeWordWrapToolStripSeparator";
            this.viewModeWordWrapToolStripSeparator.Size = new System.Drawing.Size(202, 6);
            // 
            // viewModeWordWrapToolStripMenuItem
            // 
            this.viewModeWordWrapToolStripMenuItem.Checked = true;
            this.viewModeWordWrapToolStripMenuItem.CheckOnClick = true;
            this.viewModeWordWrapToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.viewModeWordWrapToolStripMenuItem.Name = "viewModeWordWrapToolStripMenuItem";
            this.viewModeWordWrapToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.viewModeWordWrapToolStripMenuItem.Text = "Word Wrap";
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 24);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.evaluationInputs);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.resultSplitContainer);
            this.mainSplitContainer.Size = new System.Drawing.Size(800, 426);
            this.mainSplitContainer.SplitterDistance = 268;
            this.mainSplitContainer.TabIndex = 4;
            // 
            // evaluationInputs
            // 
            this.evaluationInputs.AutoSize = true;
            this.evaluationInputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.evaluationInputs.EvaluateOnChange = false;
            this.evaluationInputs.InputText = "";
            this.evaluationInputs.Location = new System.Drawing.Point(0, 0);
            this.evaluationInputs.Mode = DevHelperGUI.EvaluationInputsMode.PlainText;
            this.evaluationInputs.Name = "evaluationInputs";
            this.evaluationInputs.ParseLinesSeparately = false;
            this.evaluationInputs.PatternText = "";
            this.evaluationInputs.Size = new System.Drawing.Size(268, 426);
            this.evaluationInputs.State = DevHelperGUI.EvaluationState.NotEvaluated;
            this.evaluationInputs.TabIndex = 0;
            // 
            // resultSplitContainer
            // 
            this.resultSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.resultSplitContainer.Name = "resultSplitContainer";
            // 
            // resultSplitContainer.Panel1
            // 
            this.resultSplitContainer.Panel1.Controls.Add(this.lineResultsDataGridView);
            // 
            // resultSplitContainer.Panel2
            // 
            this.resultSplitContainer.Panel2.Controls.Add(this.resultsDataGridView);
            this.resultSplitContainer.Size = new System.Drawing.Size(528, 426);
            this.resultSplitContainer.SplitterDistance = 175;
            this.resultSplitContainer.TabIndex = 0;
            // 
            // lineResultsDataGridView
            // 
            this.lineResultsDataGridView.AllowUserToAddRows = false;
            this.lineResultsDataGridView.AllowUserToDeleteRows = false;
            this.lineResultsDataGridView.AllowUserToOrderColumns = true;
            this.lineResultsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lineResultsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lineResultsDataGridView.Location = new System.Drawing.Point(0, 0);
            this.lineResultsDataGridView.MultiSelect = false;
            this.lineResultsDataGridView.Name = "lineResultsDataGridView";
            this.lineResultsDataGridView.ReadOnly = true;
            this.lineResultsDataGridView.RowHeadersVisible = false;
            this.lineResultsDataGridView.RowTemplate.Height = 25;
            this.lineResultsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.lineResultsDataGridView.ShowEditingIcon = false;
            this.lineResultsDataGridView.Size = new System.Drawing.Size(175, 426);
            this.lineResultsDataGridView.TabIndex = 0;
            // 
            // resultsDataGridView
            // 
            this.resultsDataGridView.AllowUserToAddRows = false;
            this.resultsDataGridView.AllowUserToDeleteRows = false;
            this.resultsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.resultsDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.resultsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultsDataGridView.ColumnHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.resultsDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.resultsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultsDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.resultsDataGridView.Location = new System.Drawing.Point(0, 0);
            this.resultsDataGridView.MultiSelect = false;
            this.resultsDataGridView.Name = "resultsDataGridView";
            this.resultsDataGridView.ReadOnly = true;
            this.resultsDataGridView.RowHeadersVisible = false;
            this.resultsDataGridView.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.resultsDataGridView.RowTemplate.Height = 25;
            this.resultsDataGridView.RowTemplate.ReadOnly = true;
            this.resultsDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.resultsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.resultsDataGridView.ShowEditingIcon = false;
            this.resultsDataGridView.Size = new System.Drawing.Size(349, 426);
            this.resultsDataGridView.TabIndex = 0;
            // 
            // tabKeyToolStripSeparator
            // 
            this.tabKeyToolStripSeparator.Name = "tabKeyToolStripSeparator";
            this.tabKeyToolStripSeparator.Size = new System.Drawing.Size(210, 6);
            // 
            // compiledToolStripMenuItem
            // 
            this.compiledToolStripMenuItem.CheckOnClick = true;
            this.compiledToolStripMenuItem.Name = "compiledToolStripMenuItem";
            this.compiledToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.compiledToolStripMenuItem.Text = "Compiled";
            // 
            // ecmaScriptToolStripMenuItem
            // 
            this.ecmaScriptToolStripMenuItem.CheckOnClick = true;
            this.ecmaScriptToolStripMenuItem.Name = "ecmaScriptToolStripMenuItem";
            this.ecmaScriptToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.ecmaScriptToolStripMenuItem.Text = "ECMAScript";
            // 
            // rightToLeftToolStripMenuItem
            // 
            this.rightToLeftToolStripMenuItem.CheckOnClick = true;
            this.rightToLeftToolStripMenuItem.Name = "rightToLeftToolStripMenuItem";
            this.rightToLeftToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.rightToLeftToolStripMenuItem.Text = "Right To Left";
            // 
            // ignorePatternWhitespaceToolStripMenuItem
            // 
            this.ignorePatternWhitespaceToolStripMenuItem.CheckOnClick = true;
            this.ignorePatternWhitespaceToolStripMenuItem.Name = "ignorePatternWhitespaceToolStripMenuItem";
            this.ignorePatternWhitespaceToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.ignorePatternWhitespaceToolStripMenuItem.Text = "Ignore Pattern Whitespace";
            // 
            // explicitCaptureToolStripMenuItem
            // 
            this.explicitCaptureToolStripMenuItem.CheckOnClick = true;
            this.explicitCaptureToolStripMenuItem.Name = "explicitCaptureToolStripMenuItem";
            this.explicitCaptureToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.explicitCaptureToolStripMenuItem.Text = "Explicit Capture";
            // 
            // multilineToolStripMenuItem
            // 
            this.multilineToolStripMenuItem.CheckOnClick = true;
            this.multilineToolStripMenuItem.Name = "multilineToolStripMenuItem";
            this.multilineToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.multilineToolStripMenuItem.Text = "Multiline";
            // 
            // singleLineToolStripMenuItem
            // 
            this.singleLineToolStripMenuItem.CheckOnClick = true;
            this.singleLineToolStripMenuItem.Name = "singleLineToolStripMenuItem";
            this.singleLineToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.singleLineToolStripMenuItem.Text = "Single-Line";
            // 
            // cultureInvariantToolStripMenuItem
            // 
            this.cultureInvariantToolStripMenuItem.CheckOnClick = true;
            this.cultureInvariantToolStripMenuItem.Name = "cultureInvariantToolStripMenuItem";
            this.cultureInvariantToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.cultureInvariantToolStripMenuItem.Text = "Culture Invariant";
            // 
            // ignoreCaseToolStripMenuItem
            // 
            this.ignoreCaseToolStripMenuItem.CheckOnClick = true;
            this.ignoreCaseToolStripMenuItem.Name = "ignoreCaseToolStripMenuItem";
            this.ignoreCaseToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.ignoreCaseToolStripMenuItem.Text = "Ignore Case";
            // 
            // noneToolStripMenuItem
            // 
            this.noneToolStripMenuItem.Checked = true;
            this.noneToolStripMenuItem.CheckOnClick = true;
            this.noneToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.noneToolStripMenuItem.Enabled = false;
            this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            this.noneToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.noneToolStripMenuItem.Text = "None";
            // 
            // SplitRegexForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mainSplitContainer);
            this.Controls.Add(this.menuStrip1);
            this.Enabled = false;
            this.Name = "SplitRegexForm";
            this.Text = "Line Split Regex";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel1.PerformLayout();
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.resultSplitContainer.Panel1.ResumeLayout(false);
            this.resultSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resultSplitContainer)).EndInit();
            this.resultSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lineResultsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openSessionFileDialog;
        private System.Windows.Forms.SaveFileDialog saveSessionFileDialog;
        private System.Windows.Forms.OpenFileDialog importPatternFileDialog;
        private System.Windows.Forms.SaveFileDialog savePatternFileDialog;
        private System.Windows.Forms.OpenFileDialog importInputFileDialog;
        private System.Windows.Forms.SaveFileDialog saveInputFileDialog;
        private System.Windows.Forms.SaveFileDialog saveResultsFileDialog;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importPatternToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importInputToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savePatternToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveInputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveResultsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inputModesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inputModeAsIsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem11;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem12;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem13;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem14;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem15;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem16;
        private System.Windows.Forms.ToolStripSeparator tabKeyToolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem patternAcceptsTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inputAcceptsTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resultModesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem17;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem18;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem19;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem20;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem21;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem22;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem23;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem24;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem25;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem26;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem27;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem28;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem29;
        private System.Windows.Forms.ToolStripMenuItem hexidecimalValuesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator viewModeWordWrapToolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem30;
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private EvaluationInputsControl evaluationInputs;
        private System.Windows.Forms.SplitContainer resultSplitContainer;
        private System.Windows.Forms.DataGridView lineResultsDataGridView;
        private System.Windows.Forms.DataGridView resultsDataGridView;
        private System.Windows.Forms.ToolStripMenuItem ignoreCaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cultureInvariantToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem singleLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem multilineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem explicitCaptureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ignorePatternWhitespaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rightToLeftToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ecmaScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compiledToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inputModeBackslashedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inputModeUriEncodedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inputModeXmlEncodedToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator separateLinesToolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem separateLinesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator inputModeWordWrapToolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem inputModeWordWrapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rawValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem escapedValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem escapedValueNormalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem escapedValueTnlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quotedLinesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quotedLinesNormalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quotedLinesTnlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uriEncodedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uriEncodedNormalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uriEncodedNlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xmlEncodedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xmlEncodedNormalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xmlEncodedTnlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewModeWordWrapToolStripMenuItem;
    }
}
