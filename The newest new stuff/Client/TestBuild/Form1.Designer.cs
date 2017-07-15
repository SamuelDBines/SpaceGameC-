namespace Client_Login
{
    partial class clientform
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
            this.usernameLabel = new System.Windows.Forms.Label();
            this.loginLabel = new System.Windows.Forms.Label();
            this.openworldButton = new System.Windows.Forms.Button();
            this.Quit = new System.Windows.Forms.Button();
            this.usernameTextField = new System.Windows.Forms.TextBox();
            this.oneVsoneServer = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(62, 43);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(61, 13);
            this.usernameLabel.TabIndex = 0;
            this.usernameLabel.Text = "Username :";
            // 
            // loginLabel
            // 
            this.loginLabel.AutoSize = true;
            this.loginLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginLabel.Location = new System.Drawing.Point(126, 9);
            this.loginLabel.Name = "loginLabel";
            this.loginLabel.Size = new System.Drawing.Size(73, 29);
            this.loginLabel.TabIndex = 2;
            this.loginLabel.Text = "Login";
            // 
            // openworldButton
            // 
            this.openworldButton.Location = new System.Drawing.Point(46, 159);
            this.openworldButton.Name = "openworldButton";
            this.openworldButton.Size = new System.Drawing.Size(121, 61);
            this.openworldButton.TabIndex = 6;
            this.openworldButton.Text = "Openworld";
            this.openworldButton.UseVisualStyleBackColor = true;
            this.openworldButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // Quit
            // 
            this.Quit.Location = new System.Drawing.Point(117, 243);
            this.Quit.Name = "Quit";
            this.Quit.Size = new System.Drawing.Size(121, 61);
            this.Quit.TabIndex = 7;
            this.Quit.Text = "Quit";
            this.Quit.UseVisualStyleBackColor = true;
            this.Quit.Click += new System.EventHandler(this.Quit_Click);
            // 
            // usernameTextField
            // 
            this.usernameTextField.Location = new System.Drawing.Point(129, 43);
            this.usernameTextField.Name = "usernameTextField";
            this.usernameTextField.Size = new System.Drawing.Size(100, 20);
            this.usernameTextField.TabIndex = 12;
            this.usernameTextField.TextChanged += new System.EventHandler(this.usernameTextField_TextChanged);
            // 
            // oneVsoneServer
            // 
            this.oneVsoneServer.Location = new System.Drawing.Point(173, 159);
            this.oneVsoneServer.Name = "oneVsoneServer";
            this.oneVsoneServer.Size = new System.Drawing.Size(121, 61);
            this.oneVsoneServer.TabIndex = 16;
            this.oneVsoneServer.Text = "1 V 1";
            this.oneVsoneServer.UseVisualStyleBackColor = true;
            this.oneVsoneServer.Click += new System.EventHandler(this.oneVsoneServer_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Enabled = false;
            this.richTextBox1.Location = new System.Drawing.Point(366, 31);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(248, 96);
            this.richTextBox1.TabIndex = 17;
            this.richTextBox1.Text = "";
            // 
            // clientform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 316);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.oneVsoneServer);
            this.Controls.Add(this.usernameTextField);
            this.Controls.Add(this.Quit);
            this.Controls.Add(this.openworldButton);
            this.Controls.Add(this.loginLabel);
            this.Controls.Add(this.usernameLabel);
            this.Name = "clientform";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.Label loginLabel;
        private System.Windows.Forms.Button openworldButton;
        private System.Windows.Forms.Button Quit;
        private System.Windows.Forms.TextBox usernameTextField;
        private System.Windows.Forms.Button oneVsoneServer;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}

