namespace PwsgForms
{
    partial class RoomPlaner
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoomPlaner));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.addFurniture = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.createdFurniture = new System.Windows.Forms.GroupBox();
            this.listBox = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.newBlueprintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openBlueprintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveBlueprintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.polishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.addFurniture.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.createdFurniture.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            resources.ApplyResources(this.splitContainer, "splitContainer");
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            resources.ApplyResources(this.splitContainer.Panel1, "splitContainer.Panel1");
            this.splitContainer.Panel1.Controls.Add(this.flowLayoutPanel2);
            // 
            // splitContainer.Panel2
            // 
            resources.ApplyResources(this.splitContainer.Panel2, "splitContainer.Panel2");
            this.splitContainer.Panel2.Controls.Add(this.tableLayoutPanel1);
            // 
            // flowLayoutPanel2
            // 
            resources.ApplyResources(this.flowLayoutPanel2, "flowLayoutPanel2");
            this.flowLayoutPanel2.Controls.Add(this.pictureBox);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.addFurniture, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.createdFurniture, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // addFurniture
            // 
            resources.ApplyResources(this.addFurniture, "addFurniture");
            this.addFurniture.Controls.Add(this.flowLayoutPanel1);
            this.addFurniture.Name = "addFurniture";
            this.addFurniture.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Controls.Add(this.button2);
            this.flowLayoutPanel1.Controls.Add(this.button3);
            this.flowLayoutPanel1.Controls.Add(this.button4);
            this.flowLayoutPanel1.Controls.Add(this.button5);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // createdFurniture
            // 
            resources.ApplyResources(this.createdFurniture, "createdFurniture");
            this.createdFurniture.Controls.Add(this.listBox);
            this.createdFurniture.Name = "createdFurniture";
            this.createdFurniture.TabStop = false;
            // 
            // listBox
            // 
            resources.ApplyResources(this.listBox, "listBox");
            this.listBox.CausesValidation = false;
            this.listBox.FormattingEnabled = true;
            this.listBox.Name = "listBox";
            // 
            // menuStrip1
            // 
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Name = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newBlueprintToolStripMenuItem,
            this.openBlueprintToolStripMenuItem,
            this.saveBlueprintToolStripMenuItem,
            this.languageToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            // 
            // pictureBox
            // 
            resources.ApplyResources(this.pictureBox, "pictureBox");
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.onPictureBoxPaint);
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.onMouseDownPicture);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.onMouseMove);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.BackgroundImage = global::PwsgForms.Properties.Resources.coffee_table;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Name = "button1";
            this.button1.Tag = "Kitchen Table";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.onButtonClick);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.BackColor = System.Drawing.Color.White;
            this.button2.BackgroundImage = global::PwsgForms.Properties.Resources.table;
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Name = "button2";
            this.button2.Tag = "Table";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.onButtonClick);
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.BackColor = System.Drawing.Color.White;
            this.button3.BackgroundImage = global::PwsgForms.Properties.Resources.sofa;
            this.button3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button3.Name = "button3";
            this.button3.Tag = "Sofa";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.onButtonClick);
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.BackColor = System.Drawing.Color.White;
            this.button4.BackgroundImage = global::PwsgForms.Properties.Resources.double_bed;
            this.button4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button4.Name = "button4";
            this.button4.Tag = "Bed";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.onButtonClick);
            // 
            // button5
            // 
            resources.ApplyResources(this.button5, "button5");
            this.button5.BackColor = System.Drawing.Color.White;
            this.button5.BackgroundImage = global::PwsgForms.Properties.Resources.wall;
            this.button5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button5.Name = "button5";
            this.button5.Tag = "Wall";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.onButtonClick);
            // 
            // newBlueprintToolStripMenuItem
            // 
            resources.ApplyResources(this.newBlueprintToolStripMenuItem, "newBlueprintToolStripMenuItem");
            this.newBlueprintToolStripMenuItem.Image = global::PwsgForms.Properties.Resources.NewFile_16x;
            this.newBlueprintToolStripMenuItem.Name = "newBlueprintToolStripMenuItem";
            this.newBlueprintToolStripMenuItem.Click += new System.EventHandler(this.onNewBlueprint);
            // 
            // openBlueprintToolStripMenuItem
            // 
            resources.ApplyResources(this.openBlueprintToolStripMenuItem, "openBlueprintToolStripMenuItem");
            this.openBlueprintToolStripMenuItem.Image = global::PwsgForms.Properties.Resources.OpenFolder_16x;
            this.openBlueprintToolStripMenuItem.Name = "openBlueprintToolStripMenuItem";
            this.openBlueprintToolStripMenuItem.Click += new System.EventHandler(this.onOpenBlueprint);
            // 
            // saveBlueprintToolStripMenuItem
            // 
            resources.ApplyResources(this.saveBlueprintToolStripMenuItem, "saveBlueprintToolStripMenuItem");
            this.saveBlueprintToolStripMenuItem.Image = global::PwsgForms.Properties.Resources.Save_16x;
            this.saveBlueprintToolStripMenuItem.Name = "saveBlueprintToolStripMenuItem";
            this.saveBlueprintToolStripMenuItem.Click += new System.EventHandler(this.onSaveBlueprint);
            // 
            // languageToolStripMenuItem
            // 
            resources.ApplyResources(this.languageToolStripMenuItem, "languageToolStripMenuItem");
            this.languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.englishToolStripMenuItem,
            this.polishToolStripMenuItem});
            this.languageToolStripMenuItem.Image = global::PwsgForms.Properties.Resources.FlagDarkBlue_16x;
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            // 
            // englishToolStripMenuItem
            // 
            resources.ApplyResources(this.englishToolStripMenuItem, "englishToolStripMenuItem");
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            this.englishToolStripMenuItem.Tag = "en";
            this.englishToolStripMenuItem.Click += new System.EventHandler(this.onChangeLanguage);
            // 
            // polishToolStripMenuItem
            // 
            resources.ApplyResources(this.polishToolStripMenuItem, "polishToolStripMenuItem");
            this.polishToolStripMenuItem.Name = "polishToolStripMenuItem";
            this.polishToolStripMenuItem.Tag = "pl";
            this.polishToolStripMenuItem.Click += new System.EventHandler(this.onChangeLanguage);
            // 
            // RoomPlaner
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "RoomPlaner";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.onKeyDown);
            this.Resize += new System.EventHandler(this.onResize);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.addFurniture.ResumeLayout(false);
            this.addFurniture.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.createdFurniture.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newBlueprintToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox addFurniture;
        private System.Windows.Forms.GroupBox createdFurniture;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.ToolStripMenuItem openBlueprintToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveBlueprintToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem englishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem polishToolStripMenuItem;
    }
}

