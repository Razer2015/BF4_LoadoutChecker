namespace BF4_LoadoutChecker
{
    partial class Details
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
            this.fLP_base = new System.Windows.Forms.FlowLayoutPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dockingLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lEFTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rIGHTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tOPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bOTTOMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keepDockingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fLP_base
            // 
            this.fLP_base.AutoScroll = true;
            this.fLP_base.BackColor = System.Drawing.Color.Transparent;
            this.fLP_base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fLP_base.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.fLP_base.Location = new System.Drawing.Point(0, 24);
            this.fLP_base.Margin = new System.Windows.Forms.Padding(0);
            this.fLP_base.Name = "fLP_base";
            this.fLP_base.Size = new System.Drawing.Size(239, 444);
            this.fLP_base.TabIndex = 0;
            this.fLP_base.WrapContents = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(239, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dockingLocationToolStripMenuItem,
            this.keepDockingToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // dockingLocationToolStripMenuItem
            // 
            this.dockingLocationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lEFTToolStripMenuItem,
            this.rIGHTToolStripMenuItem,
            this.tOPToolStripMenuItem,
            this.bOTTOMToolStripMenuItem});
            this.dockingLocationToolStripMenuItem.Name = "dockingLocationToolStripMenuItem";
            this.dockingLocationToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.dockingLocationToolStripMenuItem.Text = "Docking Location";
            // 
            // lEFTToolStripMenuItem
            // 
            this.lEFTToolStripMenuItem.Name = "lEFTToolStripMenuItem";
            this.lEFTToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.lEFTToolStripMenuItem.Text = "LEFT";
            this.lEFTToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // rIGHTToolStripMenuItem
            // 
            this.rIGHTToolStripMenuItem.Checked = true;
            this.rIGHTToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rIGHTToolStripMenuItem.Name = "rIGHTToolStripMenuItem";
            this.rIGHTToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.rIGHTToolStripMenuItem.Text = "RIGHT";
            this.rIGHTToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // tOPToolStripMenuItem
            // 
            this.tOPToolStripMenuItem.Name = "tOPToolStripMenuItem";
            this.tOPToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.tOPToolStripMenuItem.Text = "TOP";
            this.tOPToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // bOTTOMToolStripMenuItem
            // 
            this.bOTTOMToolStripMenuItem.Name = "bOTTOMToolStripMenuItem";
            this.bOTTOMToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.bOTTOMToolStripMenuItem.Text = "BOTTOM";
            this.bOTTOMToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // keepDockingToolStripMenuItem
            // 
            this.keepDockingToolStripMenuItem.Checked = true;
            this.keepDockingToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.keepDockingToolStripMenuItem.Name = "keepDockingToolStripMenuItem";
            this.keepDockingToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.keepDockingToolStripMenuItem.Text = "Keep Docking";
            this.keepDockingToolStripMenuItem.Click += new System.EventHandler(this.keepDockingToolStripMenuItem_Click);
            // 
            // Details
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackgroundImage = global::BF4_LoadoutChecker.Properties.Resources.bg_video_overlay_battlefield4;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(239, 468);
            this.Controls.Add(this.fLP_base);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Details";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Details";
            this.Resize += new System.EventHandler(this.Details_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel fLP_base;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dockingLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lEFTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rIGHTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tOPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bOTTOMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keepDockingToolStripMenuItem;
    }
}