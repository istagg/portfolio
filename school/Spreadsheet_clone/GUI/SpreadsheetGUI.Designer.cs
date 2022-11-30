namespace GUI {
    partial class SpreadsheetGUI {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
         this.SSGridWidget = new SpreadsheetGrid_Core.SpreadsheetGridWidget();
         this.menuStrip1 = new System.Windows.Forms.MenuStrip();
         this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.TxtBoxCellName = new System.Windows.Forms.TextBox();
         this.TxtBoxCellValue = new System.Windows.Forms.TextBox();
         this.TxtBoxCellContent = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
         this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
         this.ChkBxCurrency = new System.Windows.Forms.CheckBox();
         this.menuStrip1.SuspendLayout();
         this.SuspendLayout();
         // 
         // SSGridWidget
         // 
         this.SSGridWidget.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.SSGridWidget.BackColor = System.Drawing.SystemColors.MenuHighlight;
         this.SSGridWidget.Location = new System.Drawing.Point(2, 78);
         this.SSGridWidget.Name = "SSGridWidget";
         this.SSGridWidget.Size = new System.Drawing.Size(977, 514);
         this.SSGridWidget.TabIndex = 0;
         this.SSGridWidget.SelectionChanged += new SpreadsheetGrid_Core.SelectionChangedHandler(this.SSGridWidget_SelectionChanged);
         // 
         // menuStrip1
         // 
         this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
         this.menuStrip1.Location = new System.Drawing.Point(0, 0);
         this.menuStrip1.Name = "menuStrip1";
         this.menuStrip1.Size = new System.Drawing.Size(979, 24);
         this.menuStrip1.TabIndex = 1;
         this.menuStrip1.Text = "menuStrip1";
         // 
         // fileToolStripMenuItem
         // 
         this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.exitToolStripMenuItem});
         this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
         this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
         this.fileToolStripMenuItem.Text = "File";
         // 
         // newToolStripMenuItem
         // 
         this.newToolStripMenuItem.Name = "newToolStripMenuItem";
         this.newToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
         this.newToolStripMenuItem.Text = "New";
         this.newToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
         // 
         // saveToolStripMenuItem
         // 
         this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
         this.saveToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
         this.saveToolStripMenuItem.Text = "Save";
         this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
         // 
         // loadToolStripMenuItem
         // 
         this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
         this.loadToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
         this.loadToolStripMenuItem.Text = "Load";
         this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
         // 
         // exitToolStripMenuItem
         // 
         this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
         this.exitToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
         this.exitToolStripMenuItem.Text = "Exit";
         this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
         // 
         // helpToolStripMenuItem
         // 
         this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
         this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
         this.helpToolStripMenuItem.Text = "Help";
         this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
         // 
         // TxtBoxCellName
         // 
         this.TxtBoxCellName.Enabled = false;
         this.TxtBoxCellName.Location = new System.Drawing.Point(8, 49);
         this.TxtBoxCellName.Name = "TxtBoxCellName";
         this.TxtBoxCellName.Size = new System.Drawing.Size(45, 23);
         this.TxtBoxCellName.TabIndex = 2;
         // 
         // TxtBoxCellValue
         // 
         this.TxtBoxCellValue.Enabled = false;
         this.TxtBoxCellValue.Location = new System.Drawing.Point(59, 49);
         this.TxtBoxCellValue.Name = "TxtBoxCellValue";
         this.TxtBoxCellValue.Size = new System.Drawing.Size(113, 23);
         this.TxtBoxCellValue.TabIndex = 3;
         // 
         // TxtBoxCellContent
         // 
         this.TxtBoxCellContent.Location = new System.Drawing.Point(178, 49);
         this.TxtBoxCellContent.Name = "TxtBoxCellContent";
         this.TxtBoxCellContent.Size = new System.Drawing.Size(259, 23);
         this.TxtBoxCellContent.TabIndex = 4;
         this.TxtBoxCellContent.TextChanged += new System.EventHandler(this.TxtBoxCellContent_TextChanged);
         this.TxtBoxCellContent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtBoxCellContent_KeyDown);
         this.TxtBoxCellContent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpreadsheetGUI_KeyDown);
         this.TxtBoxCellContent.Leave += new System.EventHandler(this.TxtBoxCellContent_Leave);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(7, 30);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(39, 15);
         this.label1.TabIndex = 5;
         this.label1.Text = "Name";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(56, 29);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(35, 15);
         this.label2.TabIndex = 6;
         this.label2.Text = "Value";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(177, 28);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(50, 15);
         this.label3.TabIndex = 7;
         this.label3.Text = "Content";
         // 
         // saveFileDialog1
         // 
         this.saveFileDialog1.Filter = "(*.sprd)|*.sprd|All Files (*.*)|*.*";
         // 
         // openFileDialog1
         // 
         this.openFileDialog1.FileName = "openFileDialog1";
         this.openFileDialog1.Filter = "(*.sprd)|*.sprd|All Files (*.*)|*.*";
         // 
         // ChkBxCurrency
         // 
         this.ChkBxCurrency.AutoSize = true;
         this.ChkBxCurrency.Location = new System.Drawing.Point(448, 50);
         this.ChkBxCurrency.Name = "ChkBxCurrency";
         this.ChkBxCurrency.Size = new System.Drawing.Size(127, 19);
         this.ChkBxCurrency.TabIndex = 8;
         this.ChkBxCurrency.Text = "Display as currency";
         this.ChkBxCurrency.UseVisualStyleBackColor = true;
         this.ChkBxCurrency.CheckedChanged += new System.EventHandler(this.ChkBxCurrency_CheckedChanged);
         // 
         // SpreadsheetGUI
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(979, 593);
         this.Controls.Add(this.ChkBxCurrency);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.TxtBoxCellContent);
         this.Controls.Add(this.TxtBoxCellValue);
         this.Controls.Add(this.TxtBoxCellName);
         this.Controls.Add(this.SSGridWidget);
         this.Controls.Add(this.menuStrip1);
         this.MainMenuStrip = this.menuStrip1;
         this.Name = "SpreadsheetGUI";
         this.Text = "Shockwave Spreadsheet";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpreadsheetGUI_FormClosing);
         this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpreadsheetGUI_KeyDown);
         this.menuStrip1.ResumeLayout(false);
         this.menuStrip1.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private SpreadsheetGrid_Core.SpreadsheetGridWidget SSGridWidget;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private TextBox TxtBoxCellName;
        private TextBox TxtBoxCellValue;
        private TextBox TxtBoxCellContent;
        private Label label1;
        private Label label2;
        private Label label3;
        private ToolStripMenuItem helpToolStripMenuItem;
        private SaveFileDialog saveFileDialog1;
        private ToolStripMenuItem loadToolStripMenuItem;
        private OpenFileDialog openFileDialog1;
      private CheckBox ChkBxCurrency;
   }
}