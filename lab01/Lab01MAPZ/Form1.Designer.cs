namespace Lab01MAPZ
{
    partial class Form1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.Runbutton = new System.Windows.Forms.Button();
            this.Backbutton = new System.Windows.Forms.Button();
            this.Homebutton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ProgramtextBox = new System.Windows.Forms.RichTextBox();
            this.HTMLtextBox = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ProgramtextBox);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(486, 591);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.webBrowser1);
            this.panel2.Location = new System.Drawing.Point(989, 38);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(535, 616);
            this.panel2.TabIndex = 1;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(535, 616);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.Url = new System.Uri("https://www.w3schools.com/", System.UriKind.Absolute);
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted_1);
            // 
            // Runbutton
            // 
            this.Runbutton.BackColor = System.Drawing.Color.ForestGreen;
            this.Runbutton.Location = new System.Drawing.Point(12, 609);
            this.Runbutton.Name = "Runbutton";
            this.Runbutton.Size = new System.Drawing.Size(486, 45);
            this.Runbutton.TabIndex = 0;
            this.Runbutton.Text = "RUN";
            this.Runbutton.UseVisualStyleBackColor = false;
            this.Runbutton.Click += new System.EventHandler(this.Runbutton_Click);
            // 
            // Backbutton
            // 
            this.Backbutton.Location = new System.Drawing.Point(989, 3);
            this.Backbutton.Name = "Backbutton";
            this.Backbutton.Size = new System.Drawing.Size(75, 23);
            this.Backbutton.TabIndex = 3;
            this.Backbutton.Text = "Back";
            this.Backbutton.UseVisualStyleBackColor = true;
            this.Backbutton.Click += new System.EventHandler(this.Backbutton_Click);
            // 
            // Homebutton
            // 
            this.Homebutton.Location = new System.Drawing.Point(1070, 2);
            this.Homebutton.Name = "Homebutton";
            this.Homebutton.Size = new System.Drawing.Size(75, 23);
            this.Homebutton.TabIndex = 4;
            this.Homebutton.Text = "Home";
            this.Homebutton.UseVisualStyleBackColor = true;
            this.Homebutton.Click += new System.EventHandler(this.Homebutton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(505, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "HTML file";
            // 
            // ProgramtextBox
            // 
            this.ProgramtextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ProgramtextBox.Location = new System.Drawing.Point(3, 4);
            this.ProgramtextBox.Name = "ProgramtextBox";
            this.ProgramtextBox.Size = new System.Drawing.Size(480, 584);
            this.ProgramtextBox.TabIndex = 0;
            this.ProgramtextBox.Text = "";
            // 
            // HTMLtextBox
            // 
            this.HTMLtextBox.Location = new System.Drawing.Point(508, 38);
            this.HTMLtextBox.Name = "HTMLtextBox";
            this.HTMLtextBox.Size = new System.Drawing.Size(464, 616);
            this.HTMLtextBox.TabIndex = 7;
            this.HTMLtextBox.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1532, 673);
            this.Controls.Add(this.HTMLtextBox);
            this.Controls.Add(this.Runbutton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Homebutton);
            this.Controls.Add(this.Backbutton);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button Runbutton;
        private System.Windows.Forms.Button Backbutton;
        private System.Windows.Forms.Button Homebutton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox ProgramtextBox;
        private System.Windows.Forms.RichTextBox HTMLtextBox;
    }
}

