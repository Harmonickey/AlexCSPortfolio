namespace Encryption_Decryption_Program
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
            this.fileBox_Encrypt = new System.Windows.Forms.TextBox();
            this.fileSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.fileSearch_2 = new System.Windows.Forms.Button();
            this.fileBox_Decrypt = new System.Windows.Forms.TextBox();
            this.encrypt = new System.Windows.Forms.Button();
            this.decrypt = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.encryptKey = new System.Windows.Forms.TextBox();
            this.decryptKey = new System.Windows.Forms.TextBox();
            this.encryptModulus = new System.Windows.Forms.TextBox();
            this.decryptModulus = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // fileBox_Encrypt
            // 
            this.fileBox_Encrypt.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.fileBox_Encrypt.Location = new System.Drawing.Point(33, 76);
            this.fileBox_Encrypt.Name = "fileBox_Encrypt";
            this.fileBox_Encrypt.ReadOnly = true;
            this.fileBox_Encrypt.Size = new System.Drawing.Size(176, 20);
            this.fileBox_Encrypt.TabIndex = 0;
            // 
            // fileSearch
            // 
            this.fileSearch.Location = new System.Drawing.Point(215, 76);
            this.fileSearch.Name = "fileSearch";
            this.fileSearch.Size = new System.Drawing.Size(26, 20);
            this.fileSearch.TabIndex = 1;
            this.fileSearch.Text = "...";
            this.fileSearch.UseVisualStyleBackColor = true;
            this.fileSearch.Click += new System.EventHandler(this.fileSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(30, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "Choose A File to Encrypt";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(30, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "Choose A File to Decrypt";
            // 
            // fileSearch_2
            // 
            this.fileSearch_2.Location = new System.Drawing.Point(215, 156);
            this.fileSearch_2.Name = "fileSearch_2";
            this.fileSearch_2.Size = new System.Drawing.Size(26, 20);
            this.fileSearch_2.TabIndex = 4;
            this.fileSearch_2.Text = "...";
            this.fileSearch_2.UseVisualStyleBackColor = true;
            this.fileSearch_2.Click += new System.EventHandler(this.fileSearch_2_Click);
            // 
            // fileBox_Decrypt
            // 
            this.fileBox_Decrypt.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.fileBox_Decrypt.Location = new System.Drawing.Point(33, 156);
            this.fileBox_Decrypt.Name = "fileBox_Decrypt";
            this.fileBox_Decrypt.ReadOnly = true;
            this.fileBox_Decrypt.Size = new System.Drawing.Size(176, 20);
            this.fileBox_Decrypt.TabIndex = 3;
            // 
            // encrypt
            // 
            this.encrypt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.encrypt.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.encrypt.Location = new System.Drawing.Point(516, 62);
            this.encrypt.Name = "encrypt";
            this.encrypt.Size = new System.Drawing.Size(99, 34);
            this.encrypt.TabIndex = 6;
            this.encrypt.Text = "ENCRYPT";
            this.encrypt.UseVisualStyleBackColor = true;
            this.encrypt.Click += new System.EventHandler(this.encrypt_Click);
            // 
            // decrypt
            // 
            this.decrypt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.decrypt.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.decrypt.Location = new System.Drawing.Point(516, 142);
            this.decrypt.Name = "decrypt";
            this.decrypt.Size = new System.Drawing.Size(99, 34);
            this.decrypt.TabIndex = 7;
            this.decrypt.Text = "DECRYPT";
            this.decrypt.UseVisualStyleBackColor = true;
            this.decrypt.Click += new System.EventHandler(this.decrypt_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(248, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Encryption Key";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(248, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Decryption Key";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(268, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "K = ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(268, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 20);
            this.label6.TabIndex = 11;
            this.label6.Text = "K =";
            // 
            // encryptKey
            // 
            this.encryptKey.Location = new System.Drawing.Point(310, 76);
            this.encryptKey.MaxLength = 3;
            this.encryptKey.Name = "encryptKey";
            this.encryptKey.Size = new System.Drawing.Size(29, 20);
            this.encryptKey.TabIndex = 12;
            this.encryptKey.WordWrap = false;
            // 
            // decryptKey
            // 
            this.decryptKey.Location = new System.Drawing.Point(310, 156);
            this.decryptKey.MaxLength = 3;
            this.decryptKey.Name = "decryptKey";
            this.decryptKey.Size = new System.Drawing.Size(29, 20);
            this.decryptKey.TabIndex = 13;
            this.decryptKey.WordWrap = false;
            // 
            // encryptModulus
            // 
            this.encryptModulus.Location = new System.Drawing.Point(437, 76);
            this.encryptModulus.MaxLength = 3;
            this.encryptModulus.Name = "encryptModulus";
            this.encryptModulus.Size = new System.Drawing.Size(33, 20);
            this.encryptModulus.TabIndex = 14;
            this.encryptModulus.WordWrap = false;
            // 
            // decryptModulus
            // 
            this.decryptModulus.Location = new System.Drawing.Point(437, 156);
            this.decryptModulus.MaxLength = 3;
            this.decryptModulus.Name = "decryptModulus";
            this.decryptModulus.Size = new System.Drawing.Size(33, 20);
            this.decryptModulus.TabIndex = 15;
            this.decryptModulus.WordWrap = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(388, 128);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 20);
            this.label7.TabIndex = 16;
            this.label7.Text = "Modulus Key";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(387, 50);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(99, 20);
            this.label8.TabIndex = 17;
            this.label8.Text = "Modulus Key";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(374, 156);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 20);
            this.label9.TabIndex = 18;
            this.label9.Text = "Mod = ";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(374, 76);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 20);
            this.label10.TabIndex = 19;
            this.label10.Text = "Mod = ";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(30, 221);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(232, 24);
            this.label11.TabIndex = 20;
            this.label11.Text = "Created By: Alex Ayerdi";
            // 
            // textBox2
            // 
            this.textBox2.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(298, 221);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(298, 89);
            this.textBox2.TabIndex = 22;
            this.textBox2.Text = "Warning!! If you use the decrypt then you must insert the correct key and modulus" +
    " the FIRST time otherwise your data will be lost in a mush of utterly useless st" +
    "ring of letters and such stuffs.";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(161, 19);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(335, 20);
            this.label13.TabIndex = 23;
            this.label13.Text = "TEXT FILE DECRYPTION-ENCRYPTION";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 328);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.decryptModulus);
            this.Controls.Add(this.encryptModulus);
            this.Controls.Add(this.decryptKey);
            this.Controls.Add(this.encryptKey);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.decrypt);
            this.Controls.Add(this.encrypt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.fileSearch_2);
            this.Controls.Add(this.fileBox_Decrypt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fileSearch);
            this.Controls.Add(this.fileBox_Encrypt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Encryption-Decryption";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox fileBox_Encrypt;
        private System.Windows.Forms.Button fileSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button fileSearch_2;
        private System.Windows.Forms.TextBox fileBox_Decrypt;
        private System.Windows.Forms.Button encrypt;
        private System.Windows.Forms.Button decrypt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox encryptKey;
        private System.Windows.Forms.TextBox decryptKey;
        private System.Windows.Forms.TextBox encryptModulus;
        private System.Windows.Forms.TextBox decryptModulus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;

    }
}

