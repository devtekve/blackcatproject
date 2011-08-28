namespace Client
{
    partial class ClientForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.ipTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.logsRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP :";
            // 
            // ipTextBox
            // 
            this.ipTextBox.Location = new System.Drawing.Point(41, 7);
            this.ipTextBox.Name = "ipTextBox";
            this.ipTextBox.ReadOnly = true;
            this.ipTextBox.Size = new System.Drawing.Size(55, 20);
            this.ipTextBox.TabIndex = 1;
            this.ipTextBox.Text = "127.0.0.1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(108, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port :";
            // 
            // portTextBox
            // 
            this.portTextBox.Location = new System.Drawing.Point(146, 7);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.ReadOnly = true;
            this.portTextBox.Size = new System.Drawing.Size(40, 20);
            this.portTextBox.TabIndex = 3;
            this.portTextBox.Text = "20000";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(210, 3);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 4;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Message :";
            // 
            // messageTextBox
            // 
            this.messageTextBox.Location = new System.Drawing.Point(74, 38);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.Size = new System.Drawing.Size(431, 20);
            this.messageTextBox.TabIndex = 6;
            this.messageTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.messageTextBox_KeyDown);
            this.messageTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.messageTextBox_KeyPress);
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(521, 35);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(75, 23);
            this.sendButton.TabIndex = 7;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Visible = false;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // logsRichTextBox
            // 
            this.logsRichTextBox.Location = new System.Drawing.Point(15, 76);
            this.logsRichTextBox.Name = "logsRichTextBox";
            this.logsRichTextBox.ReadOnly = true;
            this.logsRichTextBox.Size = new System.Drawing.Size(581, 174);
            this.logsRichTextBox.TabIndex = 8;
            this.logsRichTextBox.Text = "";
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 262);
            this.Controls.Add(this.logsRichTextBox);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.messageTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.portTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ipTextBox);
            this.Controls.Add(this.label1);
            this.Name = "ClientForm";
            this.Text = "Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ipTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox messageTextBox;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.RichTextBox logsRichTextBox;
    }
}

