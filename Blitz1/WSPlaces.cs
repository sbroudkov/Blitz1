using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Office.Tools.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace Blitz1.Client
{
    public partial class WSPlaces
    {
        private bool mblnIsActivating;

        internal void GroupsCreate()
        {
            //В Excel 2007 происходит ошибка 0x800A03EC при попытке выполнить Merge для второй таблицы после того,
            //как первая полностью подготовлена. Ошибка очень общая, имеющиеся способы ее решения результата не дают.
            //В качестве заглушки приходится сначала делать Merge на первой (а затем на второй) таблице, только
            //ПОСЛЕ этого применять все остальное форматирование и заполнение.
            MergeCells(XStartGet(0), YStartGet(), CClientData.Participants1.Count);
            if ((CClientData.GameMode == ClientGameModeEnum.TeamByTeam) ||
                (CClientData.GameMode == ClientGameModeEnum.OneToOne))
            {
                MergeCells(XStartGet(1), YStartGet(), CClientData.Participants2.Count);
            }

            GroupCreate(0, CClientData.TeamName1, CClientData.Participants1.Count);
            if ((CClientData.GameMode == ClientGameModeEnum.TeamByTeam) ||
                (CClientData.GameMode == ClientGameModeEnum.OneToOne))
            {
                GroupCreate(1, CClientData.TeamName2, CClientData.Participants2.Count);
            }

            this.EnableSelection = Excel.XlEnableSelection.xlUnlockedCells;
            CUtils.InternalProtect(this);
        }

        protected override void OnStartup()
        {
            //base.OnStartup();
            //OnActivate();
        }

        public override void EndInit()
        {
            base.EndInit();
            //OnActivate();
        }

        private void Sheet2_Startup(
            object iSender,
            EventArgs iArgs)
        {
            OnActivate();
        }

        private void Sheet2_Shutdown(
            object iSender, 
            EventArgs iArgs)
        {
        }

        private void WSPlaces_ActivateEvent()
        {
            OnActivate();
        }

        private void clsButton_Click(
            object iSender,
            EventArgs iArgs)
        {
            Microsoft.Office.Tools.Excel.Controls.Button clsButton = (Microsoft.Office.Tools.Excel.Controls.Button)iSender;
            int intColumn = clsButton.TopLeftCell.Column;
            intColumn = intColumn
                - 1 //колонки считаются с 1, привести к базе 0
                - 1  //Колонка места.
                - 1; //Разделитель.

            int intGroupColumn = intColumn / (CViewConst.EXTRA_COLUMNS + 1);

            string strTotals = ThisWorkbook.TotalsStringGet(intGroupColumn);

            Clipboard.SetText(strTotals);
            MessageBox.Show("Результаты скопированы", "Результаты", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void GroupCreate(
            int iColumn,
            string iName,
            int iCount)
        {
            int intXStart = XStartGet(iColumn);
            int intYStart = YStartGet();

            SizesSet(intXStart, intYStart, iCount);
            //Прорисовка рамок.
            BordersSet(intXStart, intYStart, iCount);
            FormatsSet(intXStart, intYStart, iCount);
            FixedTextsSet(intXStart, intYStart, iCount);
            VariableTextsSet(intXStart, intYStart, iName);
        }

        private int XStartGet(int iGroupColumn)
        {
            return (1 + //Разделитель
            1 + //Колонка команд
            CViewConst.EXTRA_COLUMNS - 1 // Колонка комментария в итогах не показывается
            ) * iGroupColumn + //Индекс групповой колонки
            1 + // Разделитель
            1; //Нумерация с 1
        }

        private int YStartGet()
        {
            return 
            1 +// Разделитель
            1; //Нумерация с 1
        }

        private void SizesSet(
            int iX0,
            int iY0,
            int iCount)
        {
            Columns[iX0 - 1].ColumnWidth = CViewConst.SEPARATOR_COLUMN_WIDTH;
            Columns[iX0].ColumnWidth = CViewConst.SCORE_COLUMN_WIDTH;
            Columns[iX0 + 1].ColumnWidth = CViewConst.PARTICIPANTS_COLUMN_WIDTH;
            for (int i = 0; i < CViewConst.EXTRA_COLUMNS - 1; i++)
            {
                Columns[iX0 + 2 + i].ColumnWidth = CViewConst.WIDE_SCORE_COLUMN_WIDTH;
            }

            Rows[iY0 - 1].RowHeight = CViewConst.SEPARATOR_ROW_HEIGHT;
            Rows[iY0].RowHeight = CViewConst.PARTICIPANT_CAPTION_ROW_HEIGHT;
            for (int i = 0; i < iCount; i++)
            {
                Rows[iY0 + 1 + i].RowHeight = CViewConst.PARTICIPANT_ROW_HEIGHT;
            }
        }

        private void BordersSet(
            int iX0,
            int iY0,
            int iCount)
        {
            //Конец таблицы.
            int intX1 = iX0 + CViewConst.EXTRA_COLUMNS - 1; //Колонка комментария в итогах не показывается
            int intY1 = iY0 + iCount;

            //Рамка места.
            CUtils.BoxSet(CUtils.ColumnGet(this, iY0, intY1, iX0), Excel.XlBorderWeight.xlMedium);
            //Рамка колонки команд.
            CUtils.BoxSet(CUtils.ColumnGet(this, iY0, intY1, iX0 + 1), Excel.XlBorderWeight.xlMedium);

            //Рамка заголовка.
            CUtils.BoxSet(CUtils.RowGet(this, iY0, iX0, intX1), Excel.XlBorderWeight.xlMedium);

            //Вертикальные рамки команд плюс рамка забитых/пропущенных мячей (по 2 ячейки в строке).
            CUtils.BorderSet(
                CUtils.ColumnGet(this, iY0, intY1, iX0 + 3),
                new List<Excel.XlBordersIndex> { Excel.XlBordersIndex.xlEdgeRight },
                new List<Excel.XlLineStyle> { Excel.XlLineStyle.xlContinuous },
                new List<Excel.XlBorderWeight> { Excel.XlBorderWeight.xlThin });

            //Вертикальные рамки колонки разницы.
            CUtils.BorderSet(
                CUtils.ColumnGet(this, iY0, intY1, intX1 + CViewConst.COLUMN_DIFF_OFFSET + 2),
                new List<Excel.XlBordersIndex> { Excel.XlBordersIndex.xlEdgeRight },
                new List<Excel.XlLineStyle> { Excel.XlLineStyle.xlContinuous },
                new List<Excel.XlBorderWeight> { Excel.XlBorderWeight.xlThin });

            //Вертикальные рамки колонок В/Н/П, очки, игры, место
            for (int i = 0; i < -CViewConst.COLUMN_DIFF_OFFSET - 2; i++) // 
            {
                CUtils.BorderSet(
                    CUtils.ColumnGet(this, iY0, intY1, intX1 + CViewConst.COLUMN_WIN_OFFSET + i + 2),
                    new List<Excel.XlBordersIndex> { Excel.XlBordersIndex.xlEdgeRight },
                    new List<Excel.XlLineStyle> { Excel.XlLineStyle.xlContinuous },
                    new List<Excel.XlBorderWeight> { Excel.XlBorderWeight.xlThin });
            }

            //Рамка итогов.
            CUtils.BoxSet(
                CUtils.RectGet(this, iY0, intX1 + CViewConst.COLUMN_WIN_OFFSET + 2, intY1, intX1 + CViewConst.COLUMN_LOST_OFFSET + 2),
                Excel.XlBorderWeight.xlMedium);

            //Горизонтальные рамки команд.
            for (int i = 0; i < iCount - 1; i++)
            {
                CUtils.BorderSet(
                    CUtils.RowGet(this, iY0 + 1 + i, iX0, intX1),
                    new List<Excel.XlBordersIndex> { Excel.XlBordersIndex.xlEdgeBottom },
                    new List<Excel.XlLineStyle> { Excel.XlLineStyle.xlContinuous },
                    new List<Excel.XlBorderWeight> { Excel.XlBorderWeight.xlThin });
            }

            //Рамки счета матчей и итоговой разницы.
            for (int j = 0; j < iCount; j++)
            {
                CUtils.BorderSet(
                    Cells[iY0 + 1 + j, iX0 + 2],
                    new List<Excel.XlBordersIndex> { Excel.XlBordersIndex.xlEdgeRight },
                    new List<Excel.XlLineStyle> { Excel.XlLineStyle.xlDot },
                    new List<Excel.XlBorderWeight> { Excel.XlBorderWeight.xlHairline });
            }

            //Общая рамка.
            CUtils.BoxSet(
                CUtils.RectGet(this, iY0, iX0, intY1, intX1),
                Excel.XlBorderWeight.xlMedium);
        }

        private void FixedTextsSet(
            int iX0,
            int iY0,
            int iCount)
        {
            //Конец таблицы.
            int intX1 = iX0 + CViewConst.EXTRA_COLUMNS - 1; //Колонка комментария не учитывается

            Cells[iY0, iX0].Value2 = "Место";
            Cells[iY0, intX1 + CViewConst.COLUMN_PLUS_OFFSET + 2].Value2 = "Разность";
            Cells[iY0, intX1 + CViewConst.COLUMN_WIN_OFFSET + 2].Value2 = "В";
            Cells[iY0, intX1 + CViewConst.COLUMN_DRAW_OFFSET + 2].Value2 = "Н";
            Cells[iY0, intX1 + CViewConst.COLUMN_LOST_OFFSET + 2].Value2 = "П";
            Cells[iY0, intX1 + CViewConst.COLUMN_POINTS_OFFSET + 2].Value2 = "Очков";
            Cells[iY0, intX1 + CViewConst.COLUMN_MATCHES_OFFSET + 2].Value2 = "Игр";
            if ((CClientData.GameMode == ClientGameModeEnum.TeamByTeam) ||
                (CClientData.GameMode == ClientGameModeEnum.OneToOne))
            {
                Cells[iY0 + iCount + 1, intX1 + CViewConst.COLUMN_PLUS_OFFSET + 2 - 1].Value2 = "Всего:";
            }
        }

        private void VariableTextsSet(
            int iX0, 
            int iY0, 
            string iName)
        {
            Cells[iY0 - 1, iX0 + 1].Value2 = iName;
        }

        private void MergeCells(
            int iX0,
            int iY0,
            int iCount)
        {
            //Конец таблицы.
            int intX1 = iX0 + CViewConst.EXTRA_COLUMNS;

            CUtils.UpdateExecute(
                this,
                delegate
                {
                    CUtils.RowGet(this, iY0, intX1 + CViewConst.COLUMN_PLUS_OFFSET + 1, intX1 + CViewConst.COLUMN_DIFF_OFFSET + 1).Merge(false);
                });
        }

        private void FormatsSet(
            int iX0,
            int iY0,
            int iCount)
        {
            //Конец таблицы.
            int intX1 = iX0 + CViewConst.EXTRA_COLUMNS - 1; //Колонка комментария в итогах не показывается
            int intY1 = iY0 + iCount;

            //Первое место.
            CUtils.RowGet(this, iY0 + 1, iX0, intX1).Interior.Color = CColorConst.GOLD;

            //Второе место.
            CUtils.RowGet(this, iY0 + 2, iX0, intX1).Interior.Color = CColorConst.SILVER;

            //Третье место.
            if (iCount > 2)
            {
                CUtils.RowGet(this, iY0 + 3, iX0, intX1).Interior.Color = CColorConst.BRONZE;
            }

            Excel.Range clsRange = Cells[iY0 - 1, iX0 + 1];
            clsRange.Font.Bold = true;
            clsRange.NumberFormat = "@";

            //Заголовки строк команд.
            clsRange = CUtils.ColumnGet(this, iY0 + 1, intY1, iX0 + 1);
            clsRange.Font.Bold = true;
            clsRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            clsRange.NumberFormat = "@";

            //Заголовки столбцов команд.
            clsRange = CUtils.RowGet(this, iY0, iX0, intX1);
            clsRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            clsRange.Font.Bold = true;
            clsRange.Orientation = 90;
            clsRange.NumberFormat = "@";

            //Итоги
            CUtils.ColumnGet(this, iY0 + 1, intY1, intX1 + CViewConst.COLUMN_WIN_OFFSET + 2).Font.Color = CColorConst.WIN;
            CUtils.ColumnGet(this, iY0 + 1, intY1, intX1 + CViewConst.COLUMN_DRAW_OFFSET + 2).Font.Color = CColorConst.DRAW;
            CUtils.ColumnGet(this, iY0 + 1, intY1, intX1 + CViewConst.COLUMN_LOST_OFFSET + 2).Font.Color = CColorConst.LOSE;
            CUtils.ColumnGet(this, iY0 + 1, intY1, iX0).Font.Bold = true;
            CUtils.RectGet(
                this,
                iY0 + 1,
                intX1 + CViewConst.COLUMN_WIN_OFFSET + 2,
                intY1,
                intX1 + CViewConst.COLUMN_MATCHES_OFFSET + 2).Font.Bold = true;

            if ((CClientData.GameMode == ClientGameModeEnum.TeamByTeam) ||
                (CClientData.GameMode == ClientGameModeEnum.OneToOne))
            {
                clsRange = Cells[iY0 + iCount + 1, intX1 + CViewConst.COLUMN_PLUS_OFFSET + 2 - 1];
                clsRange.Font.Bold = true;
                clsRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                clsRange = Cells[iY0 + iCount + 1, intX1 + CViewConst.COLUMN_PLUS_OFFSET + 2];
                clsRange.Font.Bold = true;
                clsRange.Locked = false;

                clsRange = Cells[iY0 + iCount + 1, intX1 + CViewConst.COLUMN_MINUS_OFFSET + 2];
                clsRange.Font.Bold = true;
                clsRange.Locked = false;

                clsRange = Cells[iY0 + iCount + 1, intX1 + CViewConst.COLUMN_DIFF_OFFSET + 2];
                clsRange.Font.Bold = true;
                clsRange.Locked = false;

                clsRange = Cells[iY0 + iCount + 1, intX1 + CViewConst.COLUMN_WIN_OFFSET + 2];
                clsRange.Font.Bold = true;
                clsRange.Font.Color = CColorConst.WIN;
                clsRange.Locked = false;

                clsRange = Cells[iY0 + iCount + 1, intX1 + CViewConst.COLUMN_DRAW_OFFSET + 2];
                clsRange.Font.Bold = true;
                clsRange.Font.Color = CColorConst.DRAW;
                clsRange.Locked = false;

                clsRange = Cells[iY0 + iCount + 1, intX1 + CViewConst.COLUMN_LOST_OFFSET + 2];
                clsRange.Font.Bold = true;
                clsRange.Font.Color = CColorConst.LOSE;
                clsRange.Locked = false;

                clsRange = Cells[iY0 + iCount + 1, intX1 + CViewConst.COLUMN_POINTS_OFFSET + 2];
                clsRange.Font.Bold = true;
                clsRange.Locked = false;

                clsRange = Cells[iY0 + iCount + 1, intX1 + CViewConst.COLUMN_MATCHES_OFFSET + 2];
                clsRange.Font.Bold = true;
                clsRange.Locked = false;
            }
        }

        private void ButtonAdd(
            int iX0,
            int iY0)
        {
            Microsoft.Office.Tools.Excel.Controls.Button clsButton =
                this.Controls.AddButton(CUtils.RectGet(this, iY0, iX0 + 1, iY0, iX0 + 1), "Copy" + iX0.ToString() + iY0.ToString());
            clsButton.Text = "Копировать";
            clsButton.Click += clsButton_Click;
        }

        private void Update(
            int iColumn,
            List<Server.SParticipantTotalsForClient> iData)
        {
            int intXStart = XStartGet(iColumn);
            int intYStart = YStartGet();

            //Конец таблицы.
            int intX1 = intXStart + CViewConst.EXTRA_COLUMNS;
            int intY1 = intYStart + iData.Count + 1;

            Excel.Range clsRange = CUtils.RectGet(
                this,
                intYStart + 1,
                intXStart,
                intYStart + iData.Count + 1,
                intX1);

            object[,] objValues = clsRange.Value2;

            for (int i = 0; i < iData.Count; i++)
            {
                Server.SParticipantTotalsForClient clsParticipant = iData[i];

                objValues[i + 1, 1] = clsParticipant.Place;
                objValues[i + 1, 2] = clsParticipant.Name;
                objValues[i + 1, intX1 - intXStart + 1 + CViewConst.COLUMN_PLUS_OFFSET + 1] = clsParticipant.Plus;
                objValues[i + 1, intX1 - intXStart + 1 + CViewConst.COLUMN_MINUS_OFFSET + 1] = clsParticipant.Minus;
                objValues[i + 1, intX1 - intXStart + 1 + CViewConst.COLUMN_DIFF_OFFSET + 1] = clsParticipant.Difference;
                objValues[i + 1, intX1 - intXStart + 1 + CViewConst.COLUMN_WIN_OFFSET + 1] = clsParticipant.Wins;
                objValues[i + 1, intX1 - intXStart + 1 + CViewConst.COLUMN_DRAW_OFFSET + 1] = clsParticipant.Draws;
                objValues[i + 1, intX1 - intXStart + 1 + CViewConst.COLUMN_LOST_OFFSET + 1] = clsParticipant.Loses;
                objValues[i + 1, intX1 - intXStart + 1 + CViewConst.COLUMN_POINTS_OFFSET + 1] = clsParticipant.Points;
                objValues[i + 1, intX1 - intXStart + 1 + CViewConst.COLUMN_MATCHES_OFFSET + 1] = clsParticipant.MatchesPlayed;
            }

            if ((CClientData.GameMode == ClientGameModeEnum.TeamByTeam) ||
                (CClientData.GameMode == ClientGameModeEnum.OneToOne))
            {
                objValues[intY1 - intYStart, intX1 - intXStart + 1 + CViewConst.COLUMN_PLUS_OFFSET + 1] = iData.Sum(x => x.Plus);
                objValues[intY1 - intYStart, intX1 - intXStart + 1 + CViewConst.COLUMN_MINUS_OFFSET + 1] = iData.Sum(x => x.Minus);
                objValues[intY1 - intYStart, intX1 - intXStart + 1 + CViewConst.COLUMN_DIFF_OFFSET + 1] = iData.Sum(x => x.Difference);
                objValues[intY1 - intYStart, intX1 - intXStart + 1 + CViewConst.COLUMN_WIN_OFFSET + 1] = iData.Sum(x => x.Wins);
                objValues[intY1 - intYStart, intX1 - intXStart + 1 + CViewConst.COLUMN_DRAW_OFFSET + 1] = iData.Sum(x => x.Draws);
                objValues[intY1 - intYStart, intX1 - intXStart + 1 + CViewConst.COLUMN_LOST_OFFSET + 1] = iData.Sum(x => x.Loses);
                objValues[intY1 - intYStart, intX1 - intXStart + 1 + CViewConst.COLUMN_POINTS_OFFSET + 1] = iData.Sum(x => x.Points);
                objValues[intY1 - intYStart, intX1 - intXStart + 1 + CViewConst.COLUMN_MATCHES_OFFSET + 1] = iData.Sum(x => x.MatchesPlayed);
            }

            clsRange.Value2 = objValues;
        }

        private void OnActivate()
        {
            if (!mblnIsActivating)
            {
                mblnIsActivating = true;
                CUtils.UpdateExecute(
                    this,
                    delegate
                    {
                        if (this.Controls.Count == 0)
                        {
                            //Кнопки не сохраняются вместе с файлом, при повторном открытии приходится перевставлять.
                            ButtonAdd(XStartGet(0), YStartGet());
                            if ((CClientData.GameMode == ClientGameModeEnum.TeamByTeam) ||
                                (CClientData.GameMode == ClientGameModeEnum.OneToOne))
                            {
                                ButtonAdd(XStartGet(1), YStartGet());
                            }
                        }

                        List<Server.SParticipantTotalsForClient> lstTotals = ThisWorkbook.DataForPlacesGet();

                        List<Server.SParticipantTotalsForClient> lstTotals1 = lstTotals.
                            Where(x => (x.TeamIndex == 0)).
                            OrderBy(x => x.Place).
                            ToList();

                        Update(0, lstTotals1);
                        if ((CClientData.GameMode == ClientGameModeEnum.TeamByTeam) ||
                            (CClientData.GameMode == ClientGameModeEnum.OneToOne))
                        {
                            lstTotals1 = lstTotals.
                                Where(x => (x.TeamIndex == 1)).
                                OrderBy(x=> x.Place).
                                ToList();
                            Update(1, lstTotals1);
                        }
                    });
                mblnIsActivating = false;
            }
        }

        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.ActivateEvent += new Microsoft.Office.Interop.Excel.DocEvents_ActivateEventHandler(this.WSPlaces_ActivateEvent);
            this.Startup += new System.EventHandler(this.Sheet2_Startup);
            this.Shutdown += new System.EventHandler(this.Sheet2_Shutdown);
        }

        #endregion
    }
}
