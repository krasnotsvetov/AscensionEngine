namespace AscensionEditor
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
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addAssemblyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sceneObjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addEmptyEntityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.materialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addMaterialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ComponentBox = new System.Windows.Forms.ListBox();
            this.AvailableComponents = new System.Windows.Forms.ComboBox();
            this.OutputBox = new System.Windows.Forms.RichTextBox();
            this.ComponentPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.EntityPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.TabsMain = new System.Windows.Forms.TabControl();
            this.ScenesPage = new System.Windows.Forms.TabPage();
            this.addComponent = new System.Windows.Forms.Button();
            this.MaterialsPage = new System.Windows.Forms.TabPage();
            this.TexturesPanel = new System.Windows.Forms.Panel();
            this.MaterialParams = new System.Windows.Forms.PropertyGrid();
            this.labelShader = new System.Windows.Forms.Label();
            this.MaterialShaderBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.MaterialBox = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.drawingSurface)).BeginInit();
            this.MainMenu.SuspendLayout();
            this.TabsMain.SuspendLayout();
            this.ScenesPage.SuspendLayout();
            this.MaterialsPage.SuspendLayout();
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
            this.EntityView.HideSelection = false;
            this.EntityView.Location = new System.Drawing.Point(6, 36);
            this.EntityView.Name = "EntityView";
            this.EntityView.Size = new System.Drawing.Size(197, 450);
            this.EntityView.TabIndex = 1;
            this.EntityView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.EntityView_ItemDrag);
            this.EntityView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.EntityView_AfterSelect);
            this.EntityView.DragDrop += new System.Windows.Forms.DragEventHandler(this.EntityView_DragDrop);
            this.EntityView.DragEnter += new System.Windows.Forms.DragEventHandler(this.EntityView_DragEnter);
            this.EntityView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EntityView_MouseDown);
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
            // MainMenu
            // 
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.sceneObjectsToolStripMenuItem,
            this.materialToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(1902, 28);
            this.MainMenu.TabIndex = 3;
            this.MainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveSceneToolStripMenuItem,
            this.openSceneToolStripMenuItem,
            this.addAssemblyToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveSceneToolStripMenuItem
            // 
            this.saveSceneToolStripMenuItem.Name = "saveSceneToolStripMenuItem";
            this.saveSceneToolStripMenuItem.Size = new System.Drawing.Size(177, 26);
            this.saveSceneToolStripMenuItem.Text = "Save Scene";
            this.saveSceneToolStripMenuItem.Click += new System.EventHandler(this.saveSceneToolStripMenuItem_Click);
            // 
            // openSceneToolStripMenuItem
            // 
            this.openSceneToolStripMenuItem.Name = "openSceneToolStripMenuItem";
            this.openSceneToolStripMenuItem.Size = new System.Drawing.Size(177, 26);
            this.openSceneToolStripMenuItem.Text = "Open Scene";
            this.openSceneToolStripMenuItem.Click += new System.EventHandler(this.openSceneToolStripMenuItem_Click);
            // 
            // addAssemblyToolStripMenuItem
            // 
            this.addAssemblyToolStripMenuItem.Name = "addAssemblyToolStripMenuItem";
            this.addAssemblyToolStripMenuItem.Size = new System.Drawing.Size(177, 26);
            this.addAssemblyToolStripMenuItem.Text = "Add assembly";
            this.addAssemblyToolStripMenuItem.Click += new System.EventHandler(this.addAssemblyToolStripMenuItem_Click);
            // 
            // sceneObjectsToolStripMenuItem
            // 
            this.sceneObjectsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addEmptyEntityToolStripMenuItem});
            this.sceneObjectsToolStripMenuItem.Name = "sceneObjectsToolStripMenuItem";
            this.sceneObjectsToolStripMenuItem.Size = new System.Drawing.Size(110, 24);
            this.sceneObjectsToolStripMenuItem.Text = "SceneObjects";
            // 
            // addEmptyEntityToolStripMenuItem
            // 
            this.addEmptyEntityToolStripMenuItem.Name = "addEmptyEntityToolStripMenuItem";
            this.addEmptyEntityToolStripMenuItem.Size = new System.Drawing.Size(199, 26);
            this.addEmptyEntityToolStripMenuItem.Text = "Add empty entity";
            this.addEmptyEntityToolStripMenuItem.Click += new System.EventHandler(this.addEmptyEntityToolStripMenuItem_Click);
            // 
            // materialToolStripMenuItem
            // 
            this.materialToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addMaterialToolStripMenuItem});
            this.materialToolStripMenuItem.Name = "materialToolStripMenuItem";
            this.materialToolStripMenuItem.Size = new System.Drawing.Size(76, 24);
            this.materialToolStripMenuItem.Text = "Material";
            // 
            // addMaterialToolStripMenuItem
            // 
            this.addMaterialToolStripMenuItem.Name = "addMaterialToolStripMenuItem";
            this.addMaterialToolStripMenuItem.Size = new System.Drawing.Size(112, 26);
            this.addMaterialToolStripMenuItem.Text = "Add";
            this.addMaterialToolStripMenuItem.Click += new System.EventHandler(this.addMaterialToolStripMenuItem_Click);
            // 
            // ComponentBox
            // 
            this.ComponentBox.FormattingEnabled = true;
            this.ComponentBox.ItemHeight = 16;
            this.ComponentBox.Location = new System.Drawing.Point(1614, 57);
            this.ComponentBox.Name = "ComponentBox";
            this.ComponentBox.Size = new System.Drawing.Size(254, 196);
            this.ComponentBox.TabIndex = 4;
            this.ComponentBox.SelectedIndexChanged += new System.EventHandler(this.ComponentBox_SelectedIndexChanged);
            this.ComponentBox.SelectedValueChanged += new System.EventHandler(this.ComponentBox_SelectedValueChanged);
            this.ComponentBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ComponentBox_MouseUp);
            // 
            // AvailableComponents
            // 
            this.AvailableComponents.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.AvailableComponents.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.AvailableComponents.FormattingEnabled = true;
            this.AvailableComponents.Location = new System.Drawing.Point(1614, 27);
            this.AvailableComponents.Name = "AvailableComponents";
            this.AvailableComponents.Size = new System.Drawing.Size(197, 24);
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
            // TabsMain
            // 
            this.TabsMain.Controls.Add(this.ScenesPage);
            this.TabsMain.Controls.Add(this.MaterialsPage);
            this.TabsMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabsMain.Location = new System.Drawing.Point(0, 28);
            this.TabsMain.Name = "TabsMain";
            this.TabsMain.SelectedIndex = 0;
            this.TabsMain.Size = new System.Drawing.Size(1902, 1005);
            this.TabsMain.TabIndex = 10;
            // 
            // ScenesPage
            // 
            this.ScenesPage.Controls.Add(this.addComponent);
            this.ScenesPage.Controls.Add(this.SceneComboBox);
            this.ScenesPage.Controls.Add(this.EntityView);
            this.ScenesPage.Controls.Add(this.ComponentPropertyGrid);
            this.ScenesPage.Controls.Add(this.EntityPropertyGrid);
            this.ScenesPage.Controls.Add(this.drawingSurface);
            this.ScenesPage.Controls.Add(this.AvailableComponents);
            this.ScenesPage.Controls.Add(this.ComponentBox);
            this.ScenesPage.Location = new System.Drawing.Point(4, 25);
            this.ScenesPage.Name = "ScenesPage";
            this.ScenesPage.Padding = new System.Windows.Forms.Padding(3);
            this.ScenesPage.Size = new System.Drawing.Size(1894, 976);
            this.ScenesPage.TabIndex = 0;
            this.ScenesPage.Text = "Scenes";
            this.ScenesPage.UseVisualStyleBackColor = true;
            // 
            // addComponent
            // 
            this.addComponent.Location = new System.Drawing.Point(1817, 27);
            this.addComponent.Name = "addComponent";
            this.addComponent.Size = new System.Drawing.Size(51, 23);
            this.addComponent.TabIndex = 10;
            this.addComponent.Text = "+";
            this.addComponent.UseVisualStyleBackColor = true;
            this.addComponent.Click += new System.EventHandler(this.addComponent_Click);
            // 
            // MaterialsPage
            // 
            this.MaterialsPage.Controls.Add(this.TexturesPanel);
            this.MaterialsPage.Controls.Add(this.MaterialParams);
            this.MaterialsPage.Controls.Add(this.labelShader);
            this.MaterialsPage.Controls.Add(this.MaterialShaderBox);
            this.MaterialsPage.Controls.Add(this.label1);
            this.MaterialsPage.Controls.Add(this.MaterialBox);
            this.MaterialsPage.Location = new System.Drawing.Point(4, 25);
            this.MaterialsPage.Name = "MaterialsPage";
            this.MaterialsPage.Padding = new System.Windows.Forms.Padding(3);
            this.MaterialsPage.Size = new System.Drawing.Size(1894, 976);
            this.MaterialsPage.TabIndex = 1;
            this.MaterialsPage.Text = "Materials";
            this.MaterialsPage.UseVisualStyleBackColor = true;
            // 
            // TexturesPanel
            // 
            this.TexturesPanel.Location = new System.Drawing.Point(227, 84);
            this.TexturesPanel.Name = "TexturesPanel";
            this.TexturesPanel.Size = new System.Drawing.Size(1659, 232);
            this.TexturesPanel.TabIndex = 8;
            // 
            // MaterialParams
            // 
            this.MaterialParams.Location = new System.Drawing.Point(227, 322);
            this.MaterialParams.Name = "MaterialParams";
            this.MaterialParams.Size = new System.Drawing.Size(260, 262);
            this.MaterialParams.TabIndex = 7;
            // 
            // labelShader
            // 
            this.labelShader.AutoSize = true;
            this.labelShader.Location = new System.Drawing.Point(224, 36);
            this.labelShader.Name = "labelShader";
            this.labelShader.Size = new System.Drawing.Size(58, 17);
            this.labelShader.TabIndex = 4;
            this.labelShader.Text = "Shader:";
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
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1902, 1033);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.TabsMain);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.Name = "EditorForm";
            this.Text = "1";
            this.Load += new System.EventHandler(this.EditorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.drawingSurface)).EndInit();
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.TabsMain.ResumeLayout(false);
            this.ScenesPage.ResumeLayout(false);
            this.MaterialsPage.ResumeLayout(false);
            this.MaterialsPage.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.PictureBox drawingSurface;
        public System.Windows.Forms.ComboBox SceneComboBox;
        public System.Windows.Forms.TreeView EntityView;
        private System.Windows.Forms.MenuStrip MainMenu;
        public System.Windows.Forms.ListBox ComponentBox;
        private System.Windows.Forms.ComboBox AvailableComponents;
        private System.Windows.Forms.RichTextBox OutputBox;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSceneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSceneToolStripMenuItem;
        public System.Windows.Forms.PropertyGrid ComponentPropertyGrid;
        public System.Windows.Forms.PropertyGrid EntityPropertyGrid;
        private System.Windows.Forms.ToolStripMenuItem sceneObjectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addEmptyEntityToolStripMenuItem;
        private System.Windows.Forms.TabControl TabsMain;
        private System.Windows.Forms.TabPage ScenesPage;
        private System.Windows.Forms.TabPage MaterialsPage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox MaterialBox;
        private System.Windows.Forms.Label labelShader;
        private System.Windows.Forms.ComboBox MaterialShaderBox;
        private System.Windows.Forms.PropertyGrid MaterialParams;
        private System.Windows.Forms.Panel TexturesPanel;
        private System.Windows.Forms.ToolStripMenuItem materialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addMaterialToolStripMenuItem;
        private System.Windows.Forms.Button addComponent;
        private System.Windows.Forms.ToolStripMenuItem addAssemblyToolStripMenuItem;
    }
}