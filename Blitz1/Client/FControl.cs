using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Blitz1.Client
{
    public partial class FControl : Form
    {
        private const string BOL = @"^";
        private const string DIGIT = @"\d";
        private const string OPTIONAL = @"?";
        private const string ANY_NUMBER = @"*";
        private const string OR = @"|";
        private const string LITERAL_DOT = @"\.";
        private const string WHITESPACE = @"\s";
        private const string NAME_GROUP = @"?<NAME>";
        private const string ANY_CHAR = @".";
        private const string ONE_OR_MORE = @"+";

        private const string REGEXP =
            BOL +
            "(" +
                "(" +
                    DIGIT + @"\u20E3" + OPTIONAL +
                ")" + OR + @"🔟" +
            ")" + ANY_NUMBER +
            LITERAL_DOT + OPTIONAL +
            WHITESPACE + ONE_OR_MORE +
            @"(" +
                NAME_GROUP +
                ANY_CHAR + ONE_OR_MORE +
            @")";
            //@"^((\d\u20E3?)|�)*\.?\s+(?<NAME>.+)",

        private ListView mclsDragSource;

        public FControl()
        {
            InitializeComponent();

            mupdWinPoints.Value = CClientData.PointsForWinning;
            mupdDrawPoints.Value = CClientData.PointsForDraw;
            mupdMatchesInPair.Value = CClientData.MatchesInPair;

            switch (CClientData.GameMode)
            {
                case ClientGameModeEnum.Round:
                    mrbtnTournament.Checked = true;
                    break;
                case ClientGameModeEnum.TeamByTeam:
                    mrbtnCommands.Checked = true;
                    break;
                case ClientGameModeEnum.OneToOne:
                    mrbtnOneToOne.Checked = true;
                    break;
                case ClientGameModeEnum.LeagueGroup:
                    mrbtnLeagueGroup.Checked = true;
                    break;
                default:
                    throw new Exception("Указано нераспознанное значение нумератора ClientGameMode");
            }

            //mtxtName1.Text = CCommonData.TeamName1;
            //mtxtName2.Text = CCommonData.TeamName2;

            mlstNames1.Items.Clear();
            //List<string> lstNames = CCommonData.ParticipantNamesGet(CCommonData.Participants1).ToList();
            //List<SParticipant> lstParticipants = CCommonData.Participants1.OrderBy(x => x.Position).ToList();
            //foreach (SParticipant clsItem in lstParticipants)
            //{
            //    ListViewItem clsListItem = new ListViewItem(new string[] { clsItem.Name, clsItem.Position.ToString() });
            //    mlstNames1.Items.Add(clsListItem);
            //}

            mlstNames2.Items.Clear();
            //lstNames = CCommonData.ParticipantNamesGet(CCommonData.Participants2).ToList();
            //lstParticipants = CCommonData.Participants2.OrderBy(x => x.Position).ToList();
            //foreach (SParticipant clsItem in lstParticipants)
            //{
            //    ListViewItem clsListItem = new ListViewItem(new string[] { clsItem.Name, clsItem.Position.ToString() });
            //    mlstNames2.Items.Add(clsListItem);
            //}

            //for (int i = 1; i <= 3; i++)
            //{
            //    ListViewItem clsListItem = new ListViewItem(new string[] { "A. Новый участник" + i.ToString(), i.ToString() });
            //    mlstNames1.Items.Add(clsListItem);

            //    clsListItem = new ListViewItem(new string[] { "B. Новый участник" + i.ToString(), i.ToString() });
            //    mlstNames2.Items.Add(clsListItem);
            //}

            if (mlstNames1.Items.Count > 0)
            {
                mlstNames1.Items[0].Selected = true;
            }

            if (mlstNames2.Items.Count > 0)
            {
                mlstNames2.Items[0].Selected = true;
            }

            StateUpdate(mlstNames1);
            StateUpdate(mlstNames2);

            mlstNames1.InsertionMark.Index = -1;
            mlstNames2.InsertionMark.Index = -1;
        }

        private void FControl_Load(
            object iSender,
            EventArgs iArgs)
        {
            mlstNames1.Focus();
        }

        private void mbutClear_Click(
            object iSender, 
            EventArgs iArgs)
        {
            DialogResult enmResult = MessageBox.Show(
                "Все данные будут удалены. Продолжить?",
                "Предупреждение",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (enmResult == DialogResult.OK)
            {
                CUtils.SheetReset(Globals.WSTable);
                CUtils.SheetReset(Globals.WSPlaces);
            }
        }

        private void mbutFill_Click(
            object iSender,
            EventArgs iArgs)
        {
            if (mrbtnTournament.Checked)
            {
                CClientData.GameMode = ClientGameModeEnum.Round;
            }
            else if (mrbtnCommands.Checked)
            {
                CClientData.GameMode = ClientGameModeEnum.TeamByTeam;
            }
            else if (mrbtnOneToOne.Checked)
            {
                CClientData.GameMode = ClientGameModeEnum.OneToOne;
            }
            else if (mrbtnLeagueGroup.Checked)
            {
                CClientData.GameMode = ClientGameModeEnum.LeagueGroup;
            }

            CClientData.TeamName1 = mtxtName1.Text;
            CClientData.TeamName2 = mtxtName2.Text;

            CClientData.Participants1.Clear();
            foreach (ListViewItem clsItem in mlstNames1.Items)
            {
                CClientData.Participants1.Add(clsItem.SubItems[0].Text);
            }

            CClientData.Participants2.Clear();
            foreach (ListViewItem clsItem in mlstNames2.Items)
            {
                CClientData.Participants2.Add(clsItem.SubItems[0].Text);
            }

            CClientData.PointsForWinning = (int)mupdWinPoints.Value;
            CClientData.PointsForDraw = (int)mupdDrawPoints.Value;
            CClientData.MatchesInPair = (int)mupdMatchesInPair.Value;
        }

        private void mrbtnTournament_CheckedChanged(
            object iSender,
            EventArgs iArgs)
        {
            ControlsEnable();
        }

        private void mrbtnCommands_CheckedChanged(
            object iSender, 
            EventArgs iArgs)
        {
            ControlsEnable();
        }

        private void mrbtnOneToOne_CheckedChanged(
            object iSender,
            EventArgs iArgs)
        {
            ControlsEnable();
        }

        private void mupdWinPoints_ValueChanged(object sender, EventArgs e)
        {

        }

        private void mupdDrawPoints_ValueChanged(object sender, EventArgs e)
        {

        }

        private void mmnuListview_ItemClicked(
            object iSender, 
            ToolStripItemClickedEventArgs iArgs)
        {
            ListView clsListView = (iSender as ContextMenuStrip).SourceControl as ListView;
            if (clsListView == null)
            {
                if (mlstNames1.Focused)
                {
                    clsListView = mlstNames1;
                }
                else if (mlstNames2.Focused)
                {
                    clsListView = mlstNames2;
                }
            }

            if (clsListView != null)
            {
                if (iArgs.ClickedItem == mitmSelectAll)
                {
                    AllSelect(clsListView);
                }
                else if (iArgs.ClickedItem == mitmAdd)
                {
                    ItemCreate(clsListView);
                }
                else if (iArgs.ClickedItem == mitmChange)
                {
                    ItemEdit(clsListView);
                }
                else if (iArgs.ClickedItem == mitmDelete)
                {
                    ItemsDelete(clsListView);
                }
                else if (iArgs.ClickedItem == mitmCut)
                {
                    ItemCut(clsListView);
                }
                else if (iArgs.ClickedItem == mitmCopy)
                {
                    ItemCopy(clsListView);
                }
                else if (iArgs.ClickedItem == mitmPaste)
                {
                    ItemPaste(clsListView);
                }
            }
        }

        private void mmnuListview_Opening(
            object iSender, 
            CancelEventArgs iArgs)
        {
            ListView clsListView = (iSender as ContextMenuStrip).SourceControl as ListView;
            mitmSelectAll.Enabled = clsListView.SelectedItems.Count > 0;
            mitmDelete.Enabled = clsListView.SelectedItems.Count > 0;
            mitmCut.Enabled = clsListView.SelectedItems.Count > 0;
            mitmCopy.Enabled = clsListView.SelectedItems.Count > 0;
            mitmChange.Enabled = clsListView.SelectedItems.Count > 0;
            mitmPaste.Enabled =
                Clipboard.ContainsText(TextDataFormat.Text) ||
                Clipboard.ContainsText(TextDataFormat.UnicodeText);
        }

        private void mlstNames_Resize(
            object iSender,
            EventArgs iArgs)
        {
            ListView clsListView = iSender as ListView;
            ColumnWidthAdjust(clsListView);
        }

        private void mlstNames_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {

        }

        private void mlstNames_KeyDown(
            object iSender,
            KeyEventArgs iArgs)
        {
            ListView clsListView = iSender as ListView;
            switch (iArgs.Modifiers)
            {
                case Keys.None:
                    switch (iArgs.KeyCode)
                    {
                        case Keys.Delete:
                            ItemsDelete(clsListView);
                            break;
                        case Keys.F2:
                            ItemEdit(clsListView);
                            break;
                    }
                    break;
                case Keys.Control:
                    switch (iArgs.KeyCode)
                    {
                        case Keys.A:
                            AllSelect(clsListView);
                            break;
                        case Keys.N:
                            ItemCreate(clsListView);
                            break;
                        case Keys.V:
                            ItemPaste(clsListView);
                            break;
                        case Keys.X:
                            ItemCut(clsListView);
                            break;
                        case Keys.C:
                        case Keys.Insert:
                            ItemCopy(clsListView);
                            break;
                    }
                    break;
                case Keys.Shift:
                    switch (iArgs.KeyCode)
                    {
                        case Keys.Insert:
                            ItemPaste(clsListView);
                            break;
                        case Keys.Delete:
                            ItemCut(clsListView);
                            break;
                    }
                    break;
            }
        }

        private void mlstNames_ItemDrag(
            object iSender,
            ItemDragEventArgs iArgs)
        {
            ListView clsListView = iSender as ListView;
            DataObject clsDataObject = DataObjectCreate(clsListView);

            if (clsDataObject != null)
            {
                mclsDragSource = clsListView;
                clsListView.DoDragDrop(clsDataObject, DragDropEffects.Move);
            }
        }

        private void mlstNames_DragDrop(
            object iSender, 
            DragEventArgs iArgs)
        {
            ListView clsListView = iSender as ListView;

            // Retrieve the index of the insertion mark;
            int intTargetIndex = clsListView.InsertionMark.Index;

            // If the insertion mark is not visible, exit the method.
            if (intTargetIndex == -1)
            {
                return;
            }

            // If the insertion mark is to the right of the item with
            // the corresponding index, increment the target index.
            if (clsListView.InsertionMark.AppearsAfterItem)
            {
                intTargetIndex++;
            }

            DataObject clsDataObject = (DataObject)iArgs.Data;
            FromDataObjectInsert(clsDataObject, clsListView, intTargetIndex);

            List<ListViewItem> lstItems = null;

            if (clsDataObject.GetDataPresent(typeof(List<ListViewItem>)))
            {
                lstItems = (List<ListViewItem>)clsDataObject.GetData(typeof(List<ListViewItem>));
                if (lstItems != null)
                {
                    foreach (ListViewItem clsItem in lstItems)
                    {
                        clsItem.ListView.Items.Remove(clsItem);
                    }
                }
            }
            StateUpdate(clsListView);

            if ((mclsDragSource != null) &&
                (mclsDragSource != clsListView))
            {
                StateUpdate(mclsDragSource);
            }

            mclsDragSource = null;
            clsListView.InsertionMark.Index = -1;
        }

        private void mlstNames_QueryContinueDrag(
            object iSender, 
            QueryContinueDragEventArgs iArgs)
        {
            switch (iArgs.Action)
            {
                case DragAction.Cancel:
                    mclsDragSource = null;
                    break;
                case DragAction.Continue:
                    break;
                case DragAction.Drop:
                    break;
            }
        }

        private void mlstNames_DragEnter(
            object iSender, 
            DragEventArgs iArgs)
        {
            if (iArgs.Data.GetDataPresent(typeof(List<ListViewItem>)))
            {
                iArgs.Effect = DragDropEffects.Move;
            }
            else if (iArgs.Data.GetDataPresent(DataFormats.UnicodeText, true))
            {
                iArgs.Effect = DragDropEffects.Copy;
            }
            else
            {
                iArgs.Effect = DragDropEffects.None;
            }
        }

        private void mlstNames_DragLeave(
            object iSender, 
            EventArgs iArgs)
        {
            ListView clsListView = iSender as ListView;
            clsListView.InsertionMark.Index = -1;
        }

        private void mlstNames_DragOver(
            object iSender, 
            DragEventArgs iArgs)
        {
            ListView clsListView = iSender as ListView;
            // Retrieve the client coordinates of the mouse pointer.
            Point clsTargetPoint = clsListView.PointToClient(new Point(iArgs.X, iArgs.Y));

            // Retrieve the index of the item closest to the mouse pointer.
            int intTargetIndex = clsListView.InsertionMark.NearestIndex(clsTargetPoint);
            Rectangle clsItemBounds = new Rectangle();

            // Confirm that the mouse pointer is not over the dragged item.
            if (intTargetIndex > -1)
            {
                // Determine whether the mouse pointer is to the left or
                // the right of the midpoint of the closest item and set
                // the InsertionMark.AppearsAfterItem property accordingly.
                clsItemBounds = clsListView.GetItemRect(intTargetIndex);

                if (clsTargetPoint.Y > clsItemBounds.Top + (clsItemBounds.Height / 2))
                {
                    clsListView.InsertionMark.AppearsAfterItem = true;
                }
                else
                {
                    clsListView.InsertionMark.AppearsAfterItem = false;
                }
                clsListView.EnsureVisible(intTargetIndex);
            }
            else
            {
                ListViewItem clsCurrentItem = clsListView.GetItemAt(clsTargetPoint.X, clsTargetPoint.Y);
                if (clsCurrentItem != null)
                {
                    clsItemBounds = clsListView.GetItemRect(clsCurrentItem.Index);

                    if (clsTargetPoint.Y > clsItemBounds.Top + (clsItemBounds.Height / 2))
                    {
                        clsListView.EnsureVisible(Math.Min(clsCurrentItem.Index + 1, clsListView.Items.Count - 1));
                    }
                    else
                    {
                        clsListView.EnsureVisible(Math.Max(clsCurrentItem.Index - 1, 0));
                    }
                }
                else
                {
                    if (clsListView.Items.Count == 0)
                    {
                        intTargetIndex = 0;
                        clsListView.InsertionMark.AppearsAfterItem = false;
                    }
                }
            }

            // Set the location of the insertion mark. If the mouse is
            // over the dragged item, the targetIndex value is -1 and
            // the insertion mark disappears.
            clsListView.InsertionMark.Index = intTargetIndex;
        }

        private void mlstNames_GiveFeedback(
            object iSender, 
            GiveFeedbackEventArgs iArgs)
        {
        }

        private DataObject DataObjectCreate(ListView iListView)
        {
            DataObject clsDataObject = new DataObject();
            string strData = null;
            if (iListView.SelectedItems.Count > 0)
            {
                foreach (ListViewItem clsItem in iListView.SelectedItems)
                {
                    strData += clsItem.SubItems[0].Text + Environment.NewLine;

                }
            }

            if (strData != null)
            {
                clsDataObject.SetText(strData);
                clsDataObject.SetData(
                    typeof(List<ListViewItem>),
                    new List<ListViewItem>(iListView.SelectedItems.Cast<ListViewItem>()));
            }
            return clsDataObject;
        }

        private void FromDataObjectInsert(
            DataObject iDataObject,
            ListView iListView,
            int iIndexAt)
        {
            // Retrieve the index of the insertion mark;
            int intTargetIndex = iListView.InsertionMark.Index;

            if (intTargetIndex == -1)
            {
                if (iListView.SelectedIndices.Count > 0)
                {
                    intTargetIndex = iListView.SelectedIndices[0] + 1;
                }
                else
                {
                    intTargetIndex = iListView.Items.Count;
                }
            }
            else
            {
                // If the insertion mark is to the right of the item with
                // the corresponding index, increment the target index.
                if (iListView.InsertionMark.AppearsAfterItem)
                {
                    intTargetIndex++;
                }
            }

            List<string> lstItemNames = null;
            List<ListViewItem> lstItems = null;

            if (iDataObject.GetDataPresent(typeof(List<ListViewItem>)))
            {
                lstItems = (List<ListViewItem>)iDataObject.GetData(typeof(List<ListViewItem>));

                lstItemNames = lstItems
                    .Cast<ListViewItem>()
                    .OrderBy(x => x.Index)
                    .Select(x => x.SubItems[0].Text)
                    .ToList();
            }
            else if (iDataObject.GetDataPresent(DataFormats.UnicodeText))
            {
                string strList = (string)iDataObject.GetData(DataFormats.UnicodeText, true);
                if (!string.IsNullOrWhiteSpace(strList))
                {
                    lstItemNames = strList
                        .Split(new string[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .ToList();

                    for (int i = 0; i < lstItemNames.Count; i++)
                    {
                        string strName = lstItemNames[i].Trim();

                        Match clsMatch = Regex.Match(strName, REGEXP);

                        if (clsMatch.Success)
                        {
                            strName = clsMatch.Groups["NAME"].Value;
                        }

                        lstItemNames[i] = strName.Trim();
                    }
                }
            }

            iListView.SelectedItems.Clear();
            if (lstItemNames != null)
            {
                //Список проходим с конца, т.к. вставляем в одно и то же место, т.е. каждый раз в начало списка.
                for (int i = lstItemNames.Count - 1; i >= 0; i--)
                {
                    ListViewItem clsItem = new ListViewItem(new string[] { lstItemNames[i], "" });
                    iListView.Items.Insert(intTargetIndex, clsItem);
                    clsItem.Selected = true;
                }
            }

            StateUpdate(iListView);
        }

        private void AllSelect(ListView iListView)
        {
            foreach (ListViewItem clsItem in iListView.Items)
            {
                clsItem.Selected = true;
            }
        }

        private void ItemsDelete(ListView iListView)
        {
            if (iListView.SelectedItems.Count > 0)
            {
                DialogResult enmResult = MessageBox.Show(
                    "Удалить участников из списка?",
                    "Удаление",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);
                if (enmResult == DialogResult.OK)
                {
                    foreach (ListViewItem clsItem in iListView.SelectedItems)
                    {
                        iListView.Items.Remove(clsItem);
                    }
                    StateUpdate(iListView);
                }
            }
        }

        private void ItemEdit(ListView iListView)
        {
            if (iListView.SelectedItems.Count > 0)
            {
                iListView.SelectedItems[iListView.SelectedItems.Count - 1].BeginEdit();
            }
        }

        private void ItemCreate(ListView iListView)
        {
            ListViewItem clsItem = new ListViewItem(
                new string[] { "Новый участник", (iListView.Items.Count + 1).ToString() });
            iListView.Items.Add(clsItem);
            iListView.SelectedItems.Clear();
            StateUpdate(iListView);
            clsItem.EnsureVisible();
            clsItem.BeginEdit();
        }

        private void ItemPaste(ListView iListView)
        {
            DataObject clsDataObject = (DataObject)Clipboard.GetDataObject();
            FromDataObjectInsert(clsDataObject, iListView, iListView.Items.Count);
        }

        private void ItemCut(ListView iListView)
        {
            ItemCopy(iListView);
            foreach (ListViewItem clsItem in iListView.SelectedItems)
            {
                iListView.Items.Remove(clsItem);
            }
            StateUpdate(iListView);
        }

        private void ItemCopy(ListView iListView)
        {
            DataObject clsDataObject = DataObjectCreate(iListView);
            Clipboard.SetDataObject(clsDataObject);
        }

        private void ItemsRenumber(ListView iListViewControl)
        {
            foreach (ListViewItem clsItem in iListViewControl.Items)
            {
                clsItem.SubItems[1].Text = (clsItem.Index + 1).ToString();
            }
        }

        private void ColumnWidthAdjust(ListView iListView)
        {
            int intFullShowedItems = 0;
            int intItemsHeight = 0;
            ListViewItem clsTopItem = iListView.TopItem;
            if (clsTopItem != null)
            {
                for (int i = clsTopItem.Index; i < iListView.Items.Count; i++)
                {
                    Rectangle clsItemRect;
                    try
                    {
                        clsItemRect = iListView.GetItemRect(i);
                    }
                    catch
                    {
                        break;
                    }
                    if (intItemsHeight + clsItemRect.Height < iListView.ClientSize.Height)
                    {
                        intItemsHeight += clsItemRect.Height;
                        intFullShowedItems++;
                    }
                }
            }
            int intWidth = iListView.Size.Width;

            if (intFullShowedItems < iListView.Items.Count)
            {
                intWidth -= SystemInformation.VerticalScrollBarWidth;
            }

            switch (iListView.BorderStyle)
            {
                case BorderStyle.Fixed3D:
                    intWidth -= 4;
                    break;
                case BorderStyle.FixedSingle:
                    intWidth -= 2;
                    break;
            }

            // Loop through all columns except the last one.
            for (int i = 1; i < iListView.Columns.Count; i++)
            {
                // Subtract width of the column from the width
                // of the client area.
                intWidth -= iListView.Columns[i].Width;

                // If the width goes below 1, then no need to keep going
                // because the last column can't be sized to fit due to
                // the widths of the columns before it.
                if (intWidth < 1)
                {
                    break;
                }
            };

            // If there is any width remaining, that will
            // be the width of the last column.
            if (intWidth > 0)
            {
                iListView.Columns[0].Width = intWidth;
            }
        }

        private void StateUpdate(ListView iListView)
        {
            ColumnWidthAdjust(iListView);
            ItemsRenumber(iListView);
            mlblTotal1.Text = mlstNames1.Items.Count.ToString();
            mlblTotal2.Text = mlstNames2.Items.Count.ToString();
            ControlsEnable();
        }

        private void ControlsEnable()
        {
            bool bln2Commands = mrbtnCommands.Checked || mrbtnOneToOne.Checked || mrbtnLeagueGroup.Checked;
            mlstNames2.Enabled = bln2Commands;
            mlblList2.Enabled = bln2Commands;
            mlblTotal2.Enabled = bln2Commands;
            //mtxtName1.Enabled = bln2Commands;
            mtxtName2.Enabled = bln2Commands;
            mlblMatchesInPair.Enabled = mrbtnOneToOne.Checked || mrbtnLeagueGroup.Checked;
            mupdMatchesInPair.Enabled = mrbtnOneToOne.Checked || mrbtnLeagueGroup.Checked;
            mbutFill.Enabled = (mlstNames1.Items.Count > 1);
            if (bln2Commands)
            {
                mbutFill.Enabled &= (mlstNames2.Items.Count > 1);
            }

            if (mrbtnOneToOne.Checked ||
                mrbtnLeagueGroup.Checked)
            {
                mbutFill.Enabled &= (mlstNames1.Items.Count == mlstNames2.Items.Count);
            }
        }
    }
}
