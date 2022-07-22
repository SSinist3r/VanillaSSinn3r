using System.Drawing;

namespace VanillaSSinn3r
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.namePlateRangeSlider = new System.Windows.Forms.TrackBar();
            this.processBox = new System.Windows.Forms.ComboBox();
            this.refreshBtn = new System.Windows.Forms.Button();
            this.attachBtn = new System.Windows.Forms.Button();
            this.infoBox = new System.Windows.Forms.RichTextBox();
            this.fovLabel = new System.Windows.Forms.Label();
            this.fovTextBox = new System.Windows.Forms.TextBox();
            this.fovSetBtn = new System.Windows.Forms.Button();
            this.namePlateCheckBox = new System.Windows.Forms.CheckBox();
            this.fovCheckBox = new System.Windows.Forms.CheckBox();
            this.resetBtn = new System.Windows.Forms.Button();
            this.saveBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.namePlateRangeSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // namePlateRangeSlider
            // 
            this.namePlateRangeSlider.Location = new System.Drawing.Point(137, 248);
            this.namePlateRangeSlider.Maximum = 250000;
            this.namePlateRangeSlider.Name = "namePlateRangeSlider";
            this.namePlateRangeSlider.Size = new System.Drawing.Size(260, 45);
            this.namePlateRangeSlider.TabIndex = 0;
            this.namePlateRangeSlider.Value = 20;
            this.namePlateRangeSlider.Scroll += new System.EventHandler(this.namePlateRangeSlider_Scroll);
            // 
            // processBox
            // 
            this.processBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.processBox.FormattingEnabled = true;
            this.processBox.Location = new System.Drawing.Point(12, 178);
            this.processBox.Name = "processBox";
            this.processBox.Size = new System.Drawing.Size(303, 28);
            this.processBox.TabIndex = 1;
            // 
            // refreshBtn
            // 
            this.refreshBtn.AutoSize = true;
            this.refreshBtn.Location = new System.Drawing.Point(321, 177);
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.Size = new System.Drawing.Size(76, 30);
            this.refreshBtn.TabIndex = 2;
            this.refreshBtn.Text = "Refresh";
            this.refreshBtn.UseVisualStyleBackColor = true;
            this.refreshBtn.Click += new System.EventHandler(this.refreshBtn_Click);
            // 
            // attachBtn
            // 
            this.attachBtn.AutoSize = true;
            this.attachBtn.Location = new System.Drawing.Point(94, 212);
            this.attachBtn.Name = "attachBtn";
            this.attachBtn.Size = new System.Drawing.Size(221, 30);
            this.attachBtn.TabIndex = 3;
            this.attachBtn.Text = "    Attach";
            this.attachBtn.UseVisualStyleBackColor = true;
            this.attachBtn.Click += new System.EventHandler(this.attachBtn_Click);
            // 
            // infoBox
            // 
            this.infoBox.BackColor = System.Drawing.Color.Black;
            this.infoBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.infoBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoBox.ForeColor = System.Drawing.Color.Chartreuse;
            this.infoBox.HideSelection = false;
            this.infoBox.Location = new System.Drawing.Point(12, 12);
            this.infoBox.Name = "infoBox";
            this.infoBox.ReadOnly = true;
            this.infoBox.Size = new System.Drawing.Size(387, 160);
            this.infoBox.TabIndex = 22;
            this.infoBox.Text = "";
            // 
            // fovLabel
            // 
            this.fovLabel.AutoSize = true;
            this.fovLabel.Location = new System.Drawing.Point(12, 297);
            this.fovLabel.Name = "fovLabel";
            this.fovLabel.Size = new System.Drawing.Size(396, 13);
            this.fovLabel.TabIndex = 23;
            this.fovLabel.Text = "FOV Set Value :                                                                  " +
    "           (Default - 1.5708)";
            // 
            // fovTextBox
            // 
            this.fovTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.fovTextBox.Location = new System.Drawing.Point(137, 291);
            this.fovTextBox.Name = "fovTextBox";
            this.fovTextBox.Size = new System.Drawing.Size(98, 26);
            this.fovTextBox.TabIndex = 24;
            this.fovTextBox.Text = "1.5708";
            // 
            // fovSetBtn
            // 
            this.fovSetBtn.AutoSize = true;
            this.fovSetBtn.Location = new System.Drawing.Point(242, 290);
            this.fovSetBtn.Name = "fovSetBtn";
            this.fovSetBtn.Size = new System.Drawing.Size(76, 30);
            this.fovSetBtn.TabIndex = 25;
            this.fovSetBtn.Text = "Set";
            this.fovSetBtn.UseVisualStyleBackColor = true;
            this.fovSetBtn.Click += new System.EventHandler(this.fovSetBtn_Click);
            // 
            // namePlateCheckBox
            // 
            this.namePlateCheckBox.AutoSize = true;
            this.namePlateCheckBox.Location = new System.Drawing.Point(12, 255);
            this.namePlateCheckBox.Name = "namePlateCheckBox";
            this.namePlateCheckBox.Size = new System.Drawing.Size(119, 17);
            this.namePlateCheckBox.TabIndex = 26;
            this.namePlateCheckBox.Text = "NamePlate Range: ";
            this.namePlateCheckBox.UseVisualStyleBackColor = true;
            this.namePlateCheckBox.CheckedChanged += new System.EventHandler(this.namePlateCheckBox_CheckedChanged);
            // 
            // fovCheckBox
            // 
            this.fovCheckBox.AutoSize = true;
            this.fovCheckBox.Location = new System.Drawing.Point(12, 295);
            this.fovCheckBox.Name = "fovCheckBox";
            this.fovCheckBox.Size = new System.Drawing.Size(105, 17);
            this.fovCheckBox.TabIndex = 27;
            this.fovCheckBox.Text = "FOV Set Value : ";
            this.fovCheckBox.UseVisualStyleBackColor = true;
            this.fovCheckBox.CheckedChanged += new System.EventHandler(this.fovCheckBox_CheckedChanged);
            // 
            // resetBtn
            // 
            this.resetBtn.AutoSize = true;
            this.resetBtn.Location = new System.Drawing.Point(12, 212);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(76, 30);
            this.resetBtn.TabIndex = 28;
            this.resetBtn.Text = "Reset";
            this.resetBtn.UseVisualStyleBackColor = true;
            this.resetBtn.Click += new System.EventHandler(this.resetBtn_Click);
            // 
            // saveBtn
            // 
            this.saveBtn.AutoSize = true;
            this.saveBtn.Location = new System.Drawing.Point(321, 212);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(76, 30);
            this.saveBtn.TabIndex = 29;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 332);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.resetBtn);
            this.Controls.Add(this.fovCheckBox);
            this.Controls.Add(this.namePlateCheckBox);
            this.Controls.Add(this.fovSetBtn);
            this.Controls.Add(this.fovTextBox);
            this.Controls.Add(this.fovLabel);
            this.Controls.Add(this.infoBox);
            this.Controls.Add(this.attachBtn);
            this.Controls.Add(this.refreshBtn);
            this.Controls.Add(this.processBox);
            this.Controls.Add(this.namePlateRangeSlider);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.namePlateRangeSlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar namePlateRangeSlider;
        private System.Windows.Forms.ComboBox processBox;
        private System.Windows.Forms.Button refreshBtn;
        private System.Windows.Forms.Button attachBtn;
        private System.Windows.Forms.RichTextBox infoBox;
        private System.Windows.Forms.Label fovLabel;
        private System.Windows.Forms.TextBox fovTextBox;
        private System.Windows.Forms.Button fovSetBtn;
        private System.Windows.Forms.CheckBox namePlateCheckBox;
        private System.Windows.Forms.CheckBox fovCheckBox;
        private System.Windows.Forms.Button resetBtn;
        private System.Windows.Forms.Button saveBtn;
    }
}

