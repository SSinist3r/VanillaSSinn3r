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
            this.camDistCheckBox = new System.Windows.Forms.CheckBox();
            this.camDistSlider = new System.Windows.Forms.TrackBar();
            this.freezeCheckBox = new System.Windows.Forms.CheckBox();
            this.debugCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.namePlateRangeSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camDistSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // namePlateRangeSlider
            // 
            this.namePlateRangeSlider.Location = new System.Drawing.Point(137, 273);
            this.namePlateRangeSlider.Maximum = 1000;
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
            this.processBox.Location = new System.Drawing.Point(12, 203);
            this.processBox.Name = "processBox";
            this.processBox.Size = new System.Drawing.Size(303, 28);
            this.processBox.TabIndex = 1;
            // 
            // refreshBtn
            // 
            this.refreshBtn.AutoSize = true;
            this.refreshBtn.Location = new System.Drawing.Point(321, 202);
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
            this.attachBtn.Location = new System.Drawing.Point(94, 237);
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
            this.infoBox.Location = new System.Drawing.Point(12, 37);
            this.infoBox.Name = "infoBox";
            this.infoBox.ReadOnly = true;
            this.infoBox.Size = new System.Drawing.Size(387, 160);
            this.infoBox.TabIndex = 22;
            this.infoBox.Text = "";
            this.infoBox.TextChanged += new System.EventHandler(this.infoBox_TextChanged);
            // 
            // fovLabel
            // 
            this.fovLabel.AutoSize = true;
            this.fovLabel.Location = new System.Drawing.Point(12, 322);
            this.fovLabel.Name = "fovLabel";
            this.fovLabel.Size = new System.Drawing.Size(396, 13);
            this.fovLabel.TabIndex = 23;
            this.fovLabel.Text = "FOV Set Value :                                                                  " +
    "           (Default - 1.5708)";
            // 
            // fovTextBox
            // 
            this.fovTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.fovTextBox.Location = new System.Drawing.Point(137, 316);
            this.fovTextBox.Name = "fovTextBox";
            this.fovTextBox.Size = new System.Drawing.Size(98, 26);
            this.fovTextBox.TabIndex = 24;
            this.fovTextBox.Text = "1.5708";
            // 
            // fovSetBtn
            // 
            this.fovSetBtn.AutoSize = true;
            this.fovSetBtn.Location = new System.Drawing.Point(242, 315);
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
            this.namePlateCheckBox.Location = new System.Drawing.Point(12, 280);
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
            this.fovCheckBox.Location = new System.Drawing.Point(12, 320);
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
            this.resetBtn.Location = new System.Drawing.Point(12, 237);
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
            this.saveBtn.Location = new System.Drawing.Point(321, 237);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(76, 30);
            this.saveBtn.TabIndex = 29;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // camDistCheckBox
            // 
            this.camDistCheckBox.AutoSize = true;
            this.camDistCheckBox.Location = new System.Drawing.Point(12, 358);
            this.camDistCheckBox.Name = "camDistCheckBox";
            this.camDistCheckBox.Size = new System.Drawing.Size(113, 17);
            this.camDistCheckBox.TabIndex = 31;
            this.camDistCheckBox.Text = "Camera Distance: ";
            this.camDistCheckBox.UseVisualStyleBackColor = true;
            this.camDistCheckBox.CheckedChanged += new System.EventHandler(this.camDistCheckBox_CheckedChanged);
            // 
            // camDistSlider
            // 
            this.camDistSlider.Location = new System.Drawing.Point(137, 351);
            this.camDistSlider.Maximum = 1000;
            this.camDistSlider.Name = "camDistSlider";
            this.camDistSlider.Size = new System.Drawing.Size(196, 45);
            this.camDistSlider.TabIndex = 30;
            this.camDistSlider.Scroll += new System.EventHandler(this.camDistSlider_Scroll);
            // 
            // freezeCheckBox
            // 
            this.freezeCheckBox.AutoSize = true;
            this.freezeCheckBox.Location = new System.Drawing.Point(339, 358);
            this.freezeCheckBox.Name = "freezeCheckBox";
            this.freezeCheckBox.Size = new System.Drawing.Size(58, 17);
            this.freezeCheckBox.TabIndex = 32;
            this.freezeCheckBox.Text = "Freeze";
            this.freezeCheckBox.UseVisualStyleBackColor = true;
            this.freezeCheckBox.CheckedChanged += new System.EventHandler(this.freezeCheckBox_CheckedChanged);
            // 
            // debugCheckBox
            // 
            this.debugCheckBox.AutoSize = true;
            this.debugCheckBox.Location = new System.Drawing.Point(12, 12);
            this.debugCheckBox.Name = "debugCheckBox";
            this.debugCheckBox.Size = new System.Drawing.Size(144, 17);
            this.debugCheckBox.TabIndex = 33;
            this.debugCheckBox.Text = "ENABLE DEBUG MODE";
            this.debugCheckBox.UseVisualStyleBackColor = true;
            this.debugCheckBox.CheckedChanged += new System.EventHandler(this.debugCheckBox_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 400);
            this.Controls.Add(this.debugCheckBox);
            this.Controls.Add(this.freezeCheckBox);
            this.Controls.Add(this.camDistCheckBox);
            this.Controls.Add(this.camDistSlider);
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.namePlateRangeSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camDistSlider)).EndInit();
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
        private System.Windows.Forms.CheckBox camDistCheckBox;
        private System.Windows.Forms.TrackBar camDistSlider;
        private System.Windows.Forms.CheckBox freezeCheckBox;
        private System.Windows.Forms.CheckBox debugCheckBox;
    }
}

