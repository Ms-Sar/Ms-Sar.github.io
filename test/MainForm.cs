using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using STR_WF_AT.Core;

namespace STR_WF_AT
{
    // Main GUI Form
    public partial class MainForm : Form
    {
        private DumpAsset.OutputFormat selectedFormat;
        private System.Threading.CancellationTokenSource benchmarkCancellation;
        private bool isBenchmarkRunning = false;
        private int currentProgress = 0;
        private int totalProgress = 0;
        private int skippedFiles = 0;
        private BuildAsset.BuildMode activeBuildMode = BuildAsset.BuildMode.Wreckfest1;
        private readonly Button wf2BuildButton;
        private readonly Button rruBuildButton;
        private string NamcoCode = "";
        private ContextMenuStrip settingsContextMenu;
        private ToolStripMenuItem contextMenuToggleMenuItem;
        private ToolStripMenuItem sendToToggleMenuItem;
        private ToolStripMenuItem checkForUpdatesMenuItem;

        public MainForm()
        {
            this.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

            selectedFormat = Config.DumpFormat;
            InitializeComponent();

            outputTextBox.DetectUrls = true;
            outputTextBox.LinkClicked += OutputTextBoxLinkClicked;

            // Wire up the settings menu safely
            SetupSettingsMenu();

            // Load saved build mode state from Config
            RestoreSavedBuildMode();

            int buttonWidth = 75;
            int spacing = 5;

            rruBuildButton = new Button
            {
                Text = "RRU",
                Size = new Size(buttonWidth, wreckfestButton.Height),
                Location = new Point(5, wreckfestButton.Top),
                FlatStyle = FlatStyle.Flat,
                Font = wreckfestButton.Font,
                Cursor = Cursors.Hand
            };
            rruBuildButton.Click += RruBuildButton_Click;
            gameSelectorPanel.Controls.Add(rruBuildButton);

            wreckfestButton.Size = new Size(buttonWidth, wreckfestButton.Height);
            wreckfestButton.Location = new Point(rruBuildButton.Right + spacing, wreckfestButton.Top);

            wf2BuildButton = new Button
            {
                Text = "WF2",
                Size = new Size(buttonWidth, wreckfestButton.Height),
                Location = new Point(wreckfestButton.Right + spacing, wreckfestButton.Top),
                FlatStyle = FlatStyle.Flat,
                Font = wreckfestButton.Font,
                Cursor = Cursors.Hand
            };
            wf2BuildButton.Click += Wf2BuildButton_Click;
            gameSelectorPanel.Controls.Add(wf2BuildButton);

            stuntfestButton.Size = new Size(buttonWidth, stuntfestButton.Height);
            stuntfestButton.Location = new Point(wf2BuildButton.Right + spacing, stuntfestButton.Top);

            gameSelectorPanel.Height = wf2BuildButton.Bottom + 10;

            FixControlZOrder();
            ApplyConfigToUI();
            GameToolsLocator.DetectToolDirectories(msg => LogOutput(msg, Color.Cyan));
            UpdateGameSelectorVisibility();
            UpdateGameSelection();

            this.Shown += (sender, e) =>
            {
                CenterGameSelectorButtons();
            };
        }

        private void OutputTextBoxLinkClicked(
            object sender,
            LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
            catch
            {
                // Do nothing if Windows cannot open the default browser.
            }
        }

        private void SetupSettingsMenu()
        {
            settingsContextMenu = new ContextMenuStrip();

            // Match your dark theme
            settingsContextMenu.BackColor = Color.FromArgb(45, 45, 48);
            settingsContextMenu.ForeColor = Color.White;
            settingsContextMenu.ShowImageMargin = false;
            settingsContextMenu.Renderer =
                new ToolStripProfessionalRenderer(
                    new DarkMenuColorTable());

            var selectToolsItem = new ToolStripMenuItem("Select Tools Directory");
            selectToolsItem.Click += BrowseToolsButton_Click;

            var clearConfigItem = new ToolStripMenuItem("Clear Configuration");
            clearConfigItem.Click += ClearConfigButton_Click;

            // Check initial state to set the correct text on load
            bool isInstalled = ContextMenuHelper.IsContextMenuInstalled();
            contextMenuToggleMenuItem = new ToolStripMenuItem(isInstalled ? "Disable Context Menu Entry" : "Enable Context Menu Entry");
            contextMenuToggleMenuItem.Click += ContextMenuToggleMenuItem_Click;

            // --- NEW SEND TO MENU ITEM ---
            bool isSendToInstalled = ContextMenuHelper.IsSendToInstalled();
            sendToToggleMenuItem = new ToolStripMenuItem(isSendToInstalled ? "Disable 'Send To' Option" : "Enable 'Send To' Option");
            sendToToggleMenuItem.Click += SendToToggleMenuItem_Click;

            // Add them to the menu with a separator line
            settingsContextMenu.Items.Add(selectToolsItem);
            settingsContextMenu.Items.Add(clearConfigItem);
            settingsContextMenu.Items.Add(new ToolStripSeparator());
            settingsContextMenu.Items.Add(contextMenuToggleMenuItem);
            settingsContextMenu.Items.Add(sendToToggleMenuItem);

            settingsContextMenu.Items.Add(new ToolStripSeparator());

            checkForUpdatesMenuItem =
                new ToolStripMenuItem("Check for Updates");

            checkForUpdatesMenuItem.Click +=
                CheckForUpdatesMenuItemClick;

            settingsContextMenu.Items.Add(checkForUpdatesMenuItem);

            // Force the button styling and click event
            if (settingsButton != null)
            {
                settingsButton.Text = "WF-AT Settings";
                settingsButton.Size = new Size(160, 30); // Matches Benchmark Button size
                settingsButton.Click -= SettingsButton_Click; // Safety clear
                settingsButton.Click += SettingsButton_Click;
            }
        }

        private sealed class DarkMenuColorTable :
    ProfessionalColorTable
        {
            public override Color ToolStripDropDownBackground
            {
                get { return Color.FromArgb(45, 45, 48); }
            }

            public override Color MenuItemSelected
            {
                get { return Color.FromArgb(0, 85, 150); }
            }

            public override Color MenuItemSelectedGradientBegin
            {
                get { return Color.FromArgb(0, 85, 150); }
            }

            public override Color MenuItemSelectedGradientEnd
            {
                get { return Color.FromArgb(0, 85, 150); }
            }

            public override Color MenuItemBorder
            {
                get { return Color.FromArgb(0, 122, 204); }
            }

            public override Color SeparatorDark
            {
                get { return Color.FromArgb(80, 80, 85); }
            }

            public override Color SeparatorLight
            {
                get { return Color.FromArgb(45, 45, 48); }
            }
        }

        private void ContextMenuToggleMenuItem_Click(object sender, EventArgs e)
        {
            bool isInstalled = ContextMenuHelper.IsContextMenuInstalled();

            if (!isInstalled)
            {
                ContextMenuHelper.InstallContextMenu();
            }
            else
            {
                ContextMenuHelper.RemoveContextMenu();
            }

            // Update the UI text based on the actual Windows Registry status
            bool isNowInstalled = ContextMenuHelper.IsContextMenuInstalled();
            contextMenuToggleMenuItem.Text = isNowInstalled ? "Disable Context Menu Entry" : "Enable Context Menu Entry";
        }

        private void SendToToggleMenuItem_Click(object sender, EventArgs e)
        {
            if (!ContextMenuHelper.IsSendToInstalled())
            {
                ContextMenuHelper.InstallSendTo();
            }
            else
            {
                ContextMenuHelper.RemoveSendTo();
            }

            // Update the UI text
            sendToToggleMenuItem.Text = ContextMenuHelper.IsSendToInstalled() ? "Disable 'Send To' Option" : "Enable 'Send To' Option";
        }

        private async void CheckForUpdatesMenuItemClick(
    object sender,
    EventArgs e)
        {
            if (checkForUpdatesMenuItem == null)
            {
                return;
            }

            checkForUpdatesMenuItem.Enabled = false;

            try
            {
                LogOutput(
                    "Checking for WF-AT update...",
                    Color.Cyan);

                UpdateCheckResult result =
                    await UpdateChecker.CheckAsync();

                // Deliberately silent for unavailable internet, timeout,
                // GitHub Pages downtime, invalid JSON, and similar failures.
                if (result == null)
                {
                    LogOutput(
                        "Failed to check available WF-AT builds.",
                        Color.Red);
                    return;
                }

                if (!result.IsUpdateAvailable)
                {
                    LogOutput(
                        "This version of WF-AT is already the latest version.",
                        Color.Lime);

                    return;
                }

                LogOutput(
                    "New WF-AT build found: v" +
                    result.LatestVersion +
                    " (you have v" +
                    result.CurrentVersion +
                    ").",
                    Color.Yellow);

                if (result.NewUpdates != null &&
                    result.NewUpdates.Count > 0)
                {
                    LogOutput("What's new:", Color.Cyan);

                    foreach (UpdateEntry update in result.NewUpdates)
                    {
                        LogOutput(
                            "v" + update.Version,
                            Color.White);

                        if (update.Changes == null)
                        {
                            continue;
                        }

                        foreach (string change in update.Changes)
                        {
                            if (!string.IsNullOrWhiteSpace(change))
                            {
                                LogOutput(
                                    "  • " + change,
                                    Color.LightGray);
                            }
                        }
                    }
                }

                LogOutput(
                    "Download from Nexus Mods: " +
                    UpdateChecker.NexusModsUrl,
                    Color.Cyan);

                LogOutput(
                    "Download from STRmods.github.io: " +
                    UpdateChecker.StrmodsUrl,
                    Color.Cyan);
            }
            finally
            {
                checkForUpdatesMenuItem.Enabled = true;
            }
        }

        // NEW METHOD: Safely restore the state based on the saved active directory
        private void RestoreSavedBuildMode()
        {
            activeBuildMode = Config.SelectedBuildMode;

            // Avoid retaining an unavailable Stuntfest selection if its tools are gone.
            if (activeBuildMode == BuildAsset.BuildMode.Stuntfest &&
                string.IsNullOrEmpty(Config.StuntfestToolDir))
            {
                activeBuildMode = BuildAsset.BuildMode.Wreckfest1;
                Config.SelectedBuildMode = activeBuildMode;
                Config.ActiveToolDir = Config.WreckfestToolDir;
                Config.Save();
            }

            // WF1, WF2, and RRU use the Wreckfest tool directory.
            if (activeBuildMode == BuildAsset.BuildMode.Wreckfest1 ||
                activeBuildMode == BuildAsset.BuildMode.Wreckfest2 ||
                activeBuildMode == BuildAsset.BuildMode.RRU)
            {
                Config.ActiveToolDir = Config.WreckfestToolDir;
            }
            else if (activeBuildMode == BuildAsset.BuildMode.Stuntfest)
            {
                Config.ActiveToolDir = Config.StuntfestToolDir;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData >= Keys.A && keyData <= Keys.Z)
            {
                NamcoCode += ((char)keyData).ToString();
                if (NamcoCode.Length > 8) NamcoCode = NamcoCode.Substring(NamcoCode.Length - 8);

                if (NamcoCode == "ILOVERRU")
                {
                    Config.RRUbuild = true;
                    Config.Save();
                    UpdateGameSelectorVisibility();
                    CenterGameSelectorButtons();
                    LogOutput("✚ Ridge Racer Unbounded Build Mode Unlocked!", Color.Lime);
                    NamcoCode = "";
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        void ApplyConfigToUI()
        {
            formatComboBox.SelectedIndex = (int)Config.DumpFormat;
            // 32-bit safeguard
            if (!Environment.Is64BitProcess)
            {
                // Force the thread count to never exceed 4
                threadCountSelector.Maximum = Math.Min(threadCountSelector.Maximum, 4);
                Config.ThreadCount = Math.Min(Config.ThreadCount, 4);
                toolTip.SetToolTip(threadLabel, "32bit mode detected, worker threads limited to 4.");
                benchmarkButton.Enabled = false;
                benchmarkButton.Text = "Auto Tune Disabled (32bit)";
                toolTip.SetToolTip(benchmarkButton, "32bit mode detected, auto tune disabled.");
            }
            threadCountSelector.Value = Config.ThreadCount;
            normalMapCorrectionCheckbox.Checked = Config.NormalMapCorrection;
            normalMapCorrectionCheckbox.Visible = (Config.DumpFormat == DumpAsset.OutputFormat.DDS);
            progressBar.SetProgressBarColor(Color.FromArgb(0, 122, 204));

            // Check registry status and apply checkmark in the menu
            if (contextMenuToggleMenuItem != null)
            {
                contextMenuToggleMenuItem.Checked = ContextMenuHelper.IsContextMenuInstalled();
            }
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            // Show the dropdown menu directly under the Settings button
            settingsContextMenu.Show(settingsButton, new Point(0, settingsButton.Height));
        }

        void UpdateGameSelectorVisibility()
        {
            bool hasWreckfest = !string.IsNullOrEmpty(Config.WreckfestToolDir);
            bool hasStuntfest = !string.IsNullOrEmpty(Config.StuntfestToolDir);
            bool oneGameDetected = hasWreckfest || hasStuntfest;

            gameSelectorPanel.Visible = oneGameDetected;

            if (wf2BuildButton != null)
            {
                wf2BuildButton.Visible = hasWreckfest;
                wf2BuildButton.Enabled = hasWreckfest;
            }

            if (rruBuildButton != null)
            {
                rruBuildButton.Visible = Config.RRUbuild;
                rruBuildButton.Enabled = hasWreckfest;
            }

            wreckfestButton.Visible = hasWreckfest;
            stuntfestButton.Visible = hasStuntfest;

            wreckfestButton.Enabled = hasWreckfest;
            wreckfestButton.Cursor = hasWreckfest ? Cursors.Hand : Cursors.Default;

            stuntfestButton.Enabled = hasStuntfest;
            stuntfestButton.Cursor = hasStuntfest ? Cursors.Hand : Cursors.Default;

            CenterGameSelectorButtons();
        }

        void CenterGameSelectorButtons()
        {
            if (!gameSelectorPanel.Visible) return;

            int spacing = 5;
            var buttons = new List<Button> { rruBuildButton, wreckfestButton, wf2BuildButton, stuntfestButton };
            var visibleButtons = buttons.Where(b => b != null && b.Visible).ToList();

            if (visibleButtons.Count > 0)
            {
                int totalWidth = visibleButtons.Sum(b => b.Width) + ((visibleButtons.Count - 1) * spacing);
                int startX = (gameSelectorPanel.Width - totalWidth) / 2;
                int currentX = startX;

                foreach (var btn in visibleButtons)
                {
                    btn.Location = new Point(currentX, btn.Location.Y);
                    currentX += btn.Width + spacing;
                }
            }
        }

        void UpdateGameSelection()
        {
            bool isWreckfest = activeBuildMode == BuildAsset.BuildMode.Wreckfest1;
            bool isStuntfest = activeBuildMode == BuildAsset.BuildMode.Stuntfest;
            bool isWF2 = activeBuildMode == BuildAsset.BuildMode.Wreckfest2;
            bool isRRU = activeBuildMode == BuildAsset.BuildMode.RRU;

            void applyStyle(Button btn, bool isActive)
            {
                if (btn == null) return;
                btn.BackColor = isActive ? Color.FromArgb(60, 60, 60) : Color.FromArgb(40, 40, 40);
                btn.ForeColor = isActive ? Color.White : Color.Gray;
                btn.FlatAppearance.BorderColor = isActive ? Color.FromArgb(0, 122, 204) : Color.Gray;
                btn.FlatAppearance.BorderSize = isActive ? 2 : 1;
            }

            applyStyle(wreckfestButton, isWreckfest);
            applyStyle(stuntfestButton, isStuntfest);
            applyStyle(wf2BuildButton, isWF2);
            applyStyle(rruBuildButton, isRRU);
        }

        void WreckfestButton_Click(object sender, EventArgs e)
        {
            activeBuildMode = BuildAsset.BuildMode.Wreckfest1;
            Config.SelectedBuildMode = activeBuildMode;
            Config.ActiveToolDir = Config.WreckfestToolDir;
            Config.Save();

            UpdateGameSelection();
            LogOutput("Switched to Wreckfest build mode\n", Color.Cyan);
        }

        void StuntfestButton_Click(object sender, EventArgs e)
        {
            activeBuildMode = BuildAsset.BuildMode.Stuntfest;
            Config.SelectedBuildMode = activeBuildMode;
            Config.ActiveToolDir = Config.StuntfestToolDir;
            Config.Save();

            UpdateGameSelection();
            LogOutput("Switched to Stuntfest build mode\n", Color.Cyan);

            if (!string.IsNullOrEmpty(Config.StuntfestToolDir) &&
                (Config.DumpFormat == DumpAsset.OutputFormat.DDS ||
                 Config.DumpFormat == DumpAsset.OutputFormat.PNG))
            {
                string formatName = Config.DumpFormat == DumpAsset.OutputFormat.DDS
                    ? "DDS"
                    : "PNG";

                LogOutput(
                    $"⚠ Warning: The official Stuntfest build tools don't support building from {formatName}, only TGA is supported.\n" +
                    $"Wreckfest build tools will need to be used for {formatName}\n",
                    Color.Yellow);
            }
        }

        void Wf2BuildButton_Click(object sender, EventArgs e)
        {
            activeBuildMode = BuildAsset.BuildMode.Wreckfest2;
            Config.SelectedBuildMode = activeBuildMode;
            Config.ActiveToolDir = Config.WreckfestToolDir;
            Config.Save();

            UpdateGameSelection();
            LogOutput("Switched to Wreckfest 2 build mode\n", Color.Cyan);
        }

        void RruBuildButton_Click(object sender, EventArgs e)
        {
            activeBuildMode = BuildAsset.BuildMode.RRU;
            Config.SelectedBuildMode = activeBuildMode;
            Config.ActiveToolDir = Config.WreckfestToolDir;
            Config.Save();

            UpdateGameSelection();
            LogOutput("Switched to Ridge Racer Unbounded build mode\n", Color.Cyan);
        }

        private void DropPanel_Paint(object sender, PaintEventArgs e)
        {
            if (sender is Panel panel)
            {
                // Draw a subtle dashed border inside the panel padding
                using (Pen dashedPen = new Pen(Color.FromArgb(120, 120, 120), 2))
                {
                    dashedPen.DashPattern = new float[] { 5, 5 };
                    Rectangle rect = new Rectangle(2, 2, panel.Width - 5, panel.Height - 5);
                    e.Graphics.DrawRectangle(dashedPen, rect);
                }
            }
        }

        private void InfoPanel_Click(object sender, EventArgs e)
        {
            LogOutput("To analyze a file, simply drag it onto the Asset Info panel.\n", Color.Cyan);
        }

        void InfoPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        void InfoPanel_DragDrop(object sender, DragEventArgs e)
        {
            string[] items = (string[])e.Data.GetData(DataFormats.FileDrop);
            Task.Run(() => ProcessInfoFiles(items));
        }

        void ProcessInfoFiles(string[] items)
        {
            foreach (var item in items)
            {
                if (File.Exists(item))
                {
                    LogOutput($"\n=== Analyzing Asset: {Path.GetFileName(item)} ===", Color.Yellow);
                    try
                    {
                        byte[] rawData = File.ReadAllBytes(item);
                        string ext = Path.GetExtension(item);

                        var info = BagFileAnalyzer.AnalyzeBagFile(rawData, ext);

                        bool isCompressed = false;
                        if (info != null)
                        {
                            isCompressed = info.IsCompressed;
                            LogOutput($"Type Header: {info.TypeHeader}");

                            if (!string.IsNullOrEmpty(info.TypeName))
                            {
                                LogOutput($"Type: {info.TypeName}");
                            }

                            LogOutput($"Version: {info.Version}");
                            LogOutput($"Game Version: {info.GameVersion}");
                            LogOutput($"Compressed: {(isCompressed ? "Yes" : "No")}");
                        }
                        else
                        {
                            LogOutput("Could not analyze Bag file headers. File might be too small.", Color.Gray);
                        }

                        // Check the actual internal header, not the file extension!
                        string actualType = info?.TypeHeader?.Trim().ToLower();

                        if (actualType == "bmap" || actualType == "pamb")
                        {
                            byte[] parsedData = rawData;
                            if (isCompressed)
                            {
                                parsedData = DumpAsset.DecompressLZ4(item);
                            }
                            ParseBmapToConsole(parsedData);
                        }
                        else if (actualType == "cavs")
                        {
                            byte[] parsedData = rawData;
                            if (isCompressed)
                            {
                                parsedData = DumpAsset.DecompressLZ4(item);
                            }
                            ParseCavsToConsole(parsedData, info.GameVersion, info.Version);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogOutput($"Error analyzing {Path.GetFileName(item)}: {ex.Message}", Color.Red);
                    }
                }
            }
        }

        private void ParseBmapToConsole(byte[] data)
        {
            if (data.Length < 16) return;

            int payloadStart = -1;
            bool isBigEndian;

            // 1. Check exact start for uncompressed V2 ("bmap" - RRU / early Wreckfest)
            if (data[0] == 0x62 && data[1] == 0x6D && data[2] == 0x61 && data[3] == 0x70)
            {
                payloadStart = 4;
                isBigEndian = true;
            }
            else
            {
                // Set default before searching
                isBigEndian = false;

                // 2. Scan for "pamb" (V3 Little-Endian) in the first 256 bytes
                for (int i = 0; i < Math.Min(data.Length - 4, 256); i++)
                {
                    if (data[i] == 0x70 && data[i + 1] == 0x61 && data[i + 2] == 0x6D && data[i + 3] == 0x62)
                    {
                        payloadStart = i + 4;
                        isBigEndian = false;
                        break;
                    }
                }

                // 3. If pamb not found, scan for "bmap" starting AFTER the bag header (index > 4)
                if (payloadStart == -1)
                {
                    for (int i = 5; i < Math.Min(data.Length - 4, 256); i++)
                    {
                        if (data[i] == 0x62 && data[i + 1] == 0x6D && data[i + 2] == 0x61 && data[i + 3] == 0x70)
                        {
                            payloadStart = i + 4;
                            isBigEndian = true;
                            break;
                        }
                    }
                }
            }

            if (payloadStart == -1)
            {
                LogOutput($"Not a recognized inner BMAP format (Missing 'pamb'/'bmap' signature).", Color.Gray);
                return;
            }

            int pos = payloadStart;

            // Helper function to safely read Big-Endian integers
            int ReadInt32BE(byte[] buffer, int offset)
            {
                return (buffer[offset] << 24) | (buffer[offset + 1] << 16) | (buffer[offset + 2] << 8) | buffer[offset + 3];
            }

            int version = isBigEndian ? ReadInt32BE(data, pos) : BitConverter.ToInt32(data, pos);
            pos += 4;

            int tag = isBigEndian ? ReadInt32BE(data, pos) : BitConverter.ToInt32(data, pos);
            pos += 4;

            int pathLength = isBigEndian ? ReadInt32BE(data, pos) : BitConverter.ToInt32(data, pos);
            pos += 4;

            string tagType = tag == 0 ? "DDS" : tag == 1 ? "TGA" : "Unknown";

            if (pathLength <= 0 || pathLength > 2048 || pos + pathLength > data.Length)
            {
                LogOutput($"Invalid build path length: {pathLength}", Color.Gray);
                return;
            }

            string buildPath = System.Text.Encoding.ASCII.GetString(data, pos, pathLength);
            pos += pathLength;

            if (isBigEndian) pos += 8; // Skip "bmbd" and unknown 4 bytes
            else pos += 4; // Skip unknown 4 bytes

            LogOutput($"Format Tag: {tagType}");
            LogOutput($"Build Path: {buildPath}");

            if (tagType == "DDS")
            {
                // Scan forward slightly for "DDS " magic just in case of padding
                int ddsMagicPos = -1;
                for (int i = pos; i < Math.Min(pos + 64, data.Length - 4); i++)
                {
                    if (data[i] == 0x44 && data[i + 1] == 0x44 && data[i + 2] == 0x53 && data[i + 3] == 0x20)
                    {
                        ddsMagicPos = i;
                        break;
                    }
                }

                if (ddsMagicPos != -1)
                {
                    pos = ddsMagicPos + 12; // Skip magic (4), dwSize (4), dwFlags (4)
                    uint dwHeight = BitConverter.ToUInt32(data, pos); pos += 4;
                    uint dwWidth = BitConverter.ToUInt32(data, pos); pos += 4;
                    uint dwPitchOrLinearSize = BitConverter.ToUInt32(data, pos); pos += 8; // Skip Pitch, Depth
                    uint dwMipMapCount = BitConverter.ToUInt32(data, pos); pos += 52; // skip mip(4), reserved1(44), pfSize(4)

                    uint pfFlags = BitConverter.ToUInt32(data, pos); pos += 4;

                    uint fourCC = BitConverter.ToUInt32(data, pos);
                    string fourCCStr = fourCC == 0 ? "Uncompressed" : System.Text.Encoding.ASCII.GetString(BitConverter.GetBytes(fourCC));
                    pos += 44; // skip fourCC(4), bitCount(4), masks(16), caps(20)

                    // Helper to determine if Alpha exists based on FourCC and Flags
                    bool HasAlphaChannel(string formatStr, uint flags)
                    {
                        if ((flags & 0x1) != 0) return true;

                        string f = formatStr.ToUpper();
                        return f == "DXT2" || f == "DXT3" || f == "DXT4" || f == "DXT5" || f == "BC3" || f == "BC2" || f == "BC7";
                    }

                    bool hasDXT1Alpha = (fourCCStr == "DXT1" && (dwPitchOrLinearSize == (Math.Max(1, (dwWidth + 3) / 4) * Math.Max(1, (dwHeight + 3) / 4) * 8)));
                    bool hasAlpha = HasAlphaChannel(fourCCStr, pfFlags) || hasDXT1Alpha;

                    LogOutput($"Dimensions: {dwWidth} x {dwHeight}");
                    LogOutput($"Has Alpha: {(hasAlpha ? "Yes" : "No")}");
                    LogOutput($"Mip Levels: {dwMipMapCount}");
                    LogOutput($"FourCC / Format: {fourCCStr}");

                    if (fourCC == 0x30315844 && pos + 4 <= data.Length) // "DX10"
                    {
                        uint dxgiFormat = BitConverter.ToUInt32(data, pos);
                        LogOutput($"DXGI Format ID: {dxgiFormat}");

                        if (dxgiFormat == 98 || dxgiFormat == 99)
                        {
                            LogOutput("Note: BC7 format inherently supports Alpha.");
                        }

                        if (dxgiFormat == 95 || dxgiFormat == 96)
                            LogOutput("⚠️ HDR Notice: BC6H HDR texture detected.", Color.Yellow);
                    }
                }
                else
                {
                    LogOutput("DDS Magic header not found.", Color.Gray);
                }
            }
            else if (tagType == "TGA")
            {
                if (pos + 32 <= data.Length)
                {
                    uint rawSize = isBigEndian ? (uint)ReadInt32BE(data, pos) : BitConverter.ToUInt32(data, pos);
                    pos += 12; // skip magic/unknown

                    ushort width = BitConverter.ToUInt16(data, pos); pos += 2;
                    ushort height = BitConverter.ToUInt16(data, pos);

                    LogOutput($"Dimensions: {width} x {height}");
                    LogOutput($"Raw Size: {rawSize} bytes");
                }
            }
        }

        private void ParseCavsToConsole(byte[] data, GameVersion gameVersion, int version)
        {
            try
            {
                // --- VERSION 14 AND BELOW (Legacy Wreckfest / Stuntfest) ---
                if (version <= 14)
                {
                    int pos = 12; // Compressed bags start payload at index 12

                    // Failsafe in case it's completely uncompressed (no 0x01 marker)
                    if (data[0] != 0x01) pos = 8;

                    // 1. Vehicle Name
                    int nameLen = BitConverter.ToInt32(data, pos); pos += 4;
                    string vehicleName = System.Text.Encoding.ASCII.GetString(data, pos, nameLen); pos += nameLen;

                    // 2. State, Region, Flags
                    int state = BitConverter.ToInt32(data, pos); pos += 4;
                    int region = BitConverter.ToInt32(data, pos); pos += 4;
                    int flags = BitConverter.ToInt32(data, pos); pos += 4;

                    // 3. Ragdoll String
                    int ragdollLen = BitConverter.ToInt32(data, pos); pos += 4;
                    bool ragdollEnabled = false;

                    if (ragdollLen > 0 && ragdollLen < 1024 && pos + ragdollLen <= data.Length)
                    {
                        string ragdollStr = System.Text.Encoding.ASCII.GetString(data, pos, ragdollLen);
                        if (ragdollStr.EndsWith(".scne", StringComparison.OrdinalIgnoreCase))
                        {
                            ragdollEnabled = true;
                        }
                    }

                    // Dictionaries
                    var stateDict = new Dictionary<int, string> { { 0, "Disabled" }, { 1, "Developer" }, { 2, "Final" }, { 4, "Mod" } };
                    var regionDict = new Dictionary<int, string> { { -1, "None" }, { 0, "America" }, { 1, "Europe" }, { 2, "Asia" } };

                    string stateStr = stateDict.ContainsKey(state) ? stateDict[state] : $"Unknown ({state})";
                    string regionStr = regionDict.ContainsKey(region) ? regionDict[region] : $"Unknown ({region})";

                    // Legacy Bitwise Flags
                    List<string> activeFlags = new List<string>();
                    if ((flags & 0x01) != 0) activeFlags.Add("Restrict AI to same vehicle");
                    if ((flags & 0x02) != 0) activeFlags.Add("Allow Renting");
                    if ((flags & 0x04) != 0) activeFlags.Add("AI Vehicle");

                    string flagsStr = activeFlags.Count > 0 ? string.Join(", ", activeFlags) : "None";

                    // Output (Availability and Vehicle Size do not exist in v14 and below)
                    LogOutput($"Vehicle Name: {vehicleName}");
                    LogOutput($"State: {stateStr}");
                    LogOutput($"Region: {regionStr}");
                    LogOutput($"Flags: {flagsStr}");
                    LogOutput($"Ragdoll Enabled: {(ragdollEnabled ? "Yes" : "No")}");

                    return; // Stop here for Legacy/Stuntfest
                }

                // --- VERSION 15 AND ABOVE (Wreckfest 1 & Wreckfest 2) ---

                int FindSequence(byte[] buffer, byte[] sequence, int startIndex = 0)
                {
                    for (int i = startIndex; i < buffer.Length - sequence.Length; i++)
                    {
                        bool match = true;
                        for (int j = 0; j < sequence.Length; j++)
                        {
                            if (buffer[i + j] != sequence[j])
                            {
                                match = false;
                                break;
                            }
                        }
                        if (match) return i;
                    }
                    return -1;
                }

                // 1. Locate the VEHICLE_NAME_ string to anchor our reading
                byte[] nameSearchStr = System.Text.Encoding.ASCII.GetBytes("VEHICLE_NAME_");
                int nameIdPos = FindSequence(data, nameSearchStr);
                if (nameIdPos == -1)
                {
                    LogOutput("Could not find vehicle name anchor in .cavs file.", Color.Gray);
                    return;
                }

                // The string length is stored 4 bytes before the actual ID string
                int nameIdLen = BitConverter.ToInt32(data, nameIdPos - 4);
                int actualNamePos = nameIdPos + nameIdLen;

                int actualNameLen = BitConverter.ToInt32(data, actualNamePos);
                actualNamePos += 4;
                string vehicleName2 = System.Text.Encoding.ASCII.GetString(data, actualNamePos, actualNameLen);
                int pos2 = actualNamePos + actualNameLen;

                LogOutput($"Vehicle Name: {vehicleName2}");

                int state2, availability2, region2, size2, flags2;
                bool ragdollEnabled2 = false;

                if (gameVersion == GameVersion.Wreckfest_2)
                {
                    byte[] nartSearchStr = { 0x6E, 0x61, 0x72, 0x74 }; // "nart"
                    int nartPos = FindSequence(data, nartSearchStr, pos2);
                    if (nartPos == -1)
                    {
                        LogOutput("Could not find Wreckfest 2 'nart' anchor.", Color.Red);
                        return;
                    }

                    pos2 = nartPos + 4; // Skip "nart"

                    state2 = BitConverter.ToInt32(data, pos2); pos2 += 4;
                    pos2 += 4; // Skip unknown 4 bytes

                    // If description exists, we skip it
                    int descIdLen = BitConverter.ToInt32(data, pos2); pos2 += 4;
                    if (descIdLen > 0 && descIdLen < 1000)
                    {
                        pos2 += descIdLen; // Skip description ID string
                        int descLen = BitConverter.ToInt32(data, pos2); pos2 += 4;
                        pos2 += descLen; // Skip description string
                    }

                    availability2 = BitConverter.ToInt32(data, pos2); pos2 += 4;
                    region2 = BitConverter.ToInt32(data, pos2); pos2 += 4;
                    size2 = BitConverter.ToInt32(data, pos2); pos2 += 4;
                    flags2 = BitConverter.ToInt32(data, pos2); pos2 += 4;

                    int ragdollState = BitConverter.ToInt32(data, pos2);
                    ragdollEnabled2 = (ragdollState == 1);
                }
                else // Wreckfest 1 & Fallback
                {
                    state2 = BitConverter.ToInt32(data, pos2); pos2 += 4;
                    pos2 += 4; // Skip 00 00 00 00
                    availability2 = BitConverter.ToInt32(data, pos2); pos2 += 4;
                    region2 = BitConverter.ToInt32(data, pos2); pos2 += 4;
                    size2 = BitConverter.ToInt32(data, pos2); pos2 += 4;
                    flags2 = BitConverter.ToInt32(data, pos2); pos2 += 4;

                    if (pos2 + 4 <= data.Length)
                    {
                        int nextStringLen = BitConverter.ToInt32(data, pos2); pos2 += 4;
                        if (nextStringLen > 0 && nextStringLen < 1024 && pos2 + nextStringLen <= data.Length)
                        {
                            string nextString = System.Text.Encoding.ASCII.GetString(data, pos2, nextStringLen);
                            if (nextString.EndsWith(".scne", StringComparison.OrdinalIgnoreCase))
                            {
                                ragdollEnabled2 = true;
                            }
                        }
                    }
                }

                // Dictionaries
                var stateDict2 = new Dictionary<int, string> { { 0, "Disabled" }, { 1, "Developer" }, { 2, "Final" }, { 4, "Mod" } };
                var availDict2 = new Dictionary<int, string> { { 0, "Default" }, { 1, "Always" }, { 2, "Never" }, { 3, "Reward" } };
                var regionDict2 = new Dictionary<int, string> { { -1, "None" }, { 0, "America" }, { 1, "Europe" }, { 2, "Asia" } };
                var sizeDict2 = new Dictionary<int, string> { { -1, "None" }, { 0, "Wagon/Pickup" }, { 1, "Compact" }, { 2, "Mid Size" }, { 3, "Full Size" }, { 4, "Special" }, { 5, "Tiny" } };

                string stateStr2 = stateDict2.ContainsKey(state2) ? stateDict2[state2] : $"Unknown ({state2})";
                string availStr2 = availDict2.ContainsKey(availability2) ? availDict2[availability2] : $"Unknown ({availability2})";
                string regionStr2 = regionDict2.ContainsKey(region2) ? regionDict2[region2] : $"Unknown ({region2})";
                string sizeStr2 = sizeDict2.ContainsKey(size2) ? sizeDict2[size2] : $"Unknown ({size2})";

                // V15+ Bitwise Flags
                List<string> activeFlags2 = new List<string>();
                if ((flags2 & 0x02) != 0) activeFlags2.Add("Allow Renting");
                if ((flags2 & 0x04) != 0) activeFlags2.Add("AI Vehicle");
                if ((flags2 & 0x08) != 0) activeFlags2.Add("Custom AI Set Only");
                if ((flags2 & 0x10) != 0) activeFlags2.Add("Disable Upgrades");
                if ((flags2 & 0x20) != 0) activeFlags2.Add("Disable Tuning");
                if ((flags2 & 0x40) != 0) activeFlags2.Add("Disable Customisation");
                if ((flags2 & 0x80) != 0) activeFlags2.Add("Treat as Special for Routes");

                string flagsStr2 = activeFlags2.Count > 0 ? string.Join(", ", activeFlags2) : "None";

                // Output everything
                LogOutput($"State: {stateStr2}");
                LogOutput($"Availability: {availStr2}");
                LogOutput($"Region: {regionStr2}");
                LogOutput($"Vehicle Size: {sizeStr2}");
                LogOutput($"Flags: {flagsStr2}");
                LogOutput($"Ragdoll Enabled: {(ragdollEnabled2 ? "Yes" : "No")}");
            }
            catch (Exception ex)
            {
                LogOutput($"Error parsing CAVS data: {ex.Message}", Color.Red);
            }
        }

        private void ProgressBar_Click_1(object sender, EventArgs e)
        {

        }
    }
}