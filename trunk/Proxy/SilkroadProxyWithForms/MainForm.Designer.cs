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
            this.gpbCharInfo = new System.Windows.Forms.GroupBox();
            this.lblCharCoord = new System.Windows.Forms.Label();
            this.pbCharMP = new System.Windows.Forms.ProgressBar();
            this.pbCharHP = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCharName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabConfig = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.button19 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.btnDeleteSkill = new System.Windows.Forms.Button();
            this.btnAddSkill = new System.Windows.Forms.Button();
            this.listView2 = new System.Windows.Forms.ListView();
            this.colSkillUseName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSkillType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView1 = new System.Windows.Forms.ListView();
            this.colSkillName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLevel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.button17 = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.button18 = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.listView3 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button5 = new System.Windows.Forms.Button();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.button16 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button15 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button14 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.listView4 = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.statusStripLocalListening.SuspendLayout();
            this.gpbCharInfo.SuspendLayout();
            this.tabConfig.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
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
            this.statusStripLocalListening.Location = new System.Drawing.Point(0, 673);
            this.statusStripLocalListening.Name = "statusStripLocalListening";
            this.statusStripLocalListening.Size = new System.Drawing.Size(682, 22);
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
            this.StartGameButton.Location = new System.Drawing.Point(497, 12);
            this.StartGameButton.Name = "StartGameButton";
            this.StartGameButton.Size = new System.Drawing.Size(102, 23);
            this.StartGameButton.TabIndex = 1;
            this.StartGameButton.Text = "Start Game";
            this.StartGameButton.UseVisualStyleBackColor = true;
            this.StartGameButton.Click += new System.EventHandler(this.StartGameButton_Click);
            // 
            // gpbCharInfo
            // 
            this.gpbCharInfo.Controls.Add(this.lblCharCoord);
            this.gpbCharInfo.Controls.Add(this.pbCharMP);
            this.gpbCharInfo.Controls.Add(this.pbCharHP);
            this.gpbCharInfo.Controls.Add(this.label2);
            this.gpbCharInfo.Controls.Add(this.lblCharName);
            this.gpbCharInfo.Controls.Add(this.label1);
            this.gpbCharInfo.Location = new System.Drawing.Point(497, 54);
            this.gpbCharInfo.Name = "gpbCharInfo";
            this.gpbCharInfo.Size = new System.Drawing.Size(170, 135);
            this.gpbCharInfo.TabIndex = 4;
            this.gpbCharInfo.TabStop = false;
            this.gpbCharInfo.Text = "Thông tin Nhân vật";
            // 
            // lblCharCoord
            // 
            this.lblCharCoord.AutoSize = true;
            this.lblCharCoord.Location = new System.Drawing.Point(60, 105);
            this.lblCharCoord.Name = "lblCharCoord";
            this.lblCharCoord.Size = new System.Drawing.Size(0, 13);
            this.lblCharCoord.TabIndex = 5;
            // 
            // pbCharMP
            // 
            this.pbCharMP.Location = new System.Drawing.Point(34, 77);
            this.pbCharMP.Name = "pbCharMP";
            this.pbCharMP.Size = new System.Drawing.Size(126, 13);
            this.pbCharMP.TabIndex = 4;
            // 
            // pbCharHP
            // 
            this.pbCharHP.Location = new System.Drawing.Point(34, 55);
            this.pbCharHP.Name = "pbCharHP";
            this.pbCharHP.Size = new System.Drawing.Size(126, 13);
            this.pbCharHP.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "MP";
            // 
            // lblCharName
            // 
            this.lblCharName.AutoSize = true;
            this.lblCharName.Location = new System.Drawing.Point(60, 25);
            this.lblCharName.Name = "lblCharName";
            this.lblCharName.Size = new System.Drawing.Size(0, 13);
            this.lblCharName.TabIndex = 1;
            this.lblCharName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "HP";
            // 
            // tabConfig
            // 
            this.tabConfig.Controls.Add(this.tabPage1);
            this.tabConfig.Controls.Add(this.tabPage2);
            this.tabConfig.Controls.Add(this.tabPage3);
            this.tabConfig.Location = new System.Drawing.Point(12, 12);
            this.tabConfig.Name = "tabConfig";
            this.tabConfig.SelectedIndex = 0;
            this.tabConfig.Size = new System.Drawing.Size(479, 399);
            this.tabConfig.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.btnDeleteSkill);
            this.tabPage1.Controls.Add(this.btnAddSkill);
            this.tabPage1.Controls.Add(this.listView2);
            this.tabPage1.Controls.Add(this.listView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(471, 373);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Kỹ Năng";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox9);
            this.groupBox2.Controls.Add(this.button19);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.textBox8);
            this.groupBox2.Controls.Add(this.textBox7);
            this.groupBox2.Location = new System.Drawing.Point(239, 291);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(226, 76);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Khu vực luyện";
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(27, 47);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(55, 20);
            this.textBox9.TabIndex = 5;
            // 
            // button19
            // 
            this.button19.Location = new System.Drawing.Point(91, 47);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(89, 23);
            this.button19.TabIndex = 4;
            this.button19.Text = "Lấy tọa độ";
            this.button19.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(88, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Y:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "X:";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(107, 18);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(55, 20);
            this.textBox8.TabIndex = 1;
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(27, 18);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(55, 20);
            this.textBox7.TabIndex = 0;
            // 
            // btnDeleteSkill
            // 
            this.btnDeleteSkill.Location = new System.Drawing.Point(363, 257);
            this.btnDeleteSkill.Name = "btnDeleteSkill";
            this.btnDeleteSkill.Size = new System.Drawing.Size(102, 23);
            this.btnDeleteSkill.TabIndex = 3;
            this.btnDeleteSkill.Text = "Xóa";
            this.btnDeleteSkill.UseVisualStyleBackColor = true;
            // 
            // btnAddSkill
            // 
            this.btnAddSkill.Location = new System.Drawing.Point(239, 257);
            this.btnAddSkill.Name = "btnAddSkill";
            this.btnAddSkill.Size = new System.Drawing.Size(110, 23);
            this.btnAddSkill.TabIndex = 2;
            this.btnAddSkill.Text = "Thêm kỹ năng";
            this.btnAddSkill.UseVisualStyleBackColor = true;
            // 
            // listView2
            // 
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSkillUseName,
            this.colSkillType});
            this.listView2.Location = new System.Drawing.Point(235, 9);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(230, 242);
            this.listView2.TabIndex = 1;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // colSkillUseName
            // 
            this.colSkillUseName.Text = "Tên Kỹ năng";
            this.colSkillUseName.Width = 150;
            // 
            // colSkillType
            // 
            this.colSkillType.Text = "Loại";
            this.colSkillType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colSkillType.Width = 70;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSkillName,
            this.colLevel});
            this.listView1.Location = new System.Drawing.Point(8, 9);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(221, 358);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // colSkillName
            // 
            this.colSkillName.Text = "Tên Kỹ năng";
            this.colSkillName.Width = 150;
            // 
            // colLevel
            // 
            this.colLevel.Text = "Cấp độ";
            this.colLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colLevel.Width = 50;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(471, 373);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Bảo vệ";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Location = new System.Drawing.Point(42, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 98);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(19, 56);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(125, 17);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "Ngắt kết nối khi chết";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(19, 33);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(132, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Tự chấp nhận hồi sinh";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tabControl1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(471, 373);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Lựa chọn";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Location = new System.Drawing.Point(5, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(463, 361);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(455, 335);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Lựa chọn chung";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.button17);
            this.tabPage5.Controls.Add(this.textBox4);
            this.tabPage5.Controls.Add(this.button18);
            this.tabPage5.Controls.Add(this.textBox5);
            this.tabPage5.Controls.Add(this.label6);
            this.tabPage5.Controls.Add(this.textBox6);
            this.tabPage5.Controls.Add(this.label7);
            this.tabPage5.Controls.Add(this.button10);
            this.tabPage5.Controls.Add(this.button9);
            this.tabPage5.Controls.Add(this.label3);
            this.tabPage5.Controls.Add(this.comboBox1);
            this.tabPage5.Controls.Add(this.button6);
            this.tabPage5.Controls.Add(this.button7);
            this.tabPage5.Controls.Add(this.button8);
            this.tabPage5.Controls.Add(this.button3);
            this.tabPage5.Controls.Add(this.button4);
            this.tabPage5.Controls.Add(this.button2);
            this.tabPage5.Controls.Add(this.button1);
            this.tabPage5.Controls.Add(this.listView3);
            this.tabPage5.Controls.Add(this.button5);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(455, 335);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "Nhặt đồ";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(399, 278);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(45, 24);
            this.button17.TabIndex = 19;
            this.button17.Text = "Tìm";
            this.button17.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(279, 281);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(114, 20);
            this.textBox4.TabIndex = 18;
            // 
            // button18
            // 
            this.button18.Location = new System.Drawing.Point(399, 252);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(45, 24);
            this.button18.TabIndex = 17;
            this.button18.Text = "Tìm";
            this.button18.UseVisualStyleBackColor = true;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(360, 253);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(33, 20);
            this.textBox5.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(335, 253);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "đến";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(299, 253);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(33, 20);
            this.textBox6.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(276, 253);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Cấp";
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(381, 221);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(63, 21);
            this.button10.TabIndex = 12;
            this.button10.Text = "Tất cả";
            this.button10.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(300, 221);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 21);
            this.button9.TabIndex = 11;
            this.button9.Text = "Đồ đã chọn";
            this.button9.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(271, 193);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Kiểu nhặt";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Không nhặt",
            "Thùng đồ",
            "Kho đồ",
            "Vứt bỏ"});
            this.comboBox1.Location = new System.Drawing.Point(335, 193);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(109, 21);
            this.comboBox1.TabIndex = 9;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(134, 220);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(120, 23);
            this.button6.TabIndex = 7;
            this.button6.Text = "Bùa dịch chuyển";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(134, 278);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(120, 23);
            this.button7.TabIndex = 6;
            this.button7.Text = "Tên";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(134, 191);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(120, 23);
            this.button8.TabIndex = 5;
            this.button8.Text = "Thuốc hồi phục";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(8, 278);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(120, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "Tấm lót";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(8, 249);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(120, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "Nguyên liệu giả kim";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(8, 220);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Luyện kim dược";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 191);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Đồ vật - trang bị";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // listView3
            // 
            this.listView3.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView3.Location = new System.Drawing.Point(8, 6);
            this.listView3.Name = "listView3";
            this.listView3.Size = new System.Drawing.Size(438, 173);
            this.listView3.TabIndex = 0;
            this.listView3.UseCompatibleStateImageBehavior = false;
            this.listView3.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Tên đồ vật";
            this.columnHeader1.Width = 242;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Cấp độ";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Nhặt";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 78;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(134, 249);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(120, 23);
            this.button5.TabIndex = 8;
            this.button5.Text = "Đồ vật nhiệm vụ";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.button16);
            this.tabPage6.Controls.Add(this.textBox3);
            this.tabPage6.Controls.Add(this.button15);
            this.tabPage6.Controls.Add(this.textBox2);
            this.tabPage6.Controls.Add(this.label5);
            this.tabPage6.Controls.Add(this.textBox1);
            this.tabPage6.Controls.Add(this.label4);
            this.tabPage6.Controls.Add(this.button14);
            this.tabPage6.Controls.Add(this.button13);
            this.tabPage6.Controls.Add(this.button12);
            this.tabPage6.Controls.Add(this.button11);
            this.tabPage6.Controls.Add(this.listView4);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(455, 335);
            this.tabPage6.TabIndex = 2;
            this.tabPage6.Text = "Quái vật";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(407, 32);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(45, 24);
            this.button16.TabIndex = 12;
            this.button16.Text = "Tìm";
            this.button16.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(287, 35);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(114, 20);
            this.textBox3.TabIndex = 11;
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(407, 6);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(45, 24);
            this.button15.TabIndex = 10;
            this.button15.Text = "Tìm";
            this.button15.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(368, 7);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(33, 20);
            this.textBox2.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(343, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "đến";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(307, 7);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(33, 20);
            this.textBox1.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(284, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Cấp";
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(284, 165);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(165, 26);
            this.button14.TabIndex = 5;
            this.button14.Text = "Không đánh quái vật";
            this.button14.UseVisualStyleBackColor = true;
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(284, 133);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(165, 26);
            this.button13.TabIndex = 4;
            this.button13.Text = "Không đánh quái vật đã chọn";
            this.button13.UseVisualStyleBackColor = true;
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(284, 101);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(165, 26);
            this.button12.TabIndex = 3;
            this.button12.Text = "Đánh quái vật đã chọn";
            this.button12.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(284, 69);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(165, 26);
            this.button11.TabIndex = 2;
            this.button11.Text = "Đánh tất cả quái vật";
            this.button11.UseVisualStyleBackColor = true;
            // 
            // listView4
            // 
            this.listView4.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.listView4.Location = new System.Drawing.Point(6, 6);
            this.listView4.Name = "listView4";
            this.listView4.Size = new System.Drawing.Size(272, 323);
            this.listView4.TabIndex = 1;
            this.listView4.UseCompatibleStateImageBehavior = false;
            this.listView4.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Tên quái vật";
            this.columnHeader4.Width = 147;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Cấp độ";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader5.Width = 50;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Đánh";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader6.Width = 65;
            // 
            // rtbLog
            // 
            this.rtbLog.Location = new System.Drawing.Point(12, 417);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(658, 253);
            this.rtbLog.TabIndex = 6;
            this.rtbLog.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 695);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.tabConfig);
            this.Controls.Add(this.gpbCharInfo);
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
            this.gpbCharInfo.ResumeLayout(false);
            this.gpbCharInfo.PerformLayout();
            this.tabConfig.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
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
        private System.Windows.Forms.GroupBox gpbCharInfo;
        private System.Windows.Forms.Label lblCharCoord;
        private System.Windows.Forms.ProgressBar pbCharMP;
        private System.Windows.Forms.ProgressBar pbCharHP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCharName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabConfig;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Button btnDeleteSkill;
        private System.Windows.Forms.Button btnAddSkill;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader colSkillUseName;
        private System.Windows.Forms.ColumnHeader colSkillType;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader colSkillName;
        private System.Windows.Forms.ColumnHeader colLevel;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listView3;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.ListView listView4;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.RichTextBox rtbLog;
    }
}

