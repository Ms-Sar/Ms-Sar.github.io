using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using STRmodsWF2SuspensionEditor.Models;
using STRmodsWF2SuspensionEditor.Services;

namespace STRmodsWF2SuspensionEditor.UI
{
    public sealed class MainForm : Form
    {
        private static readonly Color BackColorMain =
            Color.FromArgb(30, 33, 39);

        private static readonly Color BackColorHeader =
            Color.FromArgb(39, 43, 51);

        private static readonly Color BackColorPanel =
            Color.FromArgb(45, 49, 58);

        private static readonly Color BackColorGroup =
            Color.FromArgb(48, 53, 63);

        private static readonly Color BackColorVector =
            Color.FromArgb(58, 63, 75);

        private static readonly Color BackColorTextBox =
            Color.FromArgb(26, 29, 35);

        private static readonly Color AccentColor =
            Color.FromArgb(75, 169, 225);

        private static readonly Color AccentHoverColor =
            Color.FromArgb(97, 186, 238);

        private static readonly Color AccentSelectedColor =
            Color.FromArgb(58, 148, 205);

        private static readonly Color GreenButtonColor =
            Color.FromArgb(63, 151, 103);

        private static readonly Color GreenHoverColor =
            Color.FromArgb(78, 176, 120);

        private static readonly Color ImportButtonColor =
            Color.FromArgb(83, 130, 169);

        private static readonly Color ImportHoverColor =
            Color.FromArgb(100, 151, 193);

        private static readonly Color TextColor =
            Color.FromArgb(238, 241, 245);

        private static readonly Color MutedTextColor =
            Color.FromArgb(174, 184, 196);

        private static readonly Color SuccessColor =
            Color.FromArgb(107, 202, 127);

        private static readonly Color ErrorColor =
            Color.FromArgb(245, 107, 107);

        private readonly SuspensionFileService _suspensionFileService;
        private readonly SuspensionGeometryFileService _geometryFileService;

        private readonly Dictionary<int, TextBox> _frontSuspensionBoxes;
        private readonly Dictionary<int, TextBox> _rearSuspensionBoxes;
        private readonly Dictionary<int, TextBox> _frontGeometryBoxes;
        private readonly Dictionary<int, TextBox> _rearGeometryBoxes;

        private Panel _suspensionPage;
        private Panel _geometryPage;

        private Button _suspensionNavigationButton;
        private Button _geometryNavigationButton;

        private Label _suspensionLoadedFileLabel;
        private Label _suspensionStatusLabel;
        private Button _importSuspensionButton;
        private Button _saveSuspensionButton;

        private Label _geometryLoadedFileLabel;
        private Label _geometryStatusLabel;
        private Button _importGeometryButton;
        private Button _saveGeometryButton;

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(
            IntPtr hWnd,
            string appName,
            string idList);

        public MainForm()
        {
            _suspensionFileService = new SuspensionFileService();
            _geometryFileService = new SuspensionGeometryFileService();

            _frontSuspensionBoxes =
                new Dictionary<int, TextBox>();

            _rearSuspensionBoxes =
                new Dictionary<int, TextBox>();

            _frontGeometryBoxes =
                new Dictionary<int, TextBox>();

            _rearGeometryBoxes =
                new Dictionary<int, TextBox>();

            BuildUi();

            SetSuspensionStatus(
                "Open a Wreckfest 2 .upgr file to begin.",
                false);

            SetGeometryStatus(
                "Open a Wreckfest 2 .vesg file to begin.",
                false);
        }

        private void BuildUi()
        {
            Text = "STRmods - WF2 Suspension Editor";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 1200;
            Height = 900;
            MinimumSize = new Size(1000, 720);
            BackColor = BackColorMain;
            ForeColor = TextColor;
            Font = new Font("Segoe UI", 9.0f, FontStyle.Regular);

            Panel navigationPanel = new Panel();
            navigationPanel.Dock = DockStyle.Top;
            navigationPanel.Height = 45;
            navigationPanel.BackColor = BackColorHeader;
            navigationPanel.Padding = new Padding(12, 7, 12, 5);

            _suspensionNavigationButton = CreateNavigationButton(
                "Vehicle Suspension Settings");

            _geometryNavigationButton = CreateNavigationButton(
                "Vehicle Suspension Geometry");

            _suspensionNavigationButton.Click += delegate
            {
                ShowSuspensionPage();
            };

            _geometryNavigationButton.Click += delegate
            {
                ShowGeometryPage();
            };

            navigationPanel.Controls.Add(_geometryNavigationButton);
            navigationPanel.Controls.Add(_suspensionNavigationButton);

            Panel pageContainer = new Panel();
            pageContainer.Dock = DockStyle.Fill;
            pageContainer.BackColor = BackColorMain;

            _suspensionPage = new Panel();
            _suspensionPage.Dock = DockStyle.Fill;
            _suspensionPage.BackColor = BackColorMain;

            _geometryPage = new Panel();
            _geometryPage.Dock = DockStyle.Fill;
            _geometryPage.BackColor = BackColorMain;

            BuildSuspensionPage(_suspensionPage);
            BuildGeometryPage(_geometryPage);

            pageContainer.Controls.Add(_geometryPage);
            pageContainer.Controls.Add(_suspensionPage);

            Controls.Add(pageContainer);
            Controls.Add(navigationPanel);

            ShowSuspensionPage();
        }

        private Button CreateNavigationButton(string text)
        {
            Button button = new Button();
            button.Text = text;
            button.Dock = DockStyle.Left;
            button.Width = 230;
            button.Height = 33;
            button.Margin = new Padding(0, 0, 6, 0);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = BackColorHeader;
            button.ForeColor = MutedTextColor;
            button.Font = new Font(
                "Segoe UI",
                9.0f,
                FontStyle.Bold);

            button.Cursor = Cursors.Hand;

            button.MouseEnter += delegate
            {
                if (button.BackColor != AccentSelectedColor)
                    button.BackColor = BackColorPanel;
            };

            button.MouseLeave += delegate
            {
                if (button.BackColor != AccentSelectedColor)
                    button.BackColor = BackColorHeader;
            };

            return button;
        }

        private void ShowSuspensionPage()
        {
            _suspensionPage.BringToFront();
            _suspensionPage.Visible = true;
            _geometryPage.Visible = false;

            SetNavigationSelection(
                _suspensionNavigationButton,
                _geometryNavigationButton);
        }

        private void ShowGeometryPage()
        {
            _geometryPage.BringToFront();
            _geometryPage.Visible = true;
            _suspensionPage.Visible = false;

            SetNavigationSelection(
                _geometryNavigationButton,
                _suspensionNavigationButton);
        }

        private static void SetNavigationSelection(
            Button selectedButton,
            Button otherButton)
        {
            selectedButton.BackColor = AccentSelectedColor;
            selectedButton.ForeColor = Color.White;

            otherButton.BackColor = BackColorHeader;
            otherButton.ForeColor = MutedTextColor;
        }

        private void BuildSuspensionPage(Panel page)
        {
            Panel header = CreateHeaderPanel(
                "Vehicle Suspension Settings",
                "Edit Wreckfest 2 .upgr suspension values or import values from a Wreckfest 1 .vesu file.");

            FlowLayoutPanel actions = CreateActionPanel();

            Button openButton = CreateActionButton(
                "Open WF2 .UPGR",
                AccentColor,
                AccentHoverColor);

            openButton.Click += OpenSuspensionButton_Click;

            _importSuspensionButton = CreateActionButton(
                "Import WF1 .VESU Values",
                ImportButtonColor,
                ImportHoverColor);

            _importSuspensionButton.Enabled = false;
            _importSuspensionButton.Click += ImportSuspensionButton_Click;

            _saveSuspensionButton = CreateActionButton(
                "Save WF2 .UPGR As...",
                GreenButtonColor,
                GreenHoverColor);

            _saveSuspensionButton.Enabled = false;
            _saveSuspensionButton.Click += SaveSuspensionButton_Click;

            _suspensionLoadedFileLabel = CreateFileLabel(
                "No WF2 .upgr file loaded.");

            actions.Controls.Add(openButton);
            actions.Controls.Add(_importSuspensionButton);
            actions.Controls.Add(_saveSuspensionButton);
            actions.Controls.Add(_suspensionLoadedFileLabel);

            TableLayoutPanel content = CreateTwoColumnPanel();

            content.Controls.Add(
                CreateSuspensionGroup(
                    "Front Suspension",
                    _frontSuspensionBoxes),
                0,
                0);

            content.Controls.Add(
                CreateSuspensionGroup(
                    "Rear Suspension",
                    _rearSuspensionBoxes),
                1,
                0);

            Panel status = CreateStatusPanel(out _suspensionStatusLabel);

            page.Controls.Add(content);
            page.Controls.Add(status);
            page.Controls.Add(actions);
            page.Controls.Add(header);
        }

        private void BuildGeometryPage(Panel page)
        {
            Panel header = CreateHeaderPanel(
                "Vehicle Suspension Geometry",
                "Edit front and rear suspension mounting points in a Wreckfest 2 .vesg file.");

            FlowLayoutPanel actions = CreateActionPanel();

            Button openButton = CreateActionButton(
                "Open WF2 .VESG",
                AccentColor,
                AccentHoverColor);

            openButton.Click += OpenGeometryButton_Click;

            _importGeometryButton = CreateActionButton(
                "Import WF1 .VESU Values",
                ImportButtonColor,
                ImportHoverColor);

            _importGeometryButton.Enabled = false;
            _importGeometryButton.Click += ImportGeometryButton_Click;

            _saveGeometryButton = CreateActionButton(
                "Save WF2 .VESG As...",
                GreenButtonColor,
                GreenHoverColor);

            _saveGeometryButton.Enabled = false;
            _saveGeometryButton.Click += SaveGeometryButton_Click;

            _geometryLoadedFileLabel = CreateFileLabel(
                "No WF2 .vesg file loaded.");

            actions.Controls.Add(openButton);
            actions.Controls.Add(_importGeometryButton);
            actions.Controls.Add(_saveGeometryButton);
            actions.Controls.Add(_geometryLoadedFileLabel);

            TableLayoutPanel content = CreateTwoColumnPanel();

            content.Controls.Add(
                CreateGeometryGroup(
                    "Front Geometry",
                    _frontGeometryBoxes),
                0,
                0);

            content.Controls.Add(
                CreateGeometryGroup(
                    "Rear Geometry",
                    _rearGeometryBoxes),
                1,
                0);

            Panel status = CreateStatusPanel(out _geometryStatusLabel);

            page.Controls.Add(content);
            page.Controls.Add(status);
            page.Controls.Add(actions);
            page.Controls.Add(header);
        }

        private static Panel CreateHeaderPanel(
            string title,
            string description)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Top;
            panel.Height = 72;
            panel.BackColor = BackColorHeader;
            panel.Padding = new Padding(20, 11, 20, 8);

            Label titleLabel = new Label();
            titleLabel.Text = title;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 29;
            titleLabel.ForeColor = Color.White;
            titleLabel.Font = new Font(
                "Segoe UI",
                14.0f,
                FontStyle.Bold);

            Label descriptionLabel = new Label();
            descriptionLabel.Text = description;
            descriptionLabel.Dock = DockStyle.Bottom;
            descriptionLabel.Height = 21;
            descriptionLabel.ForeColor = MutedTextColor;
            descriptionLabel.Font = new Font(
                "Segoe UI",
                9.0f,
                FontStyle.Regular);

            panel.Controls.Add(descriptionLabel);
            panel.Controls.Add(titleLabel);

            return panel;
        }

        private static FlowLayoutPanel CreateActionPanel()
        {
            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Top;
            panel.Height = 60;
            panel.BackColor = BackColorMain;
            panel.Padding = new Padding(20, 13, 20, 8);
            panel.WrapContents = false;
            panel.AutoScroll = true;

            ApplyDarkScrollTheme(panel);

            return panel;
        }

        private static Button CreateActionButton(
            string text,
            Color normalColor,
            Color hoverColor)
        {
            Button button = new Button();
            button.Text = text;
            button.AutoSize = false;
            button.Width = 160;
            button.Height = 31;
            button.Margin = new Padding(0, 0, 8, 0);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = normalColor;
            button.ForeColor = Color.White;
            button.Font = new Font(
                "Segoe UI",
                9.0f,
                FontStyle.Bold);

            button.Cursor = Cursors.Hand;

            button.MouseEnter += delegate
            {
                if (button.Enabled)
                    button.BackColor = hoverColor;
            };

            button.MouseLeave += delegate
            {
                if (button.Enabled)
                    button.BackColor = normalColor;
            };

            button.EnabledChanged += delegate
            {
                if (!button.Enabled)
                {
                    button.BackColor = Color.FromArgb(67, 71, 79);
                    button.ForeColor = Color.FromArgb(145, 150, 158);
                }
                else
                {
                    button.BackColor = normalColor;
                    button.ForeColor = Color.White;
                }
            };

            return button;
        }

        private static Label CreateFileLabel(string text)
        {
            Label label = new Label();
            label.Text = text;
            label.Width = 610;
            label.Height = 31;
            label.Padding = new Padding(10, 7, 0, 0);
            label.ForeColor = MutedTextColor;
            label.AutoEllipsis = true;
            label.Font = new Font(
                "Segoe UI",
                8.8f,
                FontStyle.Italic);

            return label;
        }

        private static TableLayoutPanel CreateTwoColumnPanel()
        {
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.ColumnCount = 2;
            panel.RowCount = 1;
            panel.Padding = new Padding(20, 10, 20, 13);
            panel.BackColor = BackColorMain;

            panel.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    50.0f));

            panel.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    50.0f));

            return panel;
        }

        private static Panel CreateStatusPanel(out Label statusLabel)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Bottom;
            panel.Height = 37;
            panel.BackColor = BackColorHeader;
            panel.Padding = new Padding(20, 8, 20, 0);

            statusLabel = new Label();
            statusLabel.Dock = DockStyle.Fill;
            statusLabel.AutoEllipsis = true;
            statusLabel.Font = new Font(
                "Segoe UI",
                8.8f,
                FontStyle.Regular);

            panel.Controls.Add(statusLabel);

            return panel;
        }

        private static GroupBox CreateSuspensionGroup(
            string title,
            Dictionary<int, TextBox> boxMap)
        {
            GroupBox group = CreateStyledGroup(title);
            Panel scroll = CreateScrollPanel();
            TableLayoutPanel fields = CreateFieldsPanel();

            AddSingleValueRow(fields, "Ride Height", 0, boxMap);
            AddSingleValueRow(fields, "Bump Stop Up", 1, boxMap);
            AddSingleValueRow(fields, "Bump Stop Down", 2, boxMap);
            AddSingleValueRow(fields, "Spring Rate", 3, boxMap);
            AddSingleValueRow(fields, "Progressive Rate", 4, boxMap);

            AddSingleValueRow(fields, "???", 5, boxMap);

            AddSingleValueRow(fields, "Bump Stop Length", 6, boxMap);
            AddSingleValueRow(fields, "Bump Stop Rate", 7, boxMap);
            AddSingleValueRow(fields, "Bump Stop Damp", 8, boxMap);

            AddSingleValueRow(
                fields,
                "Bump Stop Rate Gain (Deflection Squared)",
                9,
                boxMap);

            AddSingleValueRow(
                fields,
                "Bump Stop Damp Gain (Deflection Squared)",
                10,
                boxMap);

            AddSingleValueRow(fields, "Rebound Length", 11, boxMap);
            AddSingleValueRow(fields, "Rebound Rate", 12, boxMap);

            AddVectorRow(
                fields,
                "Bump Limits",
                new[] { "X", "Y" },
                new[] { 13, 14 },
                boxMap);

            AddVectorRow(
                fields,
                "Bump Damp",
                new[] { "X", "Y" },
                new[] { 15, 16 },
                boxMap);

            AddVectorRow(
                fields,
                "Rebound Limits",
                new[] { "X", "Y" },
                new[] { 17, 18 },
                boxMap);

            AddVectorRow(
                fields,
                "Rebound Damp",
                new[] { "X", "Y" },
                new[] { 19, 20 },
                boxMap);

            AddSingleValueRow(fields, "Rollbar Stiffness", 21, boxMap);
            AddSingleValueRow(fields, "Camber Angle (Degrees)", 22, boxMap);

            scroll.Controls.Add(fields);
            group.Controls.Add(scroll);

            return group;
        }

        private static GroupBox CreateGeometryGroup(
            string title,
            Dictionary<int, TextBox> boxMap)
        {
            GroupBox group = CreateStyledGroup(title);
            Panel scroll = CreateScrollPanel();
            TableLayoutPanel fields = CreateFieldsPanel();

            AddVectorRow(
                fields,
                "Spindle Upper Arm",
                new[] { "X", "Y", "Z" },
                new[] { 0, 1, 2 },
                boxMap);

            AddVectorRow(
                fields,
                "Body Upper Front Arm",
                new[] { "X", "Y", "Z" },
                new[] { 3, 4, 5 },
                boxMap);

            AddVectorRow(
                fields,
                "Body Upper Rear Arm",
                new[] { "X", "Y", "Z" },
                new[] { 6, 7, 8 },
                boxMap);

            AddVectorRow(
                fields,
                "Spindle Lower Arm",
                new[] { "X", "Y", "Z" },
                new[] { 9, 10, 11 },
                boxMap);

            AddVectorRow(
                fields,
                "Body Lower Front Arm",
                new[] { "X", "Y", "Z" },
                new[] { 12, 13, 14 },
                boxMap);

            AddVectorRow(
                fields,
                "Body Lower Rear Arm",
                new[] { "X", "Y", "Z" },
                new[] { 15, 16, 17 },
                boxMap);

            AddVectorRow(
                fields,
                "Spindle Steering Rod",
                new[] { "X", "Y", "Z" },
                new[] { 18, 19, 20 },
                boxMap);

            AddVectorRow(
                fields,
                "Body Steering Rod",
                new[] { "X", "Y", "Z" },
                new[] { 21, 22, 23 },
                boxMap);

            AddVectorRow(
                fields,
                "Spindle Push Rod",
                new[] { "X", "Y", "Z" },
                new[] { 24, 25, 26 },
                boxMap);

            AddVectorRow(
                fields,
                "Body Push Rod",
                new[] { "X", "Y", "Z" },
                new[] { 27, 28, 29 },
                boxMap);

            AddSingleValueRow(
                fields,
                "Body Y-Offset",
                30,
                boxMap);

            scroll.Controls.Add(fields);
            group.Controls.Add(scroll);

            return group;
        }

        private static GroupBox CreateStyledGroup(string title)
        {
            GroupBox group = new GroupBox();
            group.Text = "  " + title + "  ";
            group.Dock = DockStyle.Fill;
            group.Padding = new Padding(12, 25, 12, 12);
            group.Margin = new Padding(0, 0, 8, 0);
            group.BackColor = BackColorGroup;
            group.ForeColor = AccentColor;
            group.Font = new Font(
                "Segoe UI",
                10.0f,
                FontStyle.Bold);

            return group;
        }

        private static Panel CreateScrollPanel()
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.AutoScroll = true;
            panel.BackColor = BackColorGroup;

            ApplyDarkScrollTheme(panel);

            return panel;
        }

        private static void ApplyDarkScrollTheme(Control control)
        {
            control.HandleCreated += delegate
            {
                try
                {
                    SetWindowTheme(
                        control.Handle,
                        "DarkMode_Explorer",
                        null);
                }
                catch
                {
                }
            };
        }

        private static TableLayoutPanel CreateFieldsPanel()
        {
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Top;
            panel.AutoSize = true;
            panel.ColumnCount = 1;
            panel.BackColor = BackColorGroup;
            panel.Padding = new Padding(2, 2, 2, 7);

            return panel;
        }

        private static void AddSingleValueRow(
            TableLayoutPanel parent,
            string fieldName,
            int fieldIndex,
            Dictionary<int, TextBox> boxMap)
        {
            TableLayoutPanel row = new TableLayoutPanel();
            row.Dock = DockStyle.Top;
            row.AutoSize = true;
            row.ColumnCount = 2;
            row.Margin = new Padding(3, 2, 3, 3);
            row.Padding = new Padding(8, 3, 8, 3);
            row.BackColor = BackColorPanel;

            row.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    68.0f));

            row.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    32.0f));

            Label label = CreateFieldLabel(fieldName);
            TextBox valueBox = CreateValueBox();

            row.Controls.Add(label, 0, 0);
            row.Controls.Add(valueBox, 1, 0);

            parent.Controls.Add(row, 0, parent.RowCount);
            parent.RowCount++;

            boxMap.Add(fieldIndex, valueBox);
        }

        private static void AddVectorRow(
            TableLayoutPanel parent,
            string fieldName,
            string[] axisNames,
            int[] fieldIndexes,
            Dictionary<int, TextBox> boxMap)
        {
            if (axisNames == null ||
                fieldIndexes == null ||
                axisNames.Length != fieldIndexes.Length)
            {
                throw new ArgumentException(
                    "Vector axis names and indexes must have matching lengths.");
            }

            Panel container = new Panel();
            container.Dock = DockStyle.Top;
            container.AutoSize = true;
            container.Margin = new Padding(3, 4, 3, 4);
            container.Padding = new Padding(9, 5, 9, 7);
            container.BackColor = BackColorVector;

            Label nameLabel = CreateFieldLabel(fieldName);
            nameLabel.Dock = DockStyle.Top;
            nameLabel.Height = 24;

            TableLayoutPanel values = new TableLayoutPanel();
            values.Dock = DockStyle.Top;
            values.AutoSize = true;
            values.ColumnCount = axisNames.Length * 2;

            for (int i = 0; i < axisNames.Length; i++)
            {
                values.ColumnStyles.Add(
                    new ColumnStyle(SizeType.AutoSize));

                values.ColumnStyles.Add(
                    new ColumnStyle(
                        SizeType.Percent,
                        100.0f / axisNames.Length));

                Label axisLabel = new Label();
                axisLabel.Text = axisNames[i];
                axisLabel.AutoSize = true;
                axisLabel.ForeColor = AccentColor;
                axisLabel.Font = new Font(
                    "Segoe UI",
                    9.0f,
                    FontStyle.Bold);

                axisLabel.Anchor = AnchorStyles.Left;
                axisLabel.Padding = new Padding(
                    i == 0 ? 0 : 11,
                    5,
                    5,
                    0);

                TextBox valueBox = CreateValueBox();

                values.Controls.Add(axisLabel, i * 2, 0);
                values.Controls.Add(valueBox, (i * 2) + 1, 0);

                boxMap.Add(fieldIndexes[i], valueBox);
            }

            container.Controls.Add(values);
            container.Controls.Add(nameLabel);

            parent.Controls.Add(container, 0, parent.RowCount);
            parent.RowCount++;
        }

        private static Label CreateFieldLabel(string text)
        {
            Label label = new Label();
            label.Text = text;
            label.AutoSize = true;
            label.Anchor = AnchorStyles.Left;
            label.ForeColor = TextColor;
            label.Font = new Font(
                "Segoe UI",
                9.0f,
                FontStyle.Bold);

            label.Padding = new Padding(0, 4, 5, 2);

            return label;
        }

        private static TextBox CreateValueBox()
        {
            TextBox box = new TextBox();
            box.Width = 85;
            box.Height = 24;
            box.TextAlign = HorizontalAlignment.Right;
            box.BorderStyle = BorderStyle.FixedSingle;
            box.BackColor = BackColorTextBox;
            box.ForeColor = Color.White;
            box.Font = new Font(
                "Consolas",
                9.5f,
                FontStyle.Regular);

            box.Enter += delegate
            {
                box.BackColor = Color.FromArgb(37, 51, 64);
            };

            box.Leave += delegate
            {
                box.BackColor = BackColorTextBox;
            };

            return box;
        }

        private void OpenSuspensionButton_Click(
            object sender,
            EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Open Wreckfest 2 Suspension Upgrade";
                dialog.Filter =
                    "Wreckfest 2 Upgrade (*.upgr)|*.upgr|All files (*.*)|*.*";

                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    SuspensionData data =
                        _suspensionFileService.LoadWf2Upgrade(
                            dialog.FileName);

                    PopulateSuspensionFields(data, false);

                    _suspensionLoadedFileLabel.Text =
                        "Loaded: " + dialog.FileName;

                    _importSuspensionButton.Enabled = true;
                    _saveSuspensionButton.Enabled = true;

                    SetSuspensionStatus(
                        "WF2 .upgr loaded successfully.",
                        false);
                }
                catch (Exception ex)
                {
                    SetSuspensionStatus(
                        "Failed to open WF2 .upgr.",
                        true);

                    ShowError("Open failed", ex.Message);
                }
            }
        }

        private void ImportSuspensionButton_Click(
            object sender,
            EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Import Wreckfest 1 Suspension";
                dialog.Filter =
                    "Wreckfest 1 Suspension (*.vesu)|*.vesu|All files (*.*)|*.*";

                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    SuspensionData data =
                        _suspensionFileService.LoadWf1Vesu(
                            dialog.FileName);

                    /*
                     * true = preserve the WF2-only ??? field because
                     * that field does not exist in the WF1 .vesu layout.
                     */
                    PopulateSuspensionFields(data, true);

                    SetSuspensionStatus(
                        "Imported WF1 values. The WF2-only ??? values were preserved.",
                        false);
                }
                catch (Exception ex)
                {
                    SetSuspensionStatus(
                        "WF1 suspension import failed.",
                        true);

                    ShowError("Import failed", ex.Message);
                }
            }
        }

        private void SaveSuspensionButton_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                SuspensionData data = ReadSuspensionFields();

                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Title =
                        "Save Wreckfest 2 Suspension Upgrade";

                    dialog.Filter =
                        "Wreckfest 2 Upgrade (*.upgr)|*.upgr|All files (*.*)|*.*";

                    dialog.DefaultExt = "upgr";
                    dialog.AddExtension = true;

                    ConfigureSaveDialog(
                        dialog,
                        _suspensionFileService.CurrentWf2Path,
                        "_edited.upgr");

                    if (dialog.ShowDialog(this) != DialogResult.OK)
                        return;

                    _suspensionFileService.SaveWf2Upgrade(
                        dialog.FileName,
                        data);

                    _suspensionLoadedFileLabel.Text =
                        "Saved: " + dialog.FileName;

                    SetSuspensionStatus(
                        "Saved as an uncompressed type-01 .upgr file.",
                        false);
                }
            }
            catch (Exception ex)
            {
                SetSuspensionStatus("Save failed.", true);
                ShowError("Save failed", ex.Message);
            }
        }

        private void OpenGeometryButton_Click(
            object sender,
            EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Open Wreckfest 2 Suspension Geometry";
                dialog.Filter =
                    "Wreckfest 2 Geometry (*.vesg)|*.vesg|All files (*.*)|*.*";

                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    SuspensionGeometryData data =
                        _geometryFileService.LoadWf2Geometry(
                            dialog.FileName);

                    PopulateGeometryFields(data);

                    _geometryLoadedFileLabel.Text =
                        "Loaded: " + dialog.FileName;

                    _importGeometryButton.Enabled = true;
                    _saveGeometryButton.Enabled = true;

                    SetGeometryStatus(
                        "WF2 .vesg loaded successfully.",
                        false);
                }
                catch (Exception ex)
                {
                    SetGeometryStatus(
                        "Failed to open WF2 .vesg.",
                        true);

                    ShowError("Open failed", ex.Message);
                }
            }
        }

        private void ImportGeometryButton_Click(
            object sender,
            EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title =
                    "Import Wreckfest 1 Suspension Geometry";

                dialog.Filter =
                    "Wreckfest 1 Suspension (*.vesu)|*.vesu|All files (*.*)|*.*";

                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    SuspensionGeometryData data =
                        _geometryFileService.LoadWf1GeometryFromVesu(
                            dialog.FileName);

                    PopulateGeometryFields(data);

                    SetGeometryStatus(
                        "Imported front and rear geometry values from WF1 .vesu.",
                        false);
                }
                catch (Exception ex)
                {
                    SetGeometryStatus(
                        "WF1 geometry import failed.",
                        true);

                    ShowError("Import failed", ex.Message);
                }
            }
        }

        private void SaveGeometryButton_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                SuspensionGeometryData data = ReadGeometryFields();

                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Title =
                        "Save Wreckfest 2 Suspension Geometry";

                    dialog.Filter =
                        "Wreckfest 2 Geometry (*.vesg)|*.vesg|All files (*.*)|*.*";

                    dialog.DefaultExt = "vesg";
                    dialog.AddExtension = true;

                    ConfigureSaveDialog(
                        dialog,
                        _geometryFileService.CurrentWf2Path,
                        "_edited.vesg");

                    if (dialog.ShowDialog(this) != DialogResult.OK)
                        return;

                    _geometryFileService.SaveWf2Geometry(
                        dialog.FileName,
                        data);

                    _geometryLoadedFileLabel.Text =
                        "Saved: " + dialog.FileName;

                    SetGeometryStatus(
                        "Saved as an uncompressed type-01 .vesg file.",
                        false);
                }
            }
            catch (Exception ex)
            {
                SetGeometryStatus("Save failed.", true);
                ShowError("Save failed", ex.Message);
            }
        }

        private void PopulateSuspensionFields(
            SuspensionData data,
            bool preserveWf2OnlyField)
        {
            foreach (SuspensionField field in SuspensionMapper.Fields)
            {
                float frontValue =
                    data.FrontValues[field.FloatIndex];

                float rearValue =
                    data.RearValues[field.FloatIndex];

                if (preserveWf2OnlyField &&
                    field.FloatIndex ==
                    SuspensionMapper.Wf2UnknownFieldIndex)
                {
                    continue;
                }

                if (float.IsNaN(frontValue) ||
                    float.IsNaN(rearValue))
                {
                    continue;
                }

                float frontDisplay =
                    SuspensionMapper.ToDisplayValue(
                        field,
                        frontValue);

                float rearDisplay =
                    SuspensionMapper.ToDisplayValue(
                        field,
                        rearValue);

                _frontSuspensionBoxes[field.FloatIndex].Text =
                    FormatFloat(frontDisplay);

                _rearSuspensionBoxes[field.FloatIndex].Text =
                    FormatFloat(rearDisplay);
            }
        }

        private SuspensionData ReadSuspensionFields()
        {
            float[] front =
                new float[SuspensionMapper.Wf2FloatCountPerAxle];

            float[] rear =
                new float[SuspensionMapper.Wf2FloatCountPerAxle];

            foreach (SuspensionField field in SuspensionMapper.Fields)
            {
                float frontDisplay = ReadFloatValue(
                    _frontSuspensionBoxes[field.FloatIndex],
                    "Front Suspension: " + field.Name);

                float rearDisplay = ReadFloatValue(
                    _rearSuspensionBoxes[field.FloatIndex],
                    "Rear Suspension: " + field.Name);

                front[field.FloatIndex] =
                    SuspensionMapper.ToStoredValue(
                        field,
                        frontDisplay);

                rear[field.FloatIndex] =
                    SuspensionMapper.ToStoredValue(
                        field,
                        rearDisplay);
            }

            return new SuspensionData(front, rear);
        }

        private void PopulateGeometryFields(
            SuspensionGeometryData data)
        {
            foreach (SuspensionField field in
                SuspensionGeometryMapper.Fields)
            {
                _frontGeometryBoxes[field.FloatIndex].Text =
                    FormatFloat(data.FrontValues[field.FloatIndex]);

                _rearGeometryBoxes[field.FloatIndex].Text =
                    FormatFloat(data.RearValues[field.FloatIndex]);
            }
        }

        private SuspensionGeometryData ReadGeometryFields()
        {
            float[] front =
                new float[
                    SuspensionGeometryMapper.FloatCountPerAxle];

            float[] rear =
                new float[
                    SuspensionGeometryMapper.FloatCountPerAxle];

            foreach (SuspensionField field in
                SuspensionGeometryMapper.Fields)
            {
                front[field.FloatIndex] = ReadFloatValue(
                    _frontGeometryBoxes[field.FloatIndex],
                    "Front Geometry: " + field.Name);

                rear[field.FloatIndex] = ReadFloatValue(
                    _rearGeometryBoxes[field.FloatIndex],
                    "Rear Geometry: " + field.Name);
            }

            return new SuspensionGeometryData(front, rear);
        }

        private static float ReadFloatValue(
            TextBox box,
            string fieldName)
        {
            float value;

            bool valid =
                float.TryParse(
                    box.Text,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out value)
                ||
                float.TryParse(
                    box.Text,
                    NumberStyles.Float,
                    CultureInfo.CurrentCulture,
                    out value);

            if (!valid)
            {
                throw new Exception(
                    "\"" + fieldName +
                    "\" is not a valid number.");
            }

            return value;
        }

        private static string FormatFloat(float value)
        {
            return value.ToString(
                "0.########",
                CultureInfo.InvariantCulture);
        }

        private static void ConfigureSaveDialog(
            SaveFileDialog dialog,
            string currentPath,
            string suffix)
        {
            if (string.IsNullOrWhiteSpace(currentPath))
                return;

            dialog.InitialDirectory =
                Path.GetDirectoryName(currentPath);

            dialog.FileName =
                Path.GetFileNameWithoutExtension(currentPath) +
                suffix;
        }

        private void SetSuspensionStatus(
            string message,
            bool isError)
        {
            _suspensionStatusLabel.Text = message;

            _suspensionStatusLabel.ForeColor =
                isError ? ErrorColor : SuccessColor;
        }

        private void SetGeometryStatus(
            string message,
            bool isError)
        {
            _geometryStatusLabel.Text = message;

            _geometryStatusLabel.ForeColor =
                isError ? ErrorColor : SuccessColor;
        }

        private void ShowError(string title, string message)
        {
            MessageBox.Show(
                this,
                message,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}