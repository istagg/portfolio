namespace ChatServer {
    partial class ChatServer {
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
         this.label1 = new System.Windows.Forms.Label();
         this.TxtBoxChatParticipants = new System.Windows.Forms.RichTextBox();
         this.TxtBoxChat = new System.Windows.Forms.RichTextBox();
         this.TxtBoxServerName = new System.Windows.Forms.TextBox();
         this.TxtBoxServerIP = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.BtnServer = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(12, 9);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(97, 15);
         this.label1.TabIndex = 0;
         this.label1.Text = "Chat Participants";
         // 
         // TxtBoxChatParticipants
         // 
         this.TxtBoxChatParticipants.Location = new System.Drawing.Point(9, 27);
         this.TxtBoxChatParticipants.Name = "TxtBoxChatParticipants";
         this.TxtBoxChatParticipants.Size = new System.Drawing.Size(196, 229);
         this.TxtBoxChatParticipants.TabIndex = 1;
         this.TxtBoxChatParticipants.Text = "";
         // 
         // TxtBoxChat
         // 
         this.TxtBoxChat.Location = new System.Drawing.Point(236, 94);
         this.TxtBoxChat.Name = "TxtBoxChat";
         this.TxtBoxChat.Size = new System.Drawing.Size(552, 344);
         this.TxtBoxChat.TabIndex = 2;
         this.TxtBoxChat.Text = "";
         // 
         // TxtBoxServerName
         // 
         this.TxtBoxServerName.Location = new System.Drawing.Point(485, 17);
         this.TxtBoxServerName.Name = "TxtBoxServerName";
         this.TxtBoxServerName.Size = new System.Drawing.Size(238, 23);
         this.TxtBoxServerName.TabIndex = 3;
         // 
         // TxtBoxServerIP
         // 
         this.TxtBoxServerIP.Location = new System.Drawing.Point(485, 46);
         this.TxtBoxServerIP.Name = "TxtBoxServerIP";
         this.TxtBoxServerIP.Size = new System.Drawing.Size(238, 23);
         this.TxtBoxServerIP.TabIndex = 4;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(382, 20);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(74, 15);
         this.label2.TabIndex = 5;
         this.label2.Text = "Server Name";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(382, 49);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(52, 15);
         this.label3.TabIndex = 6;
         this.label3.Text = "Server IP";
         // 
         // BtnServer
         // 
         this.BtnServer.Location = new System.Drawing.Point(6, 415);
         this.BtnServer.Name = "BtnServer";
         this.BtnServer.Size = new System.Drawing.Size(199, 23);
         this.BtnServer.TabIndex = 7;
         this.BtnServer.Text = "Start Server";
         this.BtnServer.UseVisualStyleBackColor = true;
         this.BtnServer.Click += new System.EventHandler(this.BtnServer_Click);
         // 
         // ChatServer
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(800, 450);
         this.Controls.Add(this.BtnServer);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.TxtBoxServerIP);
         this.Controls.Add(this.TxtBoxServerName);
         this.Controls.Add(this.TxtBoxChat);
         this.Controls.Add(this.TxtBoxChatParticipants);
         this.Controls.Add(this.label1);
         this.Name = "ChatServer";
         this.Text = "Form1";
         this.ResumeLayout(false);
         this.PerformLayout();

        }

      #endregion

      private Label label1;
      private RichTextBox TxtBoxChatParticipants;
      private RichTextBox TxtBoxChat;
      private TextBox TxtBoxServerName;
      private TextBox TxtBoxServerIP;
      private Label label2;
      private Label label3;
      private Button BtnServer;
   }
}