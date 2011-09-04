namespace SilkroadProxyWithForms
{
    partial class MainForm
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
                if (_silkroadProxy != null)
                {
                    _silkroadProxy.Dispose();
                    _silkroadProxy = null;
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStripLocalListening = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelGateway = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelAgent = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelNotify = new System.Windows.Forms.ToolStripStatusLabel();
            this.StartGameButton = new System.Windows.Forms.Button();
            this.statusStripLocalListening.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStripLocalListening
            // 
            this.statusStripLocalListening.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabelGateway,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabelAgent,
            this.toolStripStatusLabelNotify});
            this.statusStripLocalListening.Location = new System.Drawing.Point(0, 417);
            this.statusStripLocalListening.Name = "statusStripLocalListening";
            this.statusStripLocalListening.Size = new System.Drawing.Size(752, 22);
            this.statusStripLocalListening.TabIndex = 0;
            this.statusStripLocalListening.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.toolStripStatusLabel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(91, 17);
            this.toolStripStatusLabel1.Text = "Proxy Gateway";
            // 
            // toolStripStatusLabelGateway
            // 
            this.toolStripStatusLabelGateway.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabelGateway.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabelGateway.Name = "toolStripStatusLabelGateway";
            this.toolStripStatusLabelGateway.Size = new System.Drawing.Size(79, 17);
            this.toolStripStatusLabelGateway.Text = "Not working !";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.toolStripStatusLabel2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(76, 17);
            this.toolStripStatusLabel2.Text = "Agent Proxy";
            // 
            // toolStripStatusLabelAgent
            // 
            this.toolStripStatusLabelAgent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabelAgent.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabelAgent.Name = "toolStripStatusLabelAgent";
            this.toolStripStatusLabelAgent.Size = new System.Drawing.Size(79, 17);
            this.toolStripStatusLabelAgent.Text = "Not working !";
            // 
            // toolStripStatusLabelNotify
            // 
            this.toolStripStatusLabelNotify.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.toolStripStatusLabelNotify.Name = "toolStripStatusLabelNotify";
            this.toolStripStatusLabelNotify.Size = new System.Drawing.Size(0, 17);
            // 
            // StartGameButton
            // 
            this.StartGameButton.Location = new System.Drawing.Point(638, 12);
            this.StartGameButton.Name = "StartGameButton";
            this.StartGameButton.Size = new System.Drawing.Size(102, 23);
            this.StartGameButton.TabIndex = 1;
            this.StartGameButton.Text = "Start Game";
            this.StartGameButton.UseVisualStyleBackColor = true;
            this.StartGameButton.Click += new System.EventHandler(this.StartGameButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 439);
            this.Controls.Add(this.StartGameButton);
            this.Controls.Add(this.statusStripLocalListening);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Black Cat Bot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStripLocalListening.ResumeLayout(false);
            this.statusStripLocalListening.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStripLocalListening;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelGateway;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelAgent;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelNotify;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Button StartGameButton;
    }
}

