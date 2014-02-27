namespace Hatchu
{
    partial class Hatchu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Hatchu));
            this.castListTitlelbl = new System.Windows.Forms.Label();
            this.castListTxt = new System.Windows.Forms.TextBox();
            this.castListDirLbl = new System.Windows.Forms.Label();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.solutionsContainer = new System.Windows.Forms.Panel();
            this.pieceNameLbl = new System.Windows.Forms.Label();
            this.castMembersLbl = new System.Windows.Forms.Label();
            this.castListLbl = new System.Windows.Forms.Label();
            this.danceTypeLbl = new System.Windows.Forms.Label();
            this.restartBtn = new System.Windows.Forms.Button();
            this.createBtn = new System.Windows.Forms.Button();
            this.numPiecesLbl = new System.Windows.Forms.Label();
            this.numPiecesTxt = new System.Windows.Forms.TextBox();
            this.firstHalfLbl = new System.Windows.Forms.Label();
            this.firstHalfTxt = new System.Windows.Forms.TextBox();
            this.orderingBtn = new System.Windows.Forms.Button();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape2 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.maxSolutionsLbl = new System.Windows.Forms.Label();
            this.thresholdTxt = new System.Windows.Forms.TextBox();
            this.maxTypeLbl = new System.Windows.Forms.Label();
            this.limitTxt = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSession = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSession = new System.Windows.Forms.ToolStripMenuItem();
            this.danceTypeAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.loadCastFromFile = new System.Windows.Forms.ToolStripMenuItem();
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.about = new System.Windows.Forms.ToolStripMenuItem();
            this.howToUse = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.loadingBox = new System.Windows.Forms.PictureBox();
            this.panelContainer.SuspendLayout();
            this.solutionsContainer.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadingBox)).BeginInit();
            this.SuspendLayout();
            // 
            // castListTitlelbl
            // 
            this.castListTitlelbl.Location = new System.Drawing.Point(12, 35);
            this.castListTitlelbl.Name = "castListTitlelbl";
            this.castListTitlelbl.Size = new System.Drawing.Size(128, 13);
            this.castListTitlelbl.TabIndex = 7;
            this.castListTitlelbl.Text = "CAST LIST";
            this.castListTitlelbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // castListTxt
            // 
            this.castListTxt.Location = new System.Drawing.Point(11, 81);
            this.castListTxt.Multiline = true;
            this.castListTxt.Name = "castListTxt";
            this.castListTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.castListTxt.Size = new System.Drawing.Size(139, 280);
            this.castListTxt.TabIndex = 8;
            this.castListTxt.TextChanged += new System.EventHandler(this.castListTxt_TextChanged);
            // 
            // castListDirLbl
            // 
            this.castListDirLbl.Location = new System.Drawing.Point(12, 49);
            this.castListDirLbl.Name = "castListDirLbl";
            this.castListDirLbl.Size = new System.Drawing.Size(128, 29);
            this.castListDirLbl.TabIndex = 9;
            this.castListDirLbl.Text = "Separate each cast member by a new line.";
            this.castListDirLbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panelContainer
            // 
            this.panelContainer.AutoScroll = true;
            this.panelContainer.Controls.Add(this.solutionsContainer);
            this.panelContainer.Location = new System.Drawing.Point(156, 81);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(855, 288);
            this.panelContainer.TabIndex = 25;
            // 
            // solutionsContainer
            // 
            this.solutionsContainer.AutoScroll = true;
            this.solutionsContainer.Controls.Add(this.loadingBox);
            this.solutionsContainer.Controls.Add(this.pieceNameLbl);
            this.solutionsContainer.Controls.Add(this.castMembersLbl);
            this.solutionsContainer.Controls.Add(this.castListLbl);
            this.solutionsContainer.Controls.Add(this.danceTypeLbl);
            this.solutionsContainer.Location = new System.Drawing.Point(0, 0);
            this.solutionsContainer.Name = "solutionsContainer";
            this.solutionsContainer.Size = new System.Drawing.Size(852, 288);
            this.solutionsContainer.TabIndex = 26;
            this.solutionsContainer.Visible = false;
            // 
            // pieceNameLbl
            // 
            this.pieceNameLbl.AutoSize = true;
            this.pieceNameLbl.Location = new System.Drawing.Point(0, 3);
            this.pieceNameLbl.Name = "pieceNameLbl";
            this.pieceNameLbl.Size = new System.Drawing.Size(65, 13);
            this.pieceNameLbl.TabIndex = 38;
            this.pieceNameLbl.Text = "Piece Name";
            // 
            // castMembersLbl
            // 
            this.castMembersLbl.AutoSize = true;
            this.castMembersLbl.Location = new System.Drawing.Point(0, 48);
            this.castMembersLbl.Name = "castMembersLbl";
            this.castMembersLbl.Size = new System.Drawing.Size(74, 13);
            this.castMembersLbl.TabIndex = 41;
            this.castMembersLbl.Text = "Cast Members";
            // 
            // castListLbl
            // 
            this.castListLbl.AutoSize = true;
            this.castListLbl.Location = new System.Drawing.Point(0, 74);
            this.castListLbl.Name = "castListLbl";
            this.castListLbl.Size = new System.Drawing.Size(47, 13);
            this.castListLbl.TabIndex = 40;
            this.castListLbl.Text = "Cast List";
            // 
            // danceTypeLbl
            // 
            this.danceTypeLbl.AutoSize = true;
            this.danceTypeLbl.Location = new System.Drawing.Point(0, 25);
            this.danceTypeLbl.Name = "danceTypeLbl";
            this.danceTypeLbl.Size = new System.Drawing.Size(66, 13);
            this.danceTypeLbl.TabIndex = 39;
            this.danceTypeLbl.Text = "Dance Type";
            // 
            // restartBtn
            // 
            this.restartBtn.Location = new System.Drawing.Point(846, 45);
            this.restartBtn.Name = "restartBtn";
            this.restartBtn.Size = new System.Drawing.Size(106, 31);
            this.restartBtn.TabIndex = 0;
            this.restartBtn.Text = "Restart";
            this.restartBtn.UseVisualStyleBackColor = true;
            this.restartBtn.Click += new System.EventHandler(this.restartBtn_Click);
            // 
            // createBtn
            // 
            this.createBtn.Location = new System.Drawing.Point(215, 52);
            this.createBtn.Name = "createBtn";
            this.createBtn.Size = new System.Drawing.Size(75, 23);
            this.createBtn.TabIndex = 0;
            this.createBtn.Text = "Create";
            this.createBtn.UseVisualStyleBackColor = true;
            this.createBtn.Click += new System.EventHandler(this.createBtn_Click);
            // 
            // numPiecesLbl
            // 
            this.numPiecesLbl.AutoSize = true;
            this.numPiecesLbl.Location = new System.Drawing.Point(156, 32);
            this.numPiecesLbl.Name = "numPiecesLbl";
            this.numPiecesLbl.Size = new System.Drawing.Size(154, 13);
            this.numPiecesLbl.TabIndex = 26;
            this.numPiecesLbl.Text = "How many pieces in the show?";
            // 
            // numPiecesTxt
            // 
            this.numPiecesTxt.Location = new System.Drawing.Point(316, 28);
            this.numPiecesTxt.MaxLength = 3;
            this.numPiecesTxt.Name = "numPiecesTxt";
            this.numPiecesTxt.Size = new System.Drawing.Size(38, 20);
            this.numPiecesTxt.TabIndex = 27;
            this.numPiecesTxt.Text = "16";
            this.numPiecesTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // firstHalfLbl
            // 
            this.firstHalfLbl.AutoSize = true;
            this.firstHalfLbl.Enabled = false;
            this.firstHalfLbl.Location = new System.Drawing.Point(364, 28);
            this.firstHalfLbl.Name = "firstHalfLbl";
            this.firstHalfLbl.Size = new System.Drawing.Size(113, 13);
            this.firstHalfLbl.TabIndex = 28;
            this.firstHalfLbl.Text = "How many in first half?";
            // 
            // firstHalfTxt
            // 
            this.firstHalfTxt.Enabled = false;
            this.firstHalfTxt.Location = new System.Drawing.Point(483, 28);
            this.firstHalfTxt.MaxLength = 3;
            this.firstHalfTxt.Name = "firstHalfTxt";
            this.firstHalfTxt.Size = new System.Drawing.Size(56, 20);
            this.firstHalfTxt.TabIndex = 30;
            this.firstHalfTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // orderingBtn
            // 
            this.orderingBtn.Enabled = false;
            this.orderingBtn.Location = new System.Drawing.Point(734, 45);
            this.orderingBtn.Name = "orderingBtn";
            this.orderingBtn.Size = new System.Drawing.Size(106, 31);
            this.orderingBtn.TabIndex = 32;
            this.orderingBtn.Text = "Find Ordering";
            this.orderingBtn.UseVisualStyleBackColor = true;
            this.orderingBtn.Click += new System.EventHandler(this.orderingBtn_Click);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape2,
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(1023, 381);
            this.shapeContainer1.TabIndex = 33;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape2
            // 
            this.lineShape2.Name = "lineShape2";
            this.lineShape2.X1 = 687;
            this.lineShape2.X2 = 687;
            this.lineShape2.Y1 = 24;
            this.lineShape2.Y2 = 78;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 360;
            this.lineShape1.X2 = 360;
            this.lineShape1.Y1 = 21;
            this.lineShape1.Y2 = 75;
            // 
            // maxSolutionsLbl
            // 
            this.maxSolutionsLbl.AutoSize = true;
            this.maxSolutionsLbl.Location = new System.Drawing.Point(747, 28);
            this.maxSolutionsLbl.Name = "maxSolutionsLbl";
            this.maxSolutionsLbl.Size = new System.Drawing.Size(124, 13);
            this.maxSolutionsLbl.TabIndex = 36;
            this.maxSolutionsLbl.Text = "Max solutions to display?";
            // 
            // thresholdTxt
            // 
            this.thresholdTxt.Location = new System.Drawing.Point(877, 25);
            this.thresholdTxt.MaxLength = 3;
            this.thresholdTxt.Name = "thresholdTxt";
            this.thresholdTxt.Size = new System.Drawing.Size(56, 20);
            this.thresholdTxt.TabIndex = 37;
            this.thresholdTxt.Text = "10";
            this.thresholdTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // maxTypeLbl
            // 
            this.maxTypeLbl.Enabled = false;
            this.maxTypeLbl.Location = new System.Drawing.Point(364, 54);
            this.maxTypeLbl.Name = "maxTypeLbl";
            this.maxTypeLbl.Size = new System.Drawing.Size(198, 18);
            this.maxTypeLbl.TabIndex = 38;
            this.maxTypeLbl.Text = "Max amount of any dance type per half?";
            // 
            // limitTxt
            // 
            this.limitTxt.Enabled = false;
            this.limitTxt.Location = new System.Drawing.Point(568, 52);
            this.limitTxt.MaxLength = 3;
            this.limitTxt.Name = "limitTxt";
            this.limitTxt.Size = new System.Drawing.Size(100, 20);
            this.limitTxt.TabIndex = 39;
            this.limitTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.MenuBar;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1023, 24);
            this.menuStrip1.TabIndex = 40;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadSession,
            this.saveSession,
            this.danceTypeAdd,
            this.loadCastFromFile,
            this.restartToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // loadSession
            // 
            this.loadSession.Name = "loadSession";
            this.loadSession.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.loadSession.Size = new System.Drawing.Size(182, 22);
            this.loadSession.Text = "&Load Session";
            this.loadSession.Click += new System.EventHandler(this.loadSession_Click);
            // 
            // saveSession
            // 
            this.saveSession.Name = "saveSession";
            this.saveSession.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveSession.Size = new System.Drawing.Size(182, 22);
            this.saveSession.Text = "&Save Session";
            this.saveSession.Click += new System.EventHandler(this.saveSession_Click);
            // 
            // danceTypeAdd
            // 
            this.danceTypeAdd.Name = "danceTypeAdd";
            this.danceTypeAdd.Size = new System.Drawing.Size(182, 22);
            this.danceTypeAdd.Text = "Add Dance Type";
            this.danceTypeAdd.Click += new System.EventHandler(this.danceTypeAdd_Click);
            // 
            // loadCastFromFile
            // 
            this.loadCastFromFile.Name = "loadCastFromFile";
            this.loadCastFromFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.C)));
            this.loadCastFromFile.Size = new System.Drawing.Size(182, 22);
            this.loadCastFromFile.Text = "Load &Cast";
            this.loadCastFromFile.Click += new System.EventHandler(this.loadCastFromFile_Click);
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.restartToolStripMenuItem.Text = "&Restart";
            this.restartToolStripMenuItem.Click += new System.EventHandler(this.restartToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.about,
            this.howToUse});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // about
            // 
            this.about.Name = "about";
            this.about.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.about.Size = new System.Drawing.Size(181, 22);
            this.about.Text = "&About...";
            this.about.Click += new System.EventHandler(this.about_Click);
            // 
            // howToUse
            // 
            this.howToUse.Name = "howToUse";
            this.howToUse.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.howToUse.Size = new System.Drawing.Size(181, 22);
            this.howToUse.Text = "&How To Use";
            this.howToUse.Click += new System.EventHandler(this.howToUse_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            this.newToolStripMenuItem.Text = "&New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            this.openToolStripMenuItem.Text = "&Open";
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            this.saveToolStripMenuItem.Text = "&Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 6);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // printPreviewToolStripMenuItem
            // 
            this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            this.printPreviewToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(150, 150);
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(150, 175);
            this.toolStripContainer1.TabIndex = 0;
            // 
            // loadingBox
            // 
            this.loadingBox.Image = ((System.Drawing.Image)(resources.GetObject("loadingBox.Image")));
            this.loadingBox.InitialImage = ((System.Drawing.Image)(resources.GetObject("loadingBox.InitialImage")));
            this.loadingBox.Location = new System.Drawing.Point(415, 123);
            this.loadingBox.Name = "loadingBox";
            this.loadingBox.Size = new System.Drawing.Size(16, 16);
            this.loadingBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.loadingBox.TabIndex = 42;
            this.loadingBox.TabStop = false;
            this.loadingBox.Visible = false;
            // 
            // Hatchu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1023, 381);
            this.Controls.Add(this.restartBtn);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.limitTxt);
            this.Controls.Add(this.maxTypeLbl);
            this.Controls.Add(this.thresholdTxt);
            this.Controls.Add(this.maxSolutionsLbl);
            this.Controls.Add(this.orderingBtn);
            this.Controls.Add(this.firstHalfTxt);
            this.Controls.Add(this.firstHalfLbl);
            this.Controls.Add(this.numPiecesTxt);
            this.Controls.Add(this.numPiecesLbl);
            this.Controls.Add(this.createBtn);
            this.Controls.Add(this.panelContainer);
            this.Controls.Add(this.castListDirLbl);
            this.Controls.Add(this.castListTxt);
            this.Controls.Add(this.castListTitlelbl);
            this.Controls.Add(this.shapeContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Hatchu";
            this.Text = "Hatchū  (Show Ordering Solution)";
            this.panelContainer.ResumeLayout(false);
            this.solutionsContainer.ResumeLayout(false);
            this.solutionsContainer.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadingBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label castListTitlelbl;
        private System.Windows.Forms.TextBox castListTxt;
        private System.Windows.Forms.Label castListDirLbl;
        private System.Windows.Forms.Panel panelContainer;
        private System.Windows.Forms.Button createBtn;
        private System.Windows.Forms.Label numPiecesLbl;
        private System.Windows.Forms.TextBox numPiecesTxt;
        private System.Windows.Forms.Label firstHalfLbl;
        private System.Windows.Forms.TextBox firstHalfTxt;
        private System.Windows.Forms.Button orderingBtn;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape2;
        private System.Windows.Forms.Panel solutionsContainer;
        private System.Windows.Forms.Button restartBtn;
        private System.Windows.Forms.Label maxSolutionsLbl;
        private System.Windows.Forms.TextBox thresholdTxt;
        private System.Windows.Forms.Label castMembersLbl;
        private System.Windows.Forms.Label castListLbl;
        private System.Windows.Forms.Label danceTypeLbl;
        private System.Windows.Forms.Label pieceNameLbl;
        private System.Windows.Forms.Label maxTypeLbl;
        private System.Windows.Forms.TextBox limitTxt;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSession;
        private System.Windows.Forms.ToolStripMenuItem saveSession;
        private System.Windows.Forms.ToolStripMenuItem loadCastFromFile;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem about;
        private System.Windows.Forms.ToolStripMenuItem howToUse;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem danceTypeAdd;
        private System.Windows.Forms.PictureBox loadingBox;
    }
}

