namespace Blitz1.Client
{
    partial class FControl
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
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.Windows.Forms.ColumnHeader columnHeader2;
            System.Windows.Forms.ColumnHeader mcolName;
            System.Windows.Forms.ColumnHeader mcolNumber;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FControl));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Name",
            "1",
            "Name"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Name",
            "1",
            "Name"}, -1);
            this.mbutClear = new System.Windows.Forms.Button();
            this.mbutFill = new System.Windows.Forms.Button();
            this.mbutCancel = new System.Windows.Forms.Button();
            this.mlblTotal1 = new System.Windows.Forms.Label();
            this.mlblList1 = new System.Windows.Forms.Label();
            this.mmnuListview = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mitmSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mitmAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.mitmChange = new System.Windows.Forms.ToolStripMenuItem();
            this.mitmDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mitmCut = new System.Windows.Forms.ToolStripMenuItem();
            this.mitmCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mitmPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.mrbtnTournament = new System.Windows.Forms.RadioButton();
            this.mrbtnCommands = new System.Windows.Forms.RadioButton();
            this.mlblTotal2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mrbtnLeagueGroup = new System.Windows.Forms.RadioButton();
            this.mrbtnOneToOne = new System.Windows.Forms.RadioButton();
            this.mupdWinPoints = new System.Windows.Forms.NumericUpDown();
            this.mupdDrawPoints = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.mlblList2 = new System.Windows.Forms.Label();
            this.mlstNames2 = new Blitz1.UCListView();
            this.mlstNames1 = new Blitz1.UCListView();
            this.mtxtName1 = new System.Windows.Forms.TextBox();
            this.mtxtName2 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.mupdMatchesInPair = new System.Windows.Forms.NumericUpDown();
            this.mlblMatchesInPair = new System.Windows.Forms.Label();
            columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            mcolName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            mcolNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mmnuListview.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mupdWinPoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mupdDrawPoints)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mupdMatchesInPair)).BeginInit();
            this.SuspendLayout();
            // 
            // columnHeader1
            // 
            columnHeader1.DisplayIndex = 1;
            columnHeader1.Width = 38;
            // 
            // columnHeader2
            // 
            columnHeader2.DisplayIndex = 0;
            columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            columnHeader2.Width = 30;
            // 
            // mcolName
            // 
            mcolName.DisplayIndex = 1;
            mcolName.Width = 38;
            // 
            // mcolNumber
            // 
            mcolNumber.DisplayIndex = 0;
            mcolNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            mcolNumber.Width = 30;
            // 
            // mbutClear
            // 
            this.mbutClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbutClear.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.mbutClear.Location = new System.Drawing.Point(464, 74);
            this.mbutClear.Name = "mbutClear";
            this.mbutClear.Size = new System.Drawing.Size(77, 25);
            this.mbutClear.TabIndex = 12;
            this.mbutClear.Text = "Очистить";
            this.mbutClear.UseVisualStyleBackColor = true;
            this.mbutClear.Visible = false;
            this.mbutClear.Click += new System.EventHandler(this.mbutClear_Click);
            // 
            // mbutFill
            // 
            this.mbutFill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbutFill.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.mbutFill.Enabled = false;
            this.mbutFill.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.mbutFill.Location = new System.Drawing.Point(464, 12);
            this.mbutFill.Name = "mbutFill";
            this.mbutFill.Size = new System.Drawing.Size(77, 25);
            this.mbutFill.TabIndex = 10;
            this.mbutFill.Text = "Заполнить";
            this.mbutFill.UseVisualStyleBackColor = true;
            this.mbutFill.Click += new System.EventHandler(this.mbutFill_Click);
            // 
            // mbutCancel
            // 
            this.mbutCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbutCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.mbutCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.mbutCancel.Location = new System.Drawing.Point(464, 43);
            this.mbutCancel.Name = "mbutCancel";
            this.mbutCancel.Size = new System.Drawing.Size(77, 25);
            this.mbutCancel.TabIndex = 11;
            this.mbutCancel.Text = "Отменить";
            this.mbutCancel.UseVisualStyleBackColor = true;
            // 
            // mlblTotal1
            // 
            this.mlblTotal1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mlblTotal1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mlblTotal1.Location = new System.Drawing.Point(9, 443);
            this.mlblTotal1.Name = "mlblTotal1";
            this.mlblTotal1.Size = new System.Drawing.Size(30, 16);
            this.mlblTotal1.TabIndex = 14;
            this.mlblTotal1.Text = "0";
            this.mlblTotal1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // mlblList1
            // 
            this.mlblList1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mlblList1.AutoSize = true;
            this.mlblList1.Location = new System.Drawing.Point(12, 136);
            this.mlblList1.Name = "mlblList1";
            this.mlblList1.Size = new System.Drawing.Size(61, 13);
            this.mlblList1.TabIndex = 15;
            this.mlblList1.Text = "Команда 1";
            // 
            // mmnuListview
            // 
            this.mmnuListview.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mitmSelectAll,
            this.mitmAdd,
            this.mitmChange,
            this.mitmDelete,
            this.toolStripSeparator1,
            this.mitmCut,
            this.mitmCopy,
            this.mitmPaste});
            this.mmnuListview.Name = "mmnuListview";
            this.mmnuListview.Size = new System.Drawing.Size(191, 164);
            this.mmnuListview.Opening += new System.ComponentModel.CancelEventHandler(this.mmnuListview_Opening);
            this.mmnuListview.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.mmnuListview_ItemClicked);
            // 
            // mitmSelectAll
            // 
            this.mitmSelectAll.Image = ((System.Drawing.Image)(resources.GetObject("mitmSelectAll.Image")));
            this.mitmSelectAll.Name = "mitmSelectAll";
            this.mitmSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.mitmSelectAll.Size = new System.Drawing.Size(190, 22);
            this.mitmSelectAll.Text = "Выделить все";
            // 
            // mitmAdd
            // 
            this.mitmAdd.Image = ((System.Drawing.Image)(resources.GetObject("mitmAdd.Image")));
            this.mitmAdd.Name = "mitmAdd";
            this.mitmAdd.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.mitmAdd.Size = new System.Drawing.Size(190, 22);
            this.mitmAdd.Text = "Добавить";
            // 
            // mitmChange
            // 
            this.mitmChange.Image = ((System.Drawing.Image)(resources.GetObject("mitmChange.Image")));
            this.mitmChange.Name = "mitmChange";
            this.mitmChange.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.mitmChange.Size = new System.Drawing.Size(190, 22);
            this.mitmChange.Text = "Изменить";
            // 
            // mitmDelete
            // 
            this.mitmDelete.Enabled = false;
            this.mitmDelete.Image = ((System.Drawing.Image)(resources.GetObject("mitmDelete.Image")));
            this.mitmDelete.Name = "mitmDelete";
            this.mitmDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.mitmDelete.Size = new System.Drawing.Size(190, 22);
            this.mitmDelete.Text = "Удалить";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(187, 6);
            // 
            // mitmCut
            // 
            this.mitmCut.Enabled = false;
            this.mitmCut.Image = ((System.Drawing.Image)(resources.GetObject("mitmCut.Image")));
            this.mitmCut.Name = "mitmCut";
            this.mitmCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.mitmCut.Size = new System.Drawing.Size(190, 22);
            this.mitmCut.Text = "Вырезать";
            // 
            // mitmCopy
            // 
            this.mitmCopy.Enabled = false;
            this.mitmCopy.Image = ((System.Drawing.Image)(resources.GetObject("mitmCopy.Image")));
            this.mitmCopy.Name = "mitmCopy";
            this.mitmCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.mitmCopy.Size = new System.Drawing.Size(190, 22);
            this.mitmCopy.Text = "Скопировать";
            // 
            // mitmPaste
            // 
            this.mitmPaste.Enabled = false;
            this.mitmPaste.Image = ((System.Drawing.Image)(resources.GetObject("mitmPaste.Image")));
            this.mitmPaste.Name = "mitmPaste";
            this.mitmPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.mitmPaste.Size = new System.Drawing.Size(190, 22);
            this.mitmPaste.Text = "Вставить";
            // 
            // mrbtnTournament
            // 
            this.mrbtnTournament.AutoSize = true;
            this.mrbtnTournament.Checked = true;
            this.mrbtnTournament.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.mrbtnTournament.Location = new System.Drawing.Point(6, 19);
            this.mrbtnTournament.Name = "mrbtnTournament";
            this.mrbtnTournament.Size = new System.Drawing.Size(115, 18);
            this.mrbtnTournament.TabIndex = 17;
            this.mrbtnTournament.TabStop = true;
            this.mrbtnTournament.Text = "Круговой турнир";
            this.mrbtnTournament.UseVisualStyleBackColor = true;
            this.mrbtnTournament.CheckedChanged += new System.EventHandler(this.mrbtnTournament_CheckedChanged);
            // 
            // mrbtnCommands
            // 
            this.mrbtnCommands.AutoSize = true;
            this.mrbtnCommands.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.mrbtnCommands.Location = new System.Drawing.Point(6, 42);
            this.mrbtnCommands.Name = "mrbtnCommands";
            this.mrbtnCommands.Size = new System.Drawing.Size(137, 18);
            this.mrbtnCommands.TabIndex = 18;
            this.mrbtnCommands.Text = "Команда на команду";
            this.mrbtnCommands.UseVisualStyleBackColor = true;
            this.mrbtnCommands.CheckedChanged += new System.EventHandler(this.mrbtnCommands_CheckedChanged);
            // 
            // mlblTotal2
            // 
            this.mlblTotal2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mlblTotal2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mlblTotal2.Location = new System.Drawing.Point(280, 443);
            this.mlblTotal2.Name = "mlblTotal2";
            this.mlblTotal2.Size = new System.Drawing.Size(30, 16);
            this.mlblTotal2.TabIndex = 20;
            this.mlblTotal2.Text = "0";
            this.mlblTotal2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mrbtnLeagueGroup);
            this.groupBox1.Controls.Add(this.mrbtnOneToOne);
            this.groupBox1.Controls.Add(this.mrbtnCommands);
            this.groupBox1.Controls.Add(this.mrbtnTournament);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(150, 119);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Тип турнира";
            // 
            // mrbtnLeagueGroup
            // 
            this.mrbtnLeagueGroup.AutoSize = true;
            this.mrbtnLeagueGroup.Enabled = false;
            this.mrbtnLeagueGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.mrbtnLeagueGroup.Location = new System.Drawing.Point(6, 90);
            this.mrbtnLeagueGroup.Name = "mrbtnLeagueGroup";
            this.mrbtnLeagueGroup.Size = new System.Drawing.Size(110, 18);
            this.mrbtnLeagueGroup.TabIndex = 20;
            this.mrbtnLeagueGroup.Text = "Групповой этап";
            this.mrbtnLeagueGroup.UseVisualStyleBackColor = true;
            // 
            // mrbtnOneToOne
            // 
            this.mrbtnOneToOne.AutoSize = true;
            this.mrbtnOneToOne.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.mrbtnOneToOne.Location = new System.Drawing.Point(6, 66);
            this.mrbtnOneToOne.Name = "mrbtnOneToOne";
            this.mrbtnOneToOne.Size = new System.Drawing.Size(99, 18);
            this.mrbtnOneToOne.TabIndex = 19;
            this.mrbtnOneToOne.Text = "Один на один";
            this.mrbtnOneToOne.UseVisualStyleBackColor = true;
            this.mrbtnOneToOne.CheckedChanged += new System.EventHandler(this.mrbtnOneToOne_CheckedChanged);
            // 
            // mupdWinPoints
            // 
            this.mupdWinPoints.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mupdWinPoints.Location = new System.Drawing.Point(77, 19);
            this.mupdWinPoints.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.mupdWinPoints.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.mupdWinPoints.Name = "mupdWinPoints";
            this.mupdWinPoints.Size = new System.Drawing.Size(56, 20);
            this.mupdWinPoints.TabIndex = 3;
            this.mupdWinPoints.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.mupdWinPoints.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.mupdWinPoints.ValueChanged += new System.EventHandler(this.mupdWinPoints_ValueChanged);
            // 
            // mupdDrawPoints
            // 
            this.mupdDrawPoints.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mupdDrawPoints.Location = new System.Drawing.Point(208, 19);
            this.mupdDrawPoints.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.mupdDrawPoints.Name = "mupdDrawPoints";
            this.mupdDrawPoints.Size = new System.Drawing.Size(56, 20);
            this.mupdDrawPoints.TabIndex = 4;
            this.mupdDrawPoints.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.mupdDrawPoints.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.mupdDrawPoints.ValueChanged += new System.EventHandler(this.mupdDrawPoints_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "За победу:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(145, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "За ничью:";
            // 
            // mlblList2
            // 
            this.mlblList2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mlblList2.AutoSize = true;
            this.mlblList2.Location = new System.Drawing.Point(282, 135);
            this.mlblList2.Name = "mlblList2";
            this.mlblList2.Size = new System.Drawing.Size(61, 13);
            this.mlblList2.TabIndex = 27;
            this.mlblList2.Text = "Команда 2";
            // 
            // mlstNames2
            // 
            this.mlstNames2.AllowDrop = true;
            this.mlstNames2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mlstNames2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mlstNames2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader1,
            columnHeader2});
            this.mlstNames2.ContextMenuStrip = this.mmnuListview;
            this.mlstNames2.FullRowSelect = true;
            this.mlstNames2.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.mlstNames2.HideSelection = false;
            this.mlstNames2.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.mlstNames2.LabelEdit = true;
            this.mlstNames2.LabelWrap = false;
            this.mlstNames2.Location = new System.Drawing.Point(280, 186);
            this.mlstNames2.Name = "mlstNames2";
            this.mlstNames2.ShowGroups = false;
            this.mlstNames2.ShowItemToolTips = true;
            this.mlstNames2.Size = new System.Drawing.Size(260, 254);
            this.mlstNames2.TabIndex = 9;
            this.mlstNames2.UseCompatibleStateImageBehavior = false;
            this.mlstNames2.View = System.Windows.Forms.View.Details;
            this.mlstNames2.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.mlstNames_AfterLabelEdit);
            this.mlstNames2.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.mlstNames_ItemDrag);
            this.mlstNames2.DragDrop += new System.Windows.Forms.DragEventHandler(this.mlstNames_DragDrop);
            this.mlstNames2.DragEnter += new System.Windows.Forms.DragEventHandler(this.mlstNames_DragEnter);
            this.mlstNames2.DragOver += new System.Windows.Forms.DragEventHandler(this.mlstNames_DragOver);
            this.mlstNames2.DragLeave += new System.EventHandler(this.mlstNames_DragLeave);
            this.mlstNames2.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.mlstNames_GiveFeedback);
            this.mlstNames2.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.mlstNames_QueryContinueDrag);
            this.mlstNames2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mlstNames_KeyDown);
            this.mlstNames2.Resize += new System.EventHandler(this.mlstNames_Resize);
            // 
            // mlstNames1
            // 
            this.mlstNames1.AllowDrop = true;
            this.mlstNames1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mlstNames1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mlstNames1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            mcolName,
            mcolNumber});
            this.mlstNames1.ContextMenuStrip = this.mmnuListview;
            this.mlstNames1.FullRowSelect = true;
            this.mlstNames1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.mlstNames1.HideSelection = false;
            this.mlstNames1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem2});
            this.mlstNames1.LabelEdit = true;
            this.mlstNames1.LabelWrap = false;
            this.mlstNames1.Location = new System.Drawing.Point(12, 186);
            this.mlstNames1.Name = "mlstNames1";
            this.mlstNames1.ShowGroups = false;
            this.mlstNames1.ShowItemToolTips = true;
            this.mlstNames1.Size = new System.Drawing.Size(260, 254);
            this.mlstNames1.TabIndex = 7;
            this.mlstNames1.UseCompatibleStateImageBehavior = false;
            this.mlstNames1.View = System.Windows.Forms.View.Details;
            this.mlstNames1.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.mlstNames_AfterLabelEdit);
            this.mlstNames1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.mlstNames_ItemDrag);
            this.mlstNames1.DragDrop += new System.Windows.Forms.DragEventHandler(this.mlstNames_DragDrop);
            this.mlstNames1.DragEnter += new System.Windows.Forms.DragEventHandler(this.mlstNames_DragEnter);
            this.mlstNames1.DragOver += new System.Windows.Forms.DragEventHandler(this.mlstNames_DragOver);
            this.mlstNames1.DragLeave += new System.EventHandler(this.mlstNames_DragLeave);
            this.mlstNames1.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.mlstNames_GiveFeedback);
            this.mlstNames1.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.mlstNames_QueryContinueDrag);
            this.mlstNames1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mlstNames_KeyDown);
            this.mlstNames1.Resize += new System.EventHandler(this.mlstNames_Resize);
            // 
            // mtxtName1
            // 
            this.mtxtName1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mtxtName1.Location = new System.Drawing.Point(12, 154);
            this.mtxtName1.Name = "mtxtName1";
            this.mtxtName1.Size = new System.Drawing.Size(260, 20);
            this.mtxtName1.TabIndex = 6;
            this.mtxtName1.WordWrap = false;
            // 
            // mtxtName2
            // 
            this.mtxtName2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mtxtName2.Enabled = false;
            this.mtxtName2.Location = new System.Drawing.Point(280, 154);
            this.mtxtName2.Name = "mtxtName2";
            this.mtxtName2.Size = new System.Drawing.Size(260, 20);
            this.mtxtName2.TabIndex = 8;
            this.mtxtName2.WordWrap = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.mupdDrawPoints);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.mupdWinPoints);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(168, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(273, 56);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Очки";
            // 
            // mupdMatchesInPair
            // 
            this.mupdMatchesInPair.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mupdMatchesInPair.Enabled = false;
            this.mupdMatchesInPair.Location = new System.Drawing.Point(376, 74);
            this.mupdMatchesInPair.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.mupdMatchesInPair.Name = "mupdMatchesInPair";
            this.mupdMatchesInPair.Size = new System.Drawing.Size(56, 20);
            this.mupdMatchesInPair.TabIndex = 5;
            this.mupdMatchesInPair.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.mupdMatchesInPair.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // mlblMatchesInPair
            // 
            this.mlblMatchesInPair.AutoSize = true;
            this.mlblMatchesInPair.Location = new System.Drawing.Point(262, 76);
            this.mlblMatchesInPair.Name = "mlblMatchesInPair";
            this.mlblMatchesInPair.Size = new System.Drawing.Size(108, 13);
            this.mlblMatchesInPair.TabIndex = 31;
            this.mlblMatchesInPair.Text = "Игр в паре (группе):";
            // 
            // FControl
            // 
            this.AcceptButton = this.mbutFill;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.mbutCancel;
            this.ClientSize = new System.Drawing.Size(553, 468);
            this.Controls.Add(this.mupdMatchesInPair);
            this.Controls.Add(this.mlblMatchesInPair);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.mtxtName2);
            this.Controls.Add(this.mtxtName1);
            this.Controls.Add(this.mlblList2);
            this.Controls.Add(this.mlstNames2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.mlblTotal2);
            this.Controls.Add(this.mlstNames1);
            this.Controls.Add(this.mlblList1);
            this.Controls.Add(this.mlblTotal1);
            this.Controls.Add(this.mbutCancel);
            this.Controls.Add(this.mbutFill);
            this.Controls.Add(this.mbutClear);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FControl";
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.FControl_Load);
            this.mmnuListview.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mupdWinPoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mupdDrawPoints)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mupdMatchesInPair)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button mbutClear;
        private System.Windows.Forms.Button mbutFill;
        private System.Windows.Forms.Button mbutCancel;
        private System.Windows.Forms.Label mlblTotal1;
        private System.Windows.Forms.Label mlblList1;
        private System.Windows.Forms.RadioButton mrbtnTournament;
        private System.Windows.Forms.RadioButton mrbtnCommands;
        private System.Windows.Forms.Label mlblTotal2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown mupdWinPoints;
        private System.Windows.Forms.NumericUpDown mupdDrawPoints;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip mmnuListview;
        private System.Windows.Forms.ToolStripMenuItem mitmAdd;
        private System.Windows.Forms.ToolStripMenuItem mitmDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mitmCut;
        private System.Windows.Forms.ToolStripMenuItem mitmCopy;
        private System.Windows.Forms.ToolStripMenuItem mitmPaste;
        private UCListView mlstNames1;
        private System.Windows.Forms.ToolStripMenuItem mitmSelectAll;
        private System.Windows.Forms.ToolStripMenuItem mitmChange;
        private UCListView mlstNames2;
        private System.Windows.Forms.Label mlblList2;
        private System.Windows.Forms.TextBox mtxtName1;
        private System.Windows.Forms.TextBox mtxtName2;
        private System.Windows.Forms.RadioButton mrbtnOneToOne;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown mupdMatchesInPair;
        private System.Windows.Forms.Label mlblMatchesInPair;
        private System.Windows.Forms.RadioButton mrbtnLeagueGroup;
    }
}