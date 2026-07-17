using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace STR_WF_AT
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.leftLabel = new System.Windows.Forms.Label();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.normalMapCorrectionCheckbox = new System.Windows.Forms.CheckBox();
            this.threadCountSelector = new System.Windows.Forms.NumericUpDown();
            this.threadLabel = new System.Windows.Forms.Label();
            this.formatComboBox = new System.Windows.Forms.ComboBox();
            this.formatLabel = new System.Windows.Forms.Label();
            this.settingsButton = new System.Windows.Forms.Button();
            this.settingsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearConfigMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuToggleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.rightLabel = new System.Windows.Forms.Label();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.infoPanel = new System.Windows.Forms.Panel();
            this.infoLabel = new System.Windows.Forms.Label();
            this.gameSelectorPanel = new System.Windows.Forms.Panel();
            this.buildModeLabel = new System.Windows.Forms.Label();
            this.stuntfestButton = new System.Windows.Forms.Button();
            this.wreckfestButton = new System.Windows.Forms.Button();
            this.benchmarkButton = new System.Windows.Forms.Button();
            this.progressPanel = new System.Windows.Forms.Panel();
            this.outputTextBox = new System.Windows.Forms.RichTextBox();
            this.logoLabel = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.progressBar = new STR_WF_AT.TextProgressBar();
            this.leftPanel.SuspendLayout();
            this.settingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.threadCountSelector)).BeginInit();
            this.rightPanel.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.infoPanel.SuspendLayout();
            this.gameSelectorPanel.SuspendLayout();
            this.progressPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // leftPanel
            // 
            this.leftPanel.AllowDrop = true;
            this.leftPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.leftPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.leftPanel.Controls.Add(this.leftLabel);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Location = new System.Drawing.Point(0, 130);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Padding = new System.Windows.Forms.Padding(10);
            this.leftPanel.Size = new System.Drawing.Size(350, 287);
            this.leftPanel.TabIndex = 0;
            this.leftPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.DumpPanel_DragDrop);
            this.leftPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.DumpPanel_DragEnter);
            this.leftPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.DropPanel_Paint);
            // 
            // leftLabel
            // 
            this.leftLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.leftLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.leftLabel.Location = new System.Drawing.Point(10, 10);
            this.leftLabel.Name = "leftLabel";
            this.leftLabel.Size = new System.Drawing.Size(328, 265);
            this.leftLabel.TabIndex = 0;
            this.leftLabel.Text = "DUMP ASSET\r\n\r\nDrag and Drop files here to\r\ndump/decompress\r\n(.bmap textures/bag data files)";
            this.leftLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.leftLabel.Click += new System.EventHandler(this.DumpPanel_Click);
            // 
            // settingsPanel
            // 
            this.settingsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.settingsPanel.Controls.Add(this.normalMapCorrectionCheckbox);
            this.settingsPanel.Controls.Add(this.threadCountSelector);
            this.settingsPanel.Controls.Add(this.threadLabel);
            this.settingsPanel.Controls.Add(this.formatComboBox);
            this.settingsPanel.Controls.Add(this.formatLabel);
            this.settingsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.settingsPanel.Location = new System.Drawing.Point(0, 0);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Padding = new System.Windows.Forms.Padding(10);
            this.settingsPanel.Size = new System.Drawing.Size(900, 130);
            this.settingsPanel.TabIndex = 1;
            // 
            // normalMapCorrectionCheckbox
            // 
            this.normalMapCorrectionCheckbox.AutoSize = true;
            this.normalMapCorrectionCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.normalMapCorrectionCheckbox.ForeColor = System.Drawing.Color.Gainsboro;
            this.normalMapCorrectionCheckbox.Location = new System.Drawing.Point(6, 76);
            this.normalMapCorrectionCheckbox.Name = "normalMapCorrectionCheckbox";
            this.normalMapCorrectionCheckbox.Size = new System.Drawing.Size(208, 19);
            this.normalMapCorrectionCheckbox.TabIndex = 4;
            this.normalMapCorrectionCheckbox.Text = "Raw DDS normal map dump";
            this.toolTip.SetToolTip(this.normalMapCorrectionCheckbox, "Keep BC5/BC3 DDS format for normal maps instead of converting to uncompressed RGB" +
        "A.");
            this.normalMapCorrectionCheckbox.UseVisualStyleBackColor = true;
            this.normalMapCorrectionCheckbox.Visible = false;
            this.normalMapCorrectionCheckbox.CheckedChanged += new System.EventHandler(this.NormalMapCorrectionCheckbox_CheckedChanged);
            // 
            // threadCountSelector
            // 
            this.threadCountSelector.BackColor = System.Drawing.Color.Gainsboro;
            this.threadCountSelector.Cursor = System.Windows.Forms.Cursors.Default;
            this.threadCountSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.threadCountSelector.ForeColor = System.Drawing.Color.Black;
            this.threadCountSelector.Location = new System.Drawing.Point(838, 100);
            this.threadCountSelector.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.threadCountSelector.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.threadCountSelector.Name = "threadCountSelector";
            this.threadCountSelector.Size = new System.Drawing.Size(53, 21);
            this.threadCountSelector.TabIndex = 3;
            this.toolTip.SetToolTip(this.threadCountSelector, "Set the number of workers for decompressing/building assets.");
            this.threadCountSelector.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.threadCountSelector.ValueChanged += new System.EventHandler(this.ThreadCountSelector_ValueChanged);
            // 
            // threadLabel
            // 
            this.threadLabel.AutoSize = true;
            this.threadLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.threadLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.threadLabel.Location = new System.Drawing.Point(713, 101);
            this.threadLabel.Name = "threadLabel";
            this.threadLabel.Size = new System.Drawing.Size(123, 16);
            this.threadLabel.TabIndex = 2;
            this.threadLabel.Text = "Worker Threads:";
            this.toolTip.SetToolTip(this.threadLabel, "Worker Threads isn\'t 1:1 to how many\nthreads your CPU has, default is double\nyour" +
        " CPU thread count.");
            this.threadLabel.Click += new System.EventHandler(this.ThreadLabel_Click);
            // 
            // formatComboBox
            // 
            this.formatComboBox.BackColor = System.Drawing.Color.Gainsboro;
            this.formatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.formatComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.formatComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.formatComboBox.ForeColor = System.Drawing.Color.Black;
            this.formatComboBox.FormattingEnabled = true;
            this.formatComboBox.Items.AddRange(new object[] {
            "TGA",
            "PNG",
            "DDS"});
            this.formatComboBox.Location = new System.Drawing.Point(164, 98);
            this.formatComboBox.Name = "formatComboBox";
            this.formatComboBox.Size = new System.Drawing.Size(72, 23);
            this.formatComboBox.TabIndex = 1;
            this.formatComboBox.SelectedIndexChanged += new System.EventHandler(this.FormatComboBox_SelectedIndexChanged);
            // 
            // formatLabel
            // 
            this.formatLabel.AutoSize = true;
            this.formatLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.formatLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.formatLabel.Location = new System.Drawing.Point(3, 100);
            this.formatLabel.Name = "formatLabel";
            this.formatLabel.Size = new System.Drawing.Size(159, 16);
            this.formatLabel.TabIndex = 0;
            this.formatLabel.Text = "Dump Format (BMAP):";
            this.toolTip.SetToolTip(this.formatLabel, "Set the texture dump image format.");
            // 
            // settingsButton
            // 
            this.settingsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.settingsButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.settingsButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.settingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.settingsButton.ForeColor = System.Drawing.Color.White;
            this.settingsButton.Location = new System.Drawing.Point(10, 10);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(120, 30);
            this.settingsButton.TabIndex = 2;
            this.settingsButton.Text = "⚙ Settings";
            this.toolTip.SetToolTip(this.settingsButton, "Open tool settings.");
            this.settingsButton.UseVisualStyleBackColor = false;
            this.settingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // settingsContextMenu
            // 
            this.settingsContextMenu.Name = "settingsContextMenu";
            this.settingsContextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // selectToolsMenuItem
            // 
            this.selectToolsMenuItem.Name = "selectToolsMenuItem";
            this.selectToolsMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // clearConfigMenuItem
            // 
            this.clearConfigMenuItem.Name = "clearConfigMenuItem";
            this.clearConfigMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // contextMenuToggleMenuItem
            // 
            this.contextMenuToggleMenuItem.Name = "contextMenuToggleMenuItem";
            this.contextMenuToggleMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // rightPanel
            // 
            this.rightPanel.AllowDrop = true;
            this.rightPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(55)))), ((int)(((byte)(43)))));
            this.rightPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rightPanel.Controls.Add(this.rightLabel);
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.rightPanel.Location = new System.Drawing.Point(350, 130);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Padding = new System.Windows.Forms.Padding(10);
            this.rightPanel.Size = new System.Drawing.Size(350, 287);
            this.rightPanel.TabIndex = 1;
            this.rightPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.BuildPanel_DragDrop);
            this.rightPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.BuildPanel_DragEnter);
            this.rightPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.DropPanel_Paint);
            // 
            // rightLabel
            // 
            this.rightLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.rightLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.rightLabel.Location = new System.Drawing.Point(10, 10);
            this.rightLabel.Name = "rightLabel";
            this.rightLabel.Size = new System.Drawing.Size(328, 265);
            this.rightLabel.TabIndex = 0;
            this.rightLabel.Text = "BUILD ASSET\r\n\r\nDrag and Drop files here to build\r\n(Textures: TGA/PNG/DDS)\r\n(WF1 Models: BGO3)";
            this.rightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rightLabel.Click += new System.EventHandler(this.BuildPanel_Click);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.infoPanel);
            this.mainPanel.Controls.Add(this.rightPanel);
            this.mainPanel.Controls.Add(this.leftPanel);
            this.mainPanel.Controls.Add(this.settingsPanel);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(900, 417);
            this.mainPanel.TabIndex = 0;
            this.mainPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.MainPanel_Paint);
            // 
            // infoPanel
            // 
            this.infoPanel.AllowDrop = true;
            this.infoPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(58)))));
            this.infoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.infoPanel.Controls.Add(this.infoLabel);
            this.infoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoPanel.Location = new System.Drawing.Point(700, 130);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Padding = new System.Windows.Forms.Padding(10);
            this.infoPanel.Size = new System.Drawing.Size(200, 287);
            this.infoPanel.TabIndex = 2;
            this.infoPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.InfoPanel_DragDrop);
            this.infoPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.InfoPanel_DragEnter);
            this.infoPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.DropPanel_Paint);
            // 
            // infoLabel
            // 
            this.infoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.infoLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.infoLabel.Location = new System.Drawing.Point(10, 10);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(178, 265);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "ASSET INFO\r\n\r\nDrag and Drop file for info analysis\r\n(Type, Version etc)\r\n";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.infoLabel.Click += new System.EventHandler(this.InfoPanel_Click);
            // 
            // gameSelectorPanel
            // 
            this.gameSelectorPanel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gameSelectorPanel.BackColor = System.Drawing.Color.Transparent;
            this.gameSelectorPanel.Controls.Add(this.buildModeLabel);
            this.gameSelectorPanel.Controls.Add(this.stuntfestButton);
            this.gameSelectorPanel.Controls.Add(this.wreckfestButton);
            this.gameSelectorPanel.Location = new System.Drawing.Point(290, 50);
            this.gameSelectorPanel.Name = "gameSelectorPanel";
            this.gameSelectorPanel.Size = new System.Drawing.Size(320, 65);
            this.gameSelectorPanel.TabIndex = 1;
            this.gameSelectorPanel.Visible = false;
            // 
            // buildModeLabel
            // 
            this.buildModeLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.buildModeLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.buildModeLabel.Location = new System.Drawing.Point(5, -4);
            this.buildModeLabel.Name = "buildModeLabel";
            this.buildModeLabel.Size = new System.Drawing.Size(310, 18);
            this.buildModeLabel.TabIndex = 2;
            this.buildModeLabel.Text = "Build Mode";
            this.buildModeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stuntfestButton
            // 
            this.stuntfestButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.stuntfestButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.stuntfestButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.stuntfestButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stuntfestButton.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.stuntfestButton.ForeColor = System.Drawing.Color.Gray;
            this.stuntfestButton.Location = new System.Drawing.Point(165, 20);
            this.stuntfestButton.Name = "stuntfestButton";
            this.stuntfestButton.Size = new System.Drawing.Size(150, 40);
            this.stuntfestButton.TabIndex = 1;
            this.stuntfestButton.Text = "SF";
            this.stuntfestButton.UseVisualStyleBackColor = false;
            this.stuntfestButton.Click += new System.EventHandler(this.StuntfestButton_Click);
            // 
            // wreckfestButton
            // 
            this.wreckfestButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.wreckfestButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.wreckfestButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.wreckfestButton.FlatAppearance.BorderSize = 2;
            this.wreckfestButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.wreckfestButton.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.wreckfestButton.ForeColor = System.Drawing.Color.White;
            this.wreckfestButton.Location = new System.Drawing.Point(5, 20);
            this.wreckfestButton.Name = "wreckfestButton";
            this.wreckfestButton.Size = new System.Drawing.Size(150, 40);
            this.wreckfestButton.TabIndex = 0;
            this.wreckfestButton.Text = "WF";
            this.wreckfestButton.UseVisualStyleBackColor = false;
            this.wreckfestButton.Click += new System.EventHandler(this.WreckfestButton_Click);
            // 
            // benchmarkButton
            // 
            this.benchmarkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.benchmarkButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.benchmarkButton.FlatAppearance.BorderSize = 0;
            this.benchmarkButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.benchmarkButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.benchmarkButton.ForeColor = System.Drawing.Color.White;
            this.benchmarkButton.Location = new System.Drawing.Point(730, 10);
            this.benchmarkButton.Name = "benchmarkButton";
            this.benchmarkButton.Size = new System.Drawing.Size(160, 30);
            this.benchmarkButton.TabIndex = 3;
            this.benchmarkButton.Text = "Auto Tune Work Threads";
            this.toolTip.SetToolTip(this.benchmarkButton, "Run a build bmap benchmark to find the optimal thread count for building.");
            this.benchmarkButton.UseVisualStyleBackColor = false;
            this.benchmarkButton.Click += new System.EventHandler(this.BenchmarkButton_Click);
            // 
            // progressPanel
            // 
            this.progressPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.progressPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.progressPanel.Controls.Add(this.progressBar);
            this.progressPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressPanel.Location = new System.Drawing.Point(0, 617);
            this.progressPanel.Name = "progressPanel";
            this.progressPanel.Size = new System.Drawing.Size(900, 33);
            this.progressPanel.TabIndex = 4;
            // 
            // outputTextBox
            // 
            this.outputTextBox.BackColor = System.Drawing.Color.Black;
            this.outputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.outputTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.outputTextBox.Font = new System.Drawing.Font("Consolas", 11F);
            this.outputTextBox.ForeColor = System.Drawing.Color.Lime;
            this.outputTextBox.Location = new System.Drawing.Point(0, 417);
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ReadOnly = true;
            this.outputTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.outputTextBox.Size = new System.Drawing.Size(900, 200);
            this.outputTextBox.TabIndex = 5;
            this.outputTextBox.Text = "";
            // 
            // logoLabel
            // 
            this.logoLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.logoLabel.BackColor = System.Drawing.Color.Transparent;
            this.logoLabel.Font = new System.Drawing.Font("Arial Black", 16F, System.Drawing.FontStyle.Bold);
            this.logoLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.logoLabel.Location = new System.Drawing.Point(250, 10);
            this.logoLabel.Name = "logoLabel";
            this.logoLabel.Size = new System.Drawing.Size(400, 35);
            this.logoLabel.TabIndex = 7;
            this.logoLabel.Text = "STRmods Wreckfest Asset Tool";
            this.logoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
            // 
            // progressBar
            // 
            this.progressBar.DisplayText = "";
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.progressBar.Location = new System.Drawing.Point(0, 0);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(898, 31);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 0;
            this.progressBar.UseWaitCursor = true;
            this.progressBar.Visible = false;
            this.progressBar.Click += new System.EventHandler(this.ProgressBar_Click_1);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(900, 650);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.outputTextBox);
            this.Controls.Add(this.progressPanel);
            this.Controls.Add(this.benchmarkButton);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.gameSelectorPanel);
            this.Controls.Add(this.logoLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(920, 693);
            this.MinimumSize = new System.Drawing.Size(920, 693);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "STRmods: WF-AT";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.leftPanel.ResumeLayout(false);
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.threadCountSelector)).EndInit();
            this.rightPanel.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.infoPanel.ResumeLayout(false);
            this.gameSelectorPanel.ResumeLayout(false);
            this.progressPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void FixControlZOrder()
        {
            this.gameSelectorPanel.BringToFront();
            this.settingsButton.BringToFront();
            this.benchmarkButton.BringToFront();
            this.progressPanel.BringToFront();
            this.logoLabel.BringToFront();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            PositionGameSelector();
        }

        private void MainForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            Config.Save();
        }

        void FormatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedFormat = (DumpAsset.OutputFormat)formatComboBox.SelectedIndex;
            Config.DumpFormat = selectedFormat;
            Config.Save();

            normalMapCorrectionCheckbox.Visible = (selectedFormat == DumpAsset.OutputFormat.DDS);

            bool isStuntfest = !string.IsNullOrEmpty(Config.StuntfestToolDir) && Config.ActiveToolDir == Config.StuntfestToolDir;
            if (isStuntfest && (selectedFormat == DumpAsset.OutputFormat.DDS || selectedFormat == DumpAsset.OutputFormat.PNG))
            {
                string formatName = selectedFormat == DumpAsset.OutputFormat.DDS ? "DDS" : "PNG";
                LogOutput(string.Format("⚠ Warning: The official Stuntfest build tools don't support building from {0}, only TGA is supported.\nWreckfest build tools will need to be used for {0}\n", formatName), Color.Yellow);
            }
        }

        private void ThreadCountSelector_ValueChanged(object sender, EventArgs e)
        {
            Config.ThreadCount = (int)threadCountSelector.Value;
            Config.Save();
        }

        private void NormalMapCorrectionCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Config.NormalMapCorrection = normalMapCorrectionCheckbox.Checked;
            Config.Save();
        }

        private void PositionGameSelector()
        {
            gameSelectorPanel.Location = new Point(
                (this.ClientSize.Width - gameSelectorPanel.Width) / 2,
                50
            );
        }

        #endregion

        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.Panel settingsPanel;
        private System.Windows.Forms.CheckBox normalMapCorrectionCheckbox;
        private System.Windows.Forms.NumericUpDown threadCountSelector;
        private System.Windows.Forms.Label threadLabel;
        private System.Windows.Forms.ComboBox formatComboBox;
        private System.Windows.Forms.Label formatLabel;
        private System.Windows.Forms.Label leftLabel;
        private System.Windows.Forms.Panel rightPanel;
        private System.Windows.Forms.Label rightLabel;
        private System.Windows.Forms.Panel infoPanel;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel gameSelectorPanel;
        private System.Windows.Forms.Label buildModeLabel;
        private System.Windows.Forms.Button stuntfestButton;
        private System.Windows.Forms.Button wreckfestButton;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.ToolStripMenuItem selectToolsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearConfigMenuItem;
        private System.Windows.Forms.Button benchmarkButton;
        private System.Windows.Forms.Panel progressPanel;
        private TextProgressBar progressBar;
        private System.Windows.Forms.RichTextBox outputTextBox;
        private System.Windows.Forms.Label logoLabel;
        private System.Windows.Forms.ToolTip toolTip;
    }
}