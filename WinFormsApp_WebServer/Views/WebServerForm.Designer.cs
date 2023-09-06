namespace WinFormsApp_WebServer.Views
{
    partial class WebServerForm
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
            btnStart = new Button();
            btnStop = new Button();
            label1 = new Label();
            textBox1 = new TextBox();
            tbPort = new TextBox();
            label2 = new Label();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Location = new Point(60, 114);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(75, 23);
            btnStart.TabIndex = 0;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(177, 114);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(75, 23);
            btnStop.TabIndex = 1;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(60, 18);
            label1.Name = "label1";
            label1.Size = new Size(57, 15);
            label1.TabIndex = 2;
            label1.Text = "Server Url";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(60, 36);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(94, 23);
            textBox1.TabIndex = 3;
            textBox1.Text = "http://localhost:";
            // 
            // tbPort
            // 
            tbPort.Location = new Point(152, 36);
            tbPort.MaxLength = 5;
            tbPort.Name = "tbPort";
            tbPort.PlaceholderText = "23005";
            tbPort.Size = new Size(100, 23);
            tbPort.TabIndex = 4;
            tbPort.KeyPress += tbPort_KeyPress;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(152, 18);
            label2.Name = "label2";
            label2.Size = new Size(64, 15);
            label2.TabIndex = 5;
            label2.Text = "Server Port";
            // 
            // WebServerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(320, 157);
            Controls.Add(label2);
            Controls.Add(tbPort);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "WebServerForm";
            ShowIcon = false;
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "WebServer";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnStart;
        private Button btnStop;
        private Label label1;
        private TextBox textBox1;
        private TextBox tbPort;
        private Label label2;
    }
}