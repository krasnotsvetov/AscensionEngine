namespace MonogameSamples.Engine.Editor
{
    partial class EditorForm
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
            this.drawingSurface = new System.Windows.Forms.PictureBox();
            this.EntityView = new System.Windows.Forms.TreeView();
            this.SceneComboBox = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ComponentBox = new System.Windows.Forms.ListBox();
            this.AvailableComponents = new System.Windows.Forms.ComboBox();
            this.OutputBox = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.propertyGrid2 = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.drawingSurface)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // drawingSurface
            // 
            this.drawingSurface.Location = new System.Drawing.Point(215, 44);
            this.drawingSurface.Name = "drawingSurface";
            this.drawingSurface.Size = new System.Drawing.Size(1366, 768);
            this.drawingSurface.TabIndex = 0;
            this.drawingSurface.TabStop = false;
            // 
            // EntityView
            // 
            this.EntityView.Location = new System.Drawing.Point(12, 74);
            this.EntityView.Name = "EntityView";
            this.EntityView.Size = new System.Drawing.Size(197, 470);
            this.EntityView.TabIndex = 1;
            this.EntityView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.EntityView_AfterSelect);
            // 
            // SceneComboBox
            // 
            this.SceneComboBox.FormattingEnabled = true;
            this.SceneComboBox.Location = new System.Drawing.Point(12, 44);
            this.SceneComboBox.Name = "SceneComboBox";
            this.SceneComboBox.Size = new System.Drawing.Size(197, 24);
            this.SceneComboBox.TabIndex = 2;
            this.SceneComboBox.SelectedIndexChanged += new System.EventHandler(this.SceneComboBox_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1902, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveSceneToolStripMenuItem,
            this.openSceneToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveSceneToolStripMenuItem
            // 
            this.saveSceneToolStripMenuItem.Name = "saveSceneToolStripMenuItem";
            this.saveSceneToolStripMenuItem.Size = new System.Drawing.Size(163, 26);
            this.saveSceneToolStripMenuItem.Text = "Save Scene";
            this.saveSceneToolStripMenuItem.Click += new System.EventHandler(this.saveSceneToolStripMenuItem_Click);
            // 
            // openSceneToolStripMenuItem
            // 
            this.openSceneToolStripMenuItem.Name = "openSceneToolStripMenuItem";
            this.openSceneToolStripMenuItem.Size = new System.Drawing.Size(163, 26);
            this.openSceneToolStripMenuItem.Text = "Open Scene";
            this.openSceneToolStripMenuItem.Click += new System.EventHandler(this.openSceneToolStripMenuItem_Click);
            // 
            // ComponentBox
            // 
            this.ComponentBox.FormattingEnabled = true;
            this.ComponentBox.ItemHeight = 16;
            this.ComponentBox.Location = new System.Drawing.Point(1587, 74);
            this.ComponentBox.Name = "ComponentBox";
            this.ComponentBox.Size = new System.Drawing.Size(204, 196);
            this.ComponentBox.TabIndex = 4;
            this.ComponentBox.SelectedIndexChanged += new System.EventHandler(this.ComponentBox_SelectedIndexChanged);
            // 
            // AvailableComponents
            // 
            this.AvailableComponents.FormattingEnabled = true;
            this.AvailableComponents.Location = new System.Drawing.Point(1587, 44);
            this.AvailableComponents.Name = "AvailableComponents";
            this.AvailableComponents.Size = new System.Drawing.Size(204, 24);
            this.AvailableComponents.TabIndex = 5;
            // 
            // OutputBox
            // 
            this.OutputBox.Location = new System.Drawing.Point(12, 848);
            this.OutputBox.Name = "OutputBox";
            this.OutputBox.Size = new System.Drawing.Size(414, 178);
            this.OutputBox.TabIndex = 6;
            this.OutputBox.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1797, 74);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(1587, 291);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(293, 521);
            this.propertyGrid1.TabIndex = 8;
            // 
            // propertyGrid2
            // 
            this.propertyGrid2.Location = new System.Drawing.Point(12, 550);
            this.propertyGrid2.Name = "propertyGrid2";
            this.propertyGrid2.Size = new System.Drawing.Size(197, 262);
            this.propertyGrid2.TabIndex = 9;
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1902, 1033);
            this.Controls.Add(this.propertyGrid2);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.OutputBox);
            this.Controls.Add(this.AvailableComponents);
            this.Controls.Add(this.ComponentBox);
            this.Controls.Add(this.SceneComboBox);
            this.Controls.Add(this.EntityView);
            this.Controls.Add(this.drawingSurface);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EditorForm";
            this.Text = "EditorForm";
            this.Load += new System.EventHandler(this.EditorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.drawingSurface)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.PictureBox drawingSurface;
        public System.Windows.Forms.ComboBox SceneComboBox;
        public System.Windows.Forms.TreeView EntityView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ListBox ComponentBox;
        private System.Windows.Forms.ComboBox AvailableComponents;
        private System.Windows.Forms.RichTextBox OutputBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSceneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSceneToolStripMenuItem;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.PropertyGrid propertyGrid2;
    }
}