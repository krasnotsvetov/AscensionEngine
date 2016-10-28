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
            this.enToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addEmptyEntityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ComponentBox = new System.Windows.Forms.ListBox();
            this.AvailableComponents = new System.Windows.Forms.ComboBox();
            this.OutputBox = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.ComponentPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.EntityPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.TexturesPanel = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.label2 = new System.Windows.Forms.Label();
            this.MaterialShaderBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.MaterialBox = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.asdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.drawingSurface)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // drawingSurface
            // 
            this.drawingSurface.Location = new System.Drawing.Point(229, 27);
            this.drawingSurface.Name = "drawingSurface";
            this.drawingSurface.Size = new System.Drawing.Size(1366, 768);
            this.drawingSurface.TabIndex = 0;
            this.drawingSurface.TabStop = false;
            // 
            // EntityView
            // 
            this.EntityView.AllowDrop = true;
            this.EntityView.Location = new System.Drawing.Point(6, 36);
            this.EntityView.Name = "EntityView";
            this.EntityView.Size = new System.Drawing.Size(197, 450);
            this.EntityView.TabIndex = 1;
            this.EntityView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.EntityView_ItemDrag);
            this.EntityView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.EntityView_AfterSelect);
            this.EntityView.DragDrop += new System.Windows.Forms.DragEventHandler(this.EntityView_DragDrop);
            this.EntityView.DragEnter += new System.Windows.Forms.DragEventHandler(this.EntityView_DragEnter);
            this.EntityView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.EntityView_MouseUp);
            // 
            // SceneComboBox
            // 
            this.SceneComboBox.FormattingEnabled = true;
            this.SceneComboBox.Location = new System.Drawing.Point(6, 6);
            this.SceneComboBox.Name = "SceneComboBox";
            this.SceneComboBox.Size = new System.Drawing.Size(197, 24);
            this.SceneComboBox.TabIndex = 2;
            this.SceneComboBox.SelectedIndexChanged += new System.EventHandler(this.SceneComboBox_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.enToolStripMenuItem,
            this.asdToolStripMenuItem});
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
            // enToolStripMenuItem
            // 
            this.enToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addEmptyEntityToolStripMenuItem});
            this.enToolStripMenuItem.Name = "enToolStripMenuItem";
            this.enToolStripMenuItem.Size = new System.Drawing.Size(110, 24);
            this.enToolStripMenuItem.Text = "SceneObjects";
            // 
            // addEmptyEntityToolStripMenuItem
            // 
            this.addEmptyEntityToolStripMenuItem.Name = "addEmptyEntityToolStripMenuItem";
            this.addEmptyEntityToolStripMenuItem.Size = new System.Drawing.Size(199, 26);
            this.addEmptyEntityToolStripMenuItem.Text = "Add empty entity";
            this.addEmptyEntityToolStripMenuItem.Click += new System.EventHandler(this.addEmptyEntityToolStripMenuItem_Click);
            // 
            // ComponentBox
            // 
            this.ComponentBox.FormattingEnabled = true;
            this.ComponentBox.ItemHeight = 16;
            this.ComponentBox.Location = new System.Drawing.Point(1614, 57);
            this.ComponentBox.Name = "ComponentBox";
            this.ComponentBox.Size = new System.Drawing.Size(254, 196);
            this.ComponentBox.TabIndex = 4;
            this.ComponentBox.SelectedValueChanged += new System.EventHandler(this.ComponentBox_SelectedValueChanged);
            // 
            // AvailableComponents
            // 
            this.AvailableComponents.FormattingEnabled = true;
            this.AvailableComponents.Location = new System.Drawing.Point(1614, 27);
            this.AvailableComponents.Name = "AvailableComponents";
            this.AvailableComponents.Size = new System.Drawing.Size(254, 24);
            this.AvailableComponents.TabIndex = 5;
            // 
            // OutputBox
            // 
            this.OutputBox.Location = new System.Drawing.Point(12, 8);
            this.OutputBox.Name = "OutputBox";
            this.OutputBox.Size = new System.Drawing.Size(414, 188);
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
            // ComponentPropertyGrid
            // 
            this.ComponentPropertyGrid.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ComponentPropertyGrid.Location = new System.Drawing.Point(1614, 274);
            this.ComponentPropertyGrid.Name = "ComponentPropertyGrid";
            this.ComponentPropertyGrid.Size = new System.Drawing.Size(254, 521);
            this.ComponentPropertyGrid.TabIndex = 8;
            this.ComponentPropertyGrid.ToolbarVisible = false;
            // 
            // EntityPropertyGrid
            // 
            this.EntityPropertyGrid.Location = new System.Drawing.Point(6, 493);
            this.EntityPropertyGrid.Name = "EntityPropertyGrid";
            this.EntityPropertyGrid.Size = new System.Drawing.Size(197, 302);
            this.EntityPropertyGrid.TabIndex = 9;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 28);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1902, 1005);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.SceneComboBox);
            this.tabPage1.Controls.Add(this.EntityView);
            this.tabPage1.Controls.Add(this.ComponentPropertyGrid);
            this.tabPage1.Controls.Add(this.EntityPropertyGrid);
            this.tabPage1.Controls.Add(this.drawingSurface);
            this.tabPage1.Controls.Add(this.AvailableComponents);
            this.tabPage1.Controls.Add(this.ComponentBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1894, 976);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.TexturesPanel);
            this.tabPage2.Controls.Add(this.propertyGrid1);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.MaterialShaderBox);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.MaterialBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1894, 976);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // TexturesPanel
            // 
            this.TexturesPanel.Location = new System.Drawing.Point(227, 84);
            this.TexturesPanel.Name = "TexturesPanel";
            this.TexturesPanel.Size = new System.Drawing.Size(1659, 232);
            this.TexturesPanel.TabIndex = 8;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(227, 322);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(260, 285);
            this.propertyGrid1.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(224, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Shader:";
            // 
            // MaterialShaderBox
            // 
            this.MaterialShaderBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.MaterialShaderBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.MaterialShaderBox.FormattingEnabled = true;
            this.MaterialShaderBox.Location = new System.Drawing.Point(288, 36);
            this.MaterialShaderBox.Name = "MaterialShaderBox";
            this.MaterialShaderBox.Size = new System.Drawing.Size(139, 24);
            this.MaterialShaderBox.TabIndex = 3;
            this.MaterialShaderBox.SelectedIndexChanged += new System.EventHandler(this.MaterialShaderBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Materials:";
            // 
            // MaterialBox
            // 
            this.MaterialBox.FormattingEnabled = true;
            this.MaterialBox.ItemHeight = 16;
            this.MaterialBox.Location = new System.Drawing.Point(6, 36);
            this.MaterialBox.Name = "MaterialBox";
            this.MaterialBox.Size = new System.Drawing.Size(212, 548);
            this.MaterialBox.TabIndex = 0;
            this.MaterialBox.SelectedIndexChanged += new System.EventHandler(this.MaterialBox_SelectedIndexChanged);
            this.MaterialBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MaterialBox_MouseUp);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.OutputBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 866);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1902, 167);
            this.panel1.TabIndex = 11;
            // 
            // asdToolStripMenuItem
            // 
            this.asdToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem});
            this.asdToolStripMenuItem.Name = "asdToolStripMenuItem";
            this.asdToolStripMenuItem.Size = new System.Drawing.Size(76, 24);
            this.asdToolStripMenuItem.Text = "Material";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1902, 1033);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EditorForm";
            this.Text = "EditorForm";
            this.Load += new System.EventHandler(this.EditorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.drawingSurface)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.PictureBox drawingSurface;
        public System.Windows.Forms.ComboBox SceneComboBox;
        public System.Windows.Forms.TreeView EntityView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        public System.Windows.Forms.ListBox ComponentBox;
        private System.Windows.Forms.ComboBox AvailableComponents;
        private System.Windows.Forms.RichTextBox OutputBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSceneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSceneToolStripMenuItem;
        public System.Windows.Forms.PropertyGrid ComponentPropertyGrid;
        public System.Windows.Forms.PropertyGrid EntityPropertyGrid;
        private System.Windows.Forms.ToolStripMenuItem enToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addEmptyEntityToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox MaterialBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox MaterialShaderBox;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Panel TexturesPanel;
        private System.Windows.Forms.ToolStripMenuItem asdToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
    }
}