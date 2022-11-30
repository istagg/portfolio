namespace ChatClient {
    partial class ChatClient {
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
            this.addressLabel = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.chatLabel = new System.Windows.Forms.Label();
            this.addressTextBox = new System.Windows.Forms.TextBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.chatTextBox = new System.Windows.Forms.RichTextBox();
            this.participantsTextBox = new System.Windows.Forms.RichTextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.participantsButton = new System.Windows.Forms.Button();
            this.nameMissingLabel = new System.Windows.Forms.Label();
            this.connectedLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // addressLabel
            // 
            this.addressLabel.AutoSize = true;
            this.addressLabel.Location = new System.Drawing.Point(87, 90);
            this.addressLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.addressLabel.Name = "addressLabel";
            this.addressLabel.Size = new System.Drawing.Size(245, 32);
            this.addressLabel.TabIndex = 0;
            this.addressLabel.Text = "Server Name/Address";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(87, 190);
            this.nameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(78, 32);
            this.nameLabel.TabIndex = 1;
            this.nameLabel.Text = "Name";
            // 
            // chatLabel
            // 
            this.chatLabel.AutoSize = true;
            this.chatLabel.Location = new System.Drawing.Point(87, 386);
            this.chatLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.chatLabel.Name = "chatLabel";
            this.chatLabel.Size = new System.Drawing.Size(169, 32);
            this.chatLabel.TabIndex = 2;
            this.chatLabel.Text = "Send Message";
            // 
            // addressTextBox
            // 
            this.addressTextBox.Location = new System.Drawing.Point(357, 87);
            this.addressTextBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.addressTextBox.Name = "addressTextBox";
            this.addressTextBox.Size = new System.Drawing.Size(318, 39);
            this.addressTextBox.TabIndex = 3;
            this.addressTextBox.Text = "localhost";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(357, 186);
            this.nameTextBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(318, 39);
            this.nameTextBox.TabIndex = 4;
            this.nameTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nameTextBox_KeyPress);
            // 
            // messageTextBox
            // 
            this.messageTextBox.Location = new System.Drawing.Point(357, 384);
            this.messageTextBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.Size = new System.Drawing.Size(656, 39);
            this.messageTextBox.TabIndex = 5;
            this.messageTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.messageTextBox_KeyPress);
            // 
            // chatTextBox
            // 
            this.chatTextBox.Location = new System.Drawing.Point(43, 514);
            this.chatTextBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.chatTextBox.Name = "chatTextBox";
            this.chatTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.chatTextBox.Size = new System.Drawing.Size(1330, 525);
            this.chatTextBox.TabIndex = 6;
            this.chatTextBox.Text = "";
            // 
            // participantsTextBox
            // 
            this.participantsTextBox.Enabled = false;
            this.participantsTextBox.Location = new System.Drawing.Point(1086, 49);
            this.participantsTextBox.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.participantsTextBox.Name = "participantsTextBox";
            this.participantsTextBox.Size = new System.Drawing.Size(286, 339);
            this.participantsTextBox.TabIndex = 7;
            this.participantsTextBox.Text = "";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(368, 277);
            this.connectButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(292, 62);
            this.connectButton.TabIndex = 8;
            this.connectButton.Text = "Connect To Server";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // participantsButton
            // 
            this.participantsButton.Location = new System.Drawing.Point(1103, 416);
            this.participantsButton.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.participantsButton.Name = "participantsButton";
            this.participantsButton.Size = new System.Drawing.Size(243, 47);
            this.participantsButton.TabIndex = 9;
            this.participantsButton.Text = "Retrieve Participants";
            this.participantsButton.UseVisualStyleBackColor = true;
            this.participantsButton.Click += new System.EventHandler(this.participantsButton_Click);
            // 
            // nameMissingLabel
            // 
            this.nameMissingLabel.AutoSize = true;
            this.nameMissingLabel.ForeColor = System.Drawing.Color.Red;
            this.nameMissingLabel.Location = new System.Drawing.Point(357, 151);
            this.nameMissingLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nameMissingLabel.Name = "nameMissingLabel";
            this.nameMissingLabel.Size = new System.Drawing.Size(264, 32);
            this.nameMissingLabel.TabIndex = 10;
            this.nameMissingLabel.Text = "Please Type Your Name";
            this.nameMissingLabel.Visible = false;
            // 
            // connectedLabel
            // 
            this.connectedLabel.AutoSize = true;
            this.connectedLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.connectedLabel.Location = new System.Drawing.Point(357, 277);
            this.connectedLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.connectedLabel.Name = "connectedLabel";
            this.connectedLabel.Size = new System.Drawing.Size(459, 65);
            this.connectedLabel.TabIndex = 11;
            this.connectedLabel.Text = "Connected to Server";
            this.connectedLabel.Visible = false;
            // 
            // ChatClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1419, 1088);
            this.Controls.Add(this.connectedLabel);
            this.Controls.Add(this.nameMissingLabel);
            this.Controls.Add(this.participantsButton);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.participantsTextBox);
            this.Controls.Add(this.chatTextBox);
            this.Controls.Add(this.messageTextBox);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.addressTextBox);
            this.Controls.Add(this.chatLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.addressLabel);
            this.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Name = "ChatClient";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label addressLabel;
        private Label nameLabel;
        private Label chatLabel;
        private TextBox addressTextBox;
        private TextBox nameTextBox;
        private TextBox messageTextBox;
        private RichTextBox chatTextBox;
        private RichTextBox participantsTextBox;
        private Button connectButton;
        private Button participantsButton;
        private Label nameMissingLabel;
        private Label connectedLabel;
    }
}