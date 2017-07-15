namespace TestBuild
{
    partial class MainMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.IPTextBox = new System.Windows.Forms.MaskedTextBox();
            this.PortTextBox = new System.Windows.Forms.MaskedTextBox();
            this.UserNameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BlueBar = new System.Windows.Forms.TrackBar();
            this.RedBar = new System.Windows.Forms.TrackBar();
            this.GreenBar = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.PageForLocalServers = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BlueBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RedBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GreenBar)).BeginInit();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.PageForLocalServers.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 467);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Quit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(72, 86);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Join";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // IPTextBox
            // 
            this.IPTextBox.Location = new System.Drawing.Point(72, 23);
            this.IPTextBox.Mask = "000.000.000.000";
            this.IPTextBox.Name = "IPTextBox";
            this.IPTextBox.Size = new System.Drawing.Size(100, 20);
            this.IPTextBox.TabIndex = 2;
            this.IPTextBox.Text = "81 98 72 87";
            // 
            // PortTextBox
            // 
            this.PortTextBox.Location = new System.Drawing.Point(72, 46);
            this.PortTextBox.Mask = "0000";
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(100, 20);
            this.PortTextBox.TabIndex = 3;
            this.PortTextBox.Text = "8001";
            // 
            // UserNameTextBox
            // 
            this.UserNameTextBox.Location = new System.Drawing.Point(80, 204);
            this.UserNameTextBox.Name = "UserNameTextBox";
            this.UserNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.UserNameTextBox.TabIndex = 4;
            this.UserNameTextBox.TextChanged += new System.EventHandler(this.UserNameTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 207);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "User name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Ip address";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Server information";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.IPTextBox);
            this.panel1.Controls.Add(this.PortTextBox);
            this.panel1.Location = new System.Drawing.Point(3, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(184, 177);
            this.panel1.TabIndex = 9;
            // 
            // BlueBar
            // 
            this.BlueBar.AutoSize = false;
            this.BlueBar.BackColor = System.Drawing.SystemColors.Control;
            this.BlueBar.LargeChange = 20;
            this.BlueBar.Location = new System.Drawing.Point(75, 3);
            this.BlueBar.Maximum = 90;
            this.BlueBar.Name = "BlueBar";
            this.BlueBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.BlueBar.Size = new System.Drawing.Size(25, 147);
            this.BlueBar.SmallChange = 5;
            this.BlueBar.TabIndex = 10;
            this.BlueBar.TickFrequency = 10;
            this.BlueBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.BlueBar.Scroll += new System.EventHandler(this.BlueBar_Scroll);
            // 
            // RedBar
            // 
            this.RedBar.AutoSize = false;
            this.RedBar.LargeChange = 20;
            this.RedBar.Location = new System.Drawing.Point(3, 3);
            this.RedBar.Maximum = 90;
            this.RedBar.Name = "RedBar";
            this.RedBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.RedBar.Size = new System.Drawing.Size(27, 147);
            this.RedBar.SmallChange = 5;
            this.RedBar.TabIndex = 11;
            this.RedBar.TickFrequency = 10;
            this.RedBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.RedBar.Scroll += new System.EventHandler(this.RedBar_Scroll);
            // 
            // GreenBar
            // 
            this.GreenBar.AutoSize = false;
            this.GreenBar.LargeChange = 20;
            this.GreenBar.Location = new System.Drawing.Point(36, 3);
            this.GreenBar.Maximum = 90;
            this.GreenBar.Name = "GreenBar";
            this.GreenBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.GreenBar.Size = new System.Drawing.Size(33, 147);
            this.GreenBar.SmallChange = 5;
            this.GreenBar.TabIndex = 12;
            this.GreenBar.TickFrequency = 10;
            this.GreenBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.GreenBar.Scroll += new System.EventHandler(this.GreenBar_Scroll);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 153);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Green";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1, 153);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Red";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(72, 153);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(28, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Blue";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.BlueBar);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.GreenBar);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.RedBar);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(101, 177);
            this.panel2.TabIndex = 16;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.PageForLocalServers);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 249);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(252, 184);
            this.tabControl1.TabIndex = 17;
            // 
            // PageForLocalServers
            // 
            this.PageForLocalServers.Controls.Add(this.button3);
            this.PageForLocalServers.Location = new System.Drawing.Point(4, 22);
            this.PageForLocalServers.Name = "PageForLocalServers";
            this.PageForLocalServers.Padding = new System.Windows.Forms.Padding(3);
            this.PageForLocalServers.Size = new System.Drawing.Size(244, 158);
            this.PageForLocalServers.TabIndex = 0;
            this.PageForLocalServers.Text = "Local Servers";
            this.PageForLocalServers.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
            this.button3.Location = new System.Drawing.Point(206, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(31, 32);
            this.button3.TabIndex = 0;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(244, 158);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Manual connection";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 520);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.UserNameTextBox);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label1);
            this.Name = "MainMenu";
            this.Text = "MainMenu";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BlueBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RedBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GreenBar)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.PageForLocalServers.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.MaskedTextBox IPTextBox;
        private System.Windows.Forms.MaskedTextBox PortTextBox;
        private System.Windows.Forms.TextBox UserNameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TrackBar BlueBar;
        private System.Windows.Forms.TrackBar RedBar;
        private System.Windows.Forms.TrackBar GreenBar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage PageForLocalServers;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button3;
    }
}