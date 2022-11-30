namespace ClientGUI {
   partial class ClientGui {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
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
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent() {
            this.PanelStartUp = new System.Windows.Forms.Panel();
            this.BtnStart = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtBoxServer = new System.Windows.Forms.TextBox();
            this.TxtBoxName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LblErrorMessages = new System.Windows.Forms.Label();
            this.GamePanel = new System.Windows.Forms.Panel();
            this.LblStatsX = new System.Windows.Forms.Label();
            this.LblStatsY = new System.Windows.Forms.Label();
            this.LblStatsMass = new System.Windows.Forms.Label();
            this.PanelStartUp.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelStartUp
            // 
            this.PanelStartUp.Controls.Add(this.BtnStart);
            this.PanelStartUp.Controls.Add(this.label2);
            this.PanelStartUp.Controls.Add(this.TxtBoxServer);
            this.PanelStartUp.Controls.Add(this.TxtBoxName);
            this.PanelStartUp.Controls.Add(this.label1);
            this.PanelStartUp.Location = new System.Drawing.Point(506, 248);
            this.PanelStartUp.Name = "PanelStartUp";
            this.PanelStartUp.Size = new System.Drawing.Size(348, 215);
            this.PanelStartUp.TabIndex = 0;
            // 
            // BtnStart
            // 
            this.BtnStart.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BtnStart.Location = new System.Drawing.Point(128, 164);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(101, 43);
            this.BtnStart.TabIndex = 4;
            this.BtnStart.Text = "Start";
            this.BtnStart.UseVisualStyleBackColor = true;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(14, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 32);
            this.label2.TabIndex = 3;
            this.label2.Text = "Server";
            // 
            // TxtBoxServer
            // 
            this.TxtBoxServer.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TxtBoxServer.Location = new System.Drawing.Point(107, 60);
            this.TxtBoxServer.Name = "TxtBoxServer";
            this.TxtBoxServer.Size = new System.Drawing.Size(229, 34);
            this.TxtBoxServer.TabIndex = 2;
            this.TxtBoxServer.Text = "LocalHost";
            this.TxtBoxServer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ClientGui_KeyDown);
            // 
            // TxtBoxName
            // 
            this.TxtBoxName.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TxtBoxName.Location = new System.Drawing.Point(107, 19);
            this.TxtBoxName.Name = "TxtBoxName";
            this.TxtBoxName.Size = new System.Drawing.Size(229, 34);
            this.TxtBoxName.TabIndex = 1;
            this.TxtBoxName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ClientGui_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(14, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // LblErrorMessages
            // 
            this.LblErrorMessages.AutoSize = true;
            this.LblErrorMessages.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LblErrorMessages.Location = new System.Drawing.Point(15, 506);
            this.LblErrorMessages.Name = "LblErrorMessages";
            this.LblErrorMessages.Size = new System.Drawing.Size(148, 28);
            this.LblErrorMessages.TabIndex = 1;
            this.LblErrorMessages.Text = "Error Messages:";
            // 
            // GamePanel
            // 
            this.GamePanel.Location = new System.Drawing.Point(0, 0);
            this.GamePanel.Name = "GamePanel";
            this.GamePanel.Size = new System.Drawing.Size(500, 500);
            this.GamePanel.TabIndex = 2;
            this.GamePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.Draw_Scene);
            // 
            // LblStatsX
            // 
            this.LblStatsX.AutoSize = true;
            this.LblStatsX.Location = new System.Drawing.Point(533, 32);
            this.LblStatsX.Name = "LblStatsX";
            this.LblStatsX.Size = new System.Drawing.Size(26, 15);
            this.LblStatsX.TabIndex = 4;
            this.LblStatsX.Text = "X: 0";
            // 
            // LblStatsY
            // 
            this.LblStatsY.AutoSize = true;
            this.LblStatsY.Location = new System.Drawing.Point(533, 62);
            this.LblStatsY.Name = "LblStatsY";
            this.LblStatsY.Size = new System.Drawing.Size(26, 15);
            this.LblStatsY.TabIndex = 5;
            this.LblStatsY.Text = "Y: 0";
            // 
            // LblStatsMass
            // 
            this.LblStatsMass.AutoSize = true;
            this.LblStatsMass.Location = new System.Drawing.Point(533, 94);
            this.LblStatsMass.Name = "LblStatsMass";
            this.LblStatsMass.Size = new System.Drawing.Size(46, 15);
            this.LblStatsMass.TabIndex = 6;
            this.LblStatsMass.Text = "Mass: 0";
            // 
            // ClientGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 591);
            this.Controls.Add(this.LblStatsMass);
            this.Controls.Add(this.LblStatsY);
            this.Controls.Add(this.LblStatsX);
            this.Controls.Add(this.PanelStartUp);
            this.Controls.Add(this.GamePanel);
            this.Controls.Add(this.LblErrorMessages);
            this.Name = "ClientGui";
            this.Text = "ClientGui";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ClientGui_KeyDown);
            this.PanelStartUp.ResumeLayout(false);
            this.PanelStartUp.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      private Panel PanelStartUp;
      private Button BtnStart;
      private Label label2;
      private TextBox TxtBoxServer;
      private TextBox TxtBoxName;
      private Label label1;
      private Label LblErrorMessages;
      private Panel GamePanel;
        private Label LblStatsX;
        private Label LblStatsY;
        private Label LblStatsMass;
    }
}