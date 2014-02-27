namespace Hatchu
{
    partial class DanceAddScreen
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
            this.danceTypeTxt = new System.Windows.Forms.TextBox();
            this.danceTypeBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Add Dance Types";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // danceTypeTxt
            // 
            this.danceTypeTxt.Location = new System.Drawing.Point(12, 41);
            this.danceTypeTxt.Multiline = true;
            this.danceTypeTxt.Name = "danceTypeTxt";
            this.danceTypeTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.danceTypeTxt.Size = new System.Drawing.Size(110, 208);
            this.danceTypeTxt.TabIndex = 1;
            this.danceTypeTxt.TextChanged += new System.EventHandler(this.danceTypeTxt_TextChanged);
            // 
            // danceTypeBtn
            // 
            this.danceTypeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.danceTypeBtn.Location = new System.Drawing.Point(12, 262);
            this.danceTypeBtn.Name = "danceTypeBtn";
            this.danceTypeBtn.Size = new System.Drawing.Size(110, 23);
            this.danceTypeBtn.TabIndex = 2;
            this.danceTypeBtn.Text = "Save";
            this.danceTypeBtn.UseVisualStyleBackColor = false;
            this.danceTypeBtn.Click += new System.EventHandler(this.danceTypeBtn_Click);
            // 
            // DanceAddScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(135, 297);
            this.Controls.Add(this.danceTypeBtn);
            this.Controls.Add(this.danceTypeTxt);
            this.Controls.Add(this.label1);
            this.Name = "DanceAddScreen";
            this.Text = "DanceAddScreen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox danceTypeTxt;
        private System.Windows.Forms.Button danceTypeBtn;
    }
}