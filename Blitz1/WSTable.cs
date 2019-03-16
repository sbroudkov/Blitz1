using System;
using System.Linq;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text;

namespace Blitz1.Client
{
    public partial class WSTable
    {
        private bool mblnUpdatesLock = false;

        internal void GroupCreate(
            int iGroupRow,
            int iGroupColumn)
        {
            int intXStart = XStartGet(iGroupColumn);
            int intYStart = YStartGet(iGroupRow);

            int intCount1 = CClientData.Participants1.Count;
            int intCount2 = 0;
            if ((CClientData.GameMode == ClientGameModeEnum.TeamByTeam) ||
                (CClientData.GameMode == ClientGameModeEnum.OneToOne))
            {
                intCount2 = CClientData.Participants2.Count;
            }
            SizesSet(intXStart, intYStart, intCount1 + intCount2);
            //Прорисовка рамок.
            BordersSet(intXStart, intYStart, intCount1, intCount2);
            FormatsSet(intXStart, intYStart, intCount1, intCount2);
            FixedTextsSet(intXStart, intYStart, intCount1 + intCount2);
            VariableTextsSet(intXStart, intYStart, intCount1, intCount2);
            CellsMerge(intXStart, intYStart, intCount1, intCount2);
            PansFreeze(intXStart, intYStart);

            this.EnableSelection = Excel.XlEnableSelection.xlUnlockedCells;
            CUtils.InternalProtect(this);
        }

        internal void Init()
        {
            this.Change += WSTable_Change;
        }

        private void Sheet1_Startup(
            object sender,
            EventArgs e)
        {
        }

        private void Sheet1_Shutdown(
            object sender,
            EventArgs e)
        {
        }

        private void WSTable_Change(Excel.Range iTarget)
        {
            if (mblnUpdatesLock)
            {
                return;
            }

            int intCount1 = CClientData.Participants1.Count;
            int intCount2 = 0;
            if ((CClientData.GameMode == ClientGameModeEnum.TeamByTeam) ||
                (CClientData.GameMode == ClientGameModeEnum.OneToOne))
            {
                intCount2 = CClientData.Participants2.Count;
            }

            try
            {
                CUtils.UpdateExecute(
                this,
                delegate
                {
                    mblnUpdatesLock = true;

                    foreach (Excel.Range clsCell in iTarget.Cells)
                    {
                        CellUpdate(clsCell, intCount1, intCount2);
                    }
                });
            }
            finally
            {
                mblnUpdatesLock = false;
            };
        }

        private int XStartGet(int iGroupColumn)
        {
            return (1 + //Разделитель
            1 + //Колонка команд
            2 * CClientData.Participants1.Count + //результаты игр
            3 + //Забито, пропущено, разница
            3 + //В/Н/П
            1 + //Очки
            1 + //Игры
            1 //Место
            ) * iGroupColumn + //Индекс групповой колонки
            1 + // Разделитель
            1; //Нумерация с 1
        }

        private int YStartGet(int iGroupRow)
        {
            return (1 + //Разделитель
            1 + //Заголовок команд
            CClientData.Participants1.Count //Команды
            ) * iGroupRow + //Индекс групповой строки
            +1 +// Разделитель
            1; //Нумерация с 1
        }

        private void SizesSet(
            int iX0,
            int iY0,
            int iCount)
        {
            Columns[iX0 - 1].ColumnWidth = CViewConst.SEPARATOR_COLUMN_WIDTH;
            Columns[iX0].ColumnWidth = CViewConst.PARTICIPANTS_COLUMN_WIDTH;
            for (int i = 0; i < 2 * iCount + CViewConst.EXTRA_COLUMNS; i++)
            {
                if (i == 2 * iCount + CViewConst.EXTRA_COLUMNS + CViewConst.COLUMN_COMMENT_OFFSET - 1)
                {
                    Columns[iX0 + 1 + i].ColumnWidth = CViewConst.COMMENT_COLUMN_WIDTH;
                }
                else if ((i == 2 * iCount + CViewConst.EXTRA_COLUMNS + CViewConst.COLUMN_PLUS_OFFSET  - 1) ||
                    (i == 2 * iCount + CViewConst.EXTRA_COLUMNS + CViewConst.COLUMN_MINUS_OFFSET - 1) ||
                    (i == 2 * iCount + CViewConst.EXTRA_COLUMNS + CViewConst.COLUMN_DIFF_OFFSET - 1) ||
                    (i == 2 * iCount + CViewConst.EXTRA_COLUMNS + CViewConst.COLUMN_MATCHES_OFFSET - 1))
                {
                    Columns[iX0 + 1 + i].ColumnWidth = CViewConst.WIDE_SCORE_COLUMN_WIDTH;
                }
                else
                {
                    Columns[iX0 + 1 + i].ColumnWidth = CViewConst.SCORE_COLUMN_WIDTH;
                }
            }

            Rows[iY0 - 1].RowHeight = CViewConst.SEPARATOR_ROW_HEIGHT;
            Rows[iY0].RowHeight = CViewConst.PARTICIPANT_CAPTION_ROW_HEIGHT;

            int intRowCount = iCount;
            if (CClientData.GameMode== ClientGameModeEnum.OneToOne)
            {
                intRowCount *= CClientData.MatchesInPair;
            }
            for (int i = 0; i < intRowCount; i++)
            {
                Rows[iY0 + 1 + i].RowHeight = CViewConst.PARTICIPANT_ROW_HEIGHT;
            }
        }

        private void BordersSet(
            int iX0,
            int iY0,
            int iCount1,
            int iCount2)
        {
            //Конец таблицы.
            int intX1 = iX0 + CViewConst.EXTRA_COLUMNS + 2 * (iCount1 + iCount2);
            int intY1 = iY0 + (iCount1 + iCount2) * CClientData.MatchesInPair;

            //Рамка колонки команд.
            CUtils.BoxSet(CUtils.ColumnGet(this, iY0, intY1, iX0), Excel.XlBorderWeight.xlMedium);

            //Рамка заголовка (строки команд).
            CUtils.BoxSet(CUtils.RowGet(this, iY0, iX0, intX1), Excel.XlBorderWeight.xlMedium);

            //Вертикальные рамки команд плюс рамка забитых/пропущенных мячей (по 2 ячейки в строке).
            for (int i = 0; i < (iCount1 + iCount2) + 1; i++)
            {
                CUtils.BorderSet(
                    CUtils.ColumnGet(this, iY0, intY1, iX0 + 2 + 2 * i),
                    new List<Excel.XlBordersIndex> { Excel.XlBordersIndex.xlEdgeRight },
                    new List<Excel.XlLineStyle> { Excel.XlLineStyle.xlContinuous },
                    new List<Excel.XlBorderWeight> { Excel.XlBorderWeight.xlThin });
            }

            //Вертикальные рамки колонки разницы.
            CUtils.BorderSet(
                CUtils.ColumnGet(this, iY0, intY1, intX1 + CViewConst.COLUMN_DIFF_OFFSET),
                new List<Excel.XlBordersIndex> { Excel.XlBordersIndex.xlEdgeRight },
                new List<Excel.XlLineStyle> { Excel.XlLineStyle.xlContinuous },
                new List<Excel.XlBorderWeight> { Excel.XlBorderWeight.xlThin });

            //Вертикальные рамки колонок В/Н/П, очки, игры, место
            for (int i = 0; i < -CViewConst.COLUMN_DIFF_OFFSET; i++) // 
            {
                CUtils.BorderSet(
                    CUtils.ColumnGet(this, iY0, intY1, intX1 + CViewConst.COLUMN_WIN_OFFSET + i),
                    new List<Excel.XlBordersIndex> { Excel.XlBordersIndex.xlEdgeRight },
                    new List<Excel.XlLineStyle> { Excel.XlLineStyle.xlContinuous },
                    new List<Excel.XlBorderWeight> { Excel.XlBorderWeight.xlThin });
            }

            //Рамка итогов.
            CUtils.BoxSet(
                CUtils.RectGet(this, iY0, intX1 - CViewConst.EXTRA_COLUMNS + 1, intY1, intX1),
                Excel.XlBorderWeight.xlMedium);
            CUtils.BoxSet(
                CUtils.RectGet(this, iY0, intX1 + CViewConst.COLUMN_WIN_OFFSET, intY1, intX1 + CViewConst.COLUMN_LOST_OFFSET),
                Excel.XlBorderWeight.xlMedium);
            CUtils.BoxSet(
                CUtils.ColumnGet(this, iY0, intY1, intX1 + CViewConst.COLUMN_COMMENT_OFFSET),
                Excel.XlBorderWeight.xlMedium);

            //Горизонтальные рамки команд.
            for (int i = 0; i < (iCount1 + iCount2); i++)
            {
                CUtils.BorderSet(
                    CUtils.RowGet(this, iY0 + 1 + CClientData.MatchesInPair * (i + 1) - 1, iX0, intX1),
                    new List<Excel.XlBordersIndex> { Excel.XlBordersIndex.xlEdgeBottom },
                    new List<Excel.XlLineStyle> { Excel.XlLineStyle.xlContinuous },
                    new List<Excel.XlBorderWeight> { Excel.XlBorderWeight.xlThin });
            }

            //Рамки счета матчей и итоговой разницы.
            switch (CClientData.GameMode)
            {
                case ClientGameModeEnum.Round:
                    for (int i = 0; i < iCount1 + 1; i++)
                    {
                        for (int j = 0; j < iCount1; j++)
                        {
                            if (i != j)
                            {
                                CUtils.BorderSet(
                                    Cells[iY0 + 1 + j, iX0 + 1 + 2 * i],
                                    new List<Excel.XlBordersIndex> { Excel.XlBordersIndex.xlEdgeRight },
                                    new List<Excel.XlLineStyle> { Excel.XlLineStyle.xlDot },
                                    new List<Excel.XlBorderWeight> { Excel.XlBorderWeight.xlHairline });
                            }
                        }
                    }
                    break;
                case ClientGameModeEnum.TeamByTeam:
                    //Верхняя правая часть.
                    for (int i = 0; i < iCount2; i++)
                    {
                        for (int j = 0; j < iCount1; j++)
                        {
                            CUtils.BorderSet(
                                Cells[iY0 + 1 + j, iX0 + 1 + 2 * (iCount1 + i)],
                                new List<Excel.XlBordersIndex> { Excel.XlBordersIndex.xlEdgeRight },
                                new List<Excel.XlLineStyle> { Excel.XlLineStyle.xlDot },
                                new List<Excel.XlBorderWeight> { Excel.XlBorderWeight.xlHairline });
                        }
                    }

                    //Нижняя левая часть.
                    for (int i = 0; i < iCount1; i++)
                    {
                        for (int j = 0; j < iCount2; j++)
                        {
                            CUtils.BorderSet(
                                Cells[iY0 + 1 + iCount1 + j, iX0 + 1 + 2 * i],
                                new List<Excel.XlBordersIndex> { Excel.XlBordersIndex.xlEdgeRight },
                                new List<Excel.XlLineStyle> { Excel.XlLineStyle.xlDot },
                                new List<Excel.XlBorderWeight> { Excel.XlBorderWeight.xlHairline });
                        }
                    }

                    //Итоговый счет.
                    for (int j = 0; j < iCount1 + iCount2; j++)
                    {
                        CUtils.BorderSet(
                            Cells[iY0 + 1 + j, iX0 + 1 + 2 * (iCount1 + iCount2)],
                            new List<Excel.XlBordersIndex> { Excel.XlBordersIndex.xlEdgeRight },
                            new List<Excel.XlLineStyle> { Excel.XlLineStyle.xlDot },
                            new List<Excel.XlBorderWeight> { Excel.XlBorderWeight.xlHairline });
                    }
                    break;
                case ClientGameModeEnum.OneToOne:
                    int intCount = iCount1; //или iCount2 - в режиме "по парам" они должны быть равны.

                    //Верхняя правая часть - диагональные элементы.
                    for (int i = 0; i < intCount; i++)
                    {
                        for (int j = 0; j < CClientData.MatchesInPair; j++)
                        {
                            CUtils.BorderSet(
                                Cells[iY0 + 1 + CClientData.MatchesInPair * i + j, iX0 + 1 + 2 * (iCount1 + i)],
                                new List<Excel.XlBordersIndex> { Excel.XlBordersIndex.xlEdgeRight },
                                new List<Excel.XlLineStyle> { Excel.XlLineStyle.xlDot },
                                new List<Excel.XlBorderWeight> { Excel.XlBorderWeight.xlHairline });
                        }
                    }

                    //Нижняя левая часть - диагональные элементы.
                    for (int i = 0; i < intCount; i++)
                    {
                        for (int j = 0; j < CClientData.MatchesInPair; j++)
                        {
                            CUtils.BorderSet(
                                Cells[iY0 + 1 + CClientData.MatchesInPair * (iCount1 + i) + j, iX0 + 1 + 2 * i],
                                new List<Excel.XlBordersIndex> { Excel.XlBordersIndex.xlEdgeRight },
                                new List<Excel.XlLineStyle> { Excel.XlLineStyle.xlDot },
                                new List<Excel.XlBorderWeight> { Excel.XlBorderWeight.xlHairline });
                        }
                    }

                    //Итоговый счет.
                    for (int i = 0; i < iCount1 + iCount2; i++)
                    {
                        for (int j = 0; j < CClientData.MatchesInPair; j++)
                        {
                            CUtils.BorderSet(
                            Cells[iY0 + 1 + CClientData.MatchesInPair * i + j, iX0 + 1 + 2 * (iCount1 + iCount2)],
                            new List<Excel.XlBordersIndex> { Excel.XlBordersIndex.xlEdgeRight },
                            new List<Excel.XlLineStyle> { Excel.XlLineStyle.xlDot },
                            new List<Excel.XlBorderWeight> { Excel.XlBorderWeight.xlHairline });
                        }
                    }
                    break;
                default:
                    throw new Exception("Нераспознанное значение нумератора ClientGameMode.");
            }

            //Линии разделения команд.
            if ((CClientData.GameMode == ClientGameModeEnum.TeamByTeam) ||
                (CClientData.GameMode == ClientGameModeEnum.OneToOne))
            {
                CUtils.BorderSet(
                    CUtils.RowGet(this, iY0 + iCount1 * CClientData.MatchesInPair, iX0, intX1),
                    new List<Excel.XlBordersIndex> { Excel.XlBordersIndex.xlEdgeBottom },
                    new List<Excel.XlLineStyle> { Excel.XlLineStyle.xlContinuous },
                    new List<Excel.XlBorderWeight> { Excel.XlBorderWeight.xlThick });
                CUtils.BorderSet(
                    CUtils.ColumnGet(this, iY0, intY1, iX0 + 2 * iCount1),
                    new List<Excel.XlBordersIndex> { Excel.XlBordersIndex.xlEdgeRight },
                    new List<Excel.XlLineStyle> { Excel.XlLineStyle.xlContinuous },
                    new List<Excel.XlBorderWeight> { Excel.XlBorderWeight.xlThick });
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
            int intX1 = iX0 + CViewConst.EXTRA_COLUMNS + 2 * iCount;
            int intY1 = iY0 + iCount * CClientData.MatchesInPair;

            Cells[iY0, intX1 - CViewConst.EXTRA_COLUMNS + 1].Value2 = "Разность";
            Cells[iY0, intX1 + CViewConst.COLUMN_WIN_OFFSET].Value2 = "В";
            Cells[iY0, intX1 + CViewConst.COLUMN_DRAW_OFFSET].Value2 = "Н";
            Cells[iY0, intX1 + CViewConst.COLUMN_LOST_OFFSET].Value2 = "П";
            Cells[iY0, intX1 + CViewConst.COLUMN_POINTS_OFFSET].Value2 = "Очков";
            Cells[iY0, intX1 + CViewConst.COLUMN_MATCHES_OFFSET].Value2 = "Игр";
            Cells[iY0, intX1 + CViewConst.COLUMN_PLACES_OFFSET].Value2 = "Место";
            Cells[iY0, intX1 + CViewConst.COLUMN_COMMENT_OFFSET].Value2 = "Комментарий";

            Cells[intY1 + 1, intX1 + CViewConst.COLUMN_MATCHES_OFFSET - 1].Value2 = "Всего:";
            Cells[intY1 + 2, intX1 + CViewConst.COLUMN_MATCHES_OFFSET - 1].Value2 = "Сыграно:";
            Cells[intY1 + 3, intX1 + CViewConst.COLUMN_MATCHES_OFFSET - 1].Value2 = "Осталось:";
        }

        private void VariableTextsSet(
            int iX0,
            int iY0,
            int iCount1,
            int iCount2)
        {
            int intRowCount = (iCount1 + iCount2) * CClientData.MatchesInPair;

            Cells[iY0 + 1, iX0 - 1].Value2 = CClientData.TeamName1;

            for (int i = 0; i < iCount1; i++)
            {
                Cells[iY0 + 1 + CClientData.MatchesInPair * i, iX0].Value2 = CClientData.Participants1[i];
                Cells[iY0, iX0 + 1 + 2 * i].Value2 = CClientData.Participants1[i];
            }

            if ((CClientData.GameMode == ClientGameModeEnum.TeamByTeam) ||
                (CClientData.GameMode == ClientGameModeEnum.OneToOne))
            {
                Cells[iY0 + 1 + CClientData.MatchesInPair * iCount1, iX0 - 1].Value2 = CClientData.TeamName2;

                for (int i = 0; i < iCount2; i++)
                {
                    Cells[iY0 + 1 + CClientData.MatchesInPair * (iCount1 + i), iX0].Value2 = CClientData.Participants2[i];
                    Cells[iY0, iX0 + 1 + 2 * (iCount1 + i)].Value2 = CClientData.Participants2[i];
                }
            }

            ShowTotalMatches(iX0, iY0, iCount1, iCount2);
        }

        private void CellsMerge(
            int iX0,
            int iY0,
            int iCount1,
            int iCount2)
        {
            //Конец таблицы.
            int intX1 = iX0 + CViewConst.EXTRA_COLUMNS + 2 * (iCount1 + iCount2);
            int intY1 = iY0 + (iCount1 + iCount2) * CClientData.MatchesInPair;

            CUtils.UpdateExecute(
                this,
                delegate
                {
                    //Название команды.
                    CUtils.ColumnGet(
                        this, 
                        iY0 + 1, 
                        iY0 + 1 + CClientData.MatchesInPair * iCount1 - 1, 
                        iX0 - 1).Merge(false);
                    if ((CClientData.GameMode == ClientGameModeEnum.TeamByTeam) ||
                        (CClientData.GameMode == ClientGameModeEnum.OneToOne))
                    {
                        CUtils.ColumnGet(
                            this, 
                            iY0 + 1 + CClientData.MatchesInPair * iCount1, 
                            iY0 + 1 + CClientData.MatchesInPair * (iCount1 + iCount2) - 1, 
                            iX0 - 1).Merge(false);
                    }

                    //Колонки участников
                    for (int i = 0; i < iCount1 + iCount2; i++)
                    {
                        CUtils.RowGet(this, iY0, iX0 + 1 + 2 * i, iX0 + 2 + 2 * i).Merge(false);
                    }

                    //Заголовок разницы мячей.
                    CUtils.RowGet(
                        this,
                        iY0,
                        intX1 - CViewConst.EXTRA_COLUMNS + 1,
                        intX1 + CViewConst.COLUMN_DIFF_OFFSET).Merge(false);

                    if (CClientData.GameMode == ClientGameModeEnum.OneToOne)
                    {
                        for (int i = 0; i < iCount1 + iCount2; i++)
                        {
                            CUtils.ColumnGet(
                                this,
                                iY0 + 1 + CClientData.MatchesInPair * i,
                                iY0 + 1 + CClientData.MatchesInPair * (i + 1) - 1,
                                iX0).Merge(false);

                            CUtils.ColumnGet(
                                this,
                                iY0 + 1 + CClientData.MatchesInPair * i,
                                iY0 + 1 + CClientData.MatchesInPair * (i + 1) - 1,
                                intX1 + CViewConst.COLUMN_PLUS_OFFSET).Merge(false);
                            CUtils.ColumnGet(
                                this,
                                iY0 + 1 + CClientData.MatchesInPair * i,
                                iY0 + 1 + CClientData.MatchesInPair * (i + 1) - 1,
                                intX1 + CViewConst.COLUMN_MINUS_OFFSET).Merge(false);
                            CUtils.ColumnGet(
                                this,
                                iY0 + 1 + CClientData.MatchesInPair * i,
                                iY0 + 1 + CClientData.MatchesInPair * (i + 1) - 1,
                                intX1 + CViewConst.COLUMN_DIFF_OFFSET).Merge(false);
                            CUtils.ColumnGet(
                                this,
                                iY0 + 1 + CClientData.MatchesInPair * i,
                                iY0 + 1 + CClientData.MatchesInPair * (i + 1) - 1,
                                intX1 + CViewConst.COLUMN_WIN_OFFSET).Merge(false);
                            CUtils.ColumnGet(
                                this,
                                iY0 + 1 + CClientData.MatchesInPair * i,
                                iY0 + 1 + CClientData.MatchesInPair * (i + 1) - 1,
                                intX1 + CViewConst.COLUMN_DRAW_OFFSET).Merge(false);
                            CUtils.ColumnGet(
                                this,
                                iY0 + 1 + CClientData.MatchesInPair * i,
                                iY0 + 1 + CClientData.MatchesInPair * (i + 1) - 1,
                                intX1 + CViewConst.COLUMN_LOST_OFFSET).Merge(false);
                            CUtils.ColumnGet(
                                this,
                                iY0 + 1 + CClientData.MatchesInPair * i,
                                iY0 + 1 + CClientData.MatchesInPair * (i + 1) - 1,
                                intX1 + CViewConst.COLUMN_POINTS_OFFSET).Merge(false);
                            CUtils.ColumnGet(
                                this,
                                iY0 + 1 + CClientData.MatchesInPair * i,
                                iY0 + 1 + CClientData.MatchesInPair * (i + 1) - 1,
                                intX1 + CViewConst.COLUMN_MATCHES_OFFSET).Merge(false);
                            CUtils.ColumnGet(
                                this,
                                iY0 + 1 + CClientData.MatchesInPair * i,
                                iY0 + 1 + CClientData.MatchesInPair * (i + 1) - 1,
                                intX1 + CViewConst.COLUMN_PLACES_OFFSET).Merge(false);
                        }
                    }
                });
        }

        private void FormatsSet(
            int iX0,
            int iY0,
            int iCount1,
            int iCount2)
        {
            //Конец таблицы.
            int intX1 = iX0 + CViewConst.EXTRA_COLUMNS + 2 * (iCount1 + iCount2);
            int intY1 = iY0 + (iCount1 + iCount2) * CClientData.MatchesInPair;

            Excel.Range clsRange = null;

            //Название команды.
            clsRange = Cells[iY0 + 1, iX0 - 1];
            clsRange.Font.Bold = true;
            clsRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            clsRange.Orientation = 90;
            clsRange.NumberFormat = "@";

            if ((CClientData.GameMode == ClientGameModeEnum.TeamByTeam) ||
                (CClientData.GameMode == ClientGameModeEnum.OneToOne))
            {
                clsRange = Cells[iY0 + 1 + CClientData.MatchesInPair * iCount1, iX0 - 1];
                clsRange.Font.Bold = true;
                clsRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                clsRange.Orientation = 90;
                clsRange.NumberFormat = "@";
            }

            //Счет
            clsRange = Cells[iY0, iX0];
            clsRange.Font.Bold = true;
            clsRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
            clsRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            clsRange.Orientation = 0;
            clsRange.WrapText = true;

            //Верхний левый угол таблицы.
            Excel.Range clsGrayRange = Cells[iY0, iX0];
            Excel.Range clsUnlockedRange = null;

            //Диагональ таблицы.
            for (int i = 0; i < CClientData.MatchesInPair * (iCount1 + iCount2); i++)
            {
                for (int j = 0; j < 2 * (iCount1 + iCount2); j++)
                {
                    
                    clsRange = Cells[iY0 + 1 + i, iX0 + 1 + j];
                    if (IsInWorkingArea(i, j, iCount1, iCount2))
                    {
                        if (clsUnlockedRange==null)
                        {
                            clsUnlockedRange = clsRange;
                        }
                        else
                        {
                            clsUnlockedRange = Application.Union(clsUnlockedRange, clsRange);
                        }
                    }
                    else
                    {
                        clsGrayRange = Application.Union(clsGrayRange, clsRange);
                    }
                }
            }

            clsUnlockedRange.Locked = false;
            clsGrayRange.Interior.Color = CColorConst.DIAGONAL_BACKGROUND;

            //Заголовки строк участников.
            clsRange = CUtils.ColumnGet(this, iY0 + 1, intY1, iX0);
            clsRange.Font.Bold = true;
            clsRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            if (CClientData.MatchesInPair > 1)
            {
                clsRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            }
            clsRange.NumberFormat = "@";

            //Заголовки столбцов участников.
            clsRange = CUtils.RowGet(this, iY0, iX0 + 1, intX1);
            clsRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            clsRange.Font.Bold = true;
            clsRange.Orientation = 90;
            clsRange.NumberFormat = "@";

            //Итоги
            CUtils.ColumnGet(this, iY0 + 1, intY1, intX1 + CViewConst.COLUMN_WIN_OFFSET).Font.Color = CColorConst.WIN;
            CUtils.ColumnGet(this, iY0 + 1, intY1, intX1 + CViewConst.COLUMN_DRAW_OFFSET).Font.Color = CColorConst.DRAW;
            CUtils.ColumnGet(this, iY0 + 1, intY1, intX1 + CViewConst.COLUMN_LOST_OFFSET).Font.Color = CColorConst.LOSE;
            CUtils.RectGet(
                this,
                iY0 + 1,
                intX1 + CViewConst.COLUMN_WIN_OFFSET,
                intY1,
                intX1 + CViewConst.COLUMN_PLACES_OFFSET).Font.Bold = true;
            if (CClientData.MatchesInPair > 1)
            {
                CUtils.RectGet(
                    this,
                    iY0 + 1,
                    intX1 + CViewConst.COLUMN_PLUS_OFFSET,
                    intY1,
                    intX1 + CViewConst.COLUMN_PLACES_OFFSET).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            }

            Cells[intY1 + 1, intX1 + CViewConst.COLUMN_MATCHES_OFFSET - 1].Font.Bold = true;
            Cells[intY1 + 1, intX1 + CViewConst.COLUMN_MATCHES_OFFSET - 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            Cells[intY1 + 1, intX1 + CViewConst.COLUMN_MATCHES_OFFSET].Font.Bold = true;

            Cells[intY1 + 2, intX1 + CViewConst.COLUMN_MATCHES_OFFSET - 1].Font.Bold = true;
            Cells[intY1 + 2, intX1 + CViewConst.COLUMN_MATCHES_OFFSET - 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            Cells[intY1 + 2, intX1 + CViewConst.COLUMN_MATCHES_OFFSET].Font.Bold = true;

            Cells[intY1 + 3, intX1 + CViewConst.COLUMN_MATCHES_OFFSET - 1].Font.Bold = true;
            Cells[intY1 + 3, intX1 + CViewConst.COLUMN_MATCHES_OFFSET - 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            Cells[intY1 + 3, intX1 + CViewConst.COLUMN_MATCHES_OFFSET].Font.Bold = true;

            //Комментарии.
            CUtils.ColumnGet(this, iY0 + 1, intY1, intX1 + CViewConst.COLUMN_COMMENT_OFFSET).Locked = false;
            CUtils.ColumnGet(this, iY0 + 1, intY1, intX1 + CViewConst.COLUMN_COMMENT_OFFSET).NumberFormat = "@";
        }

        private void PansFreeze(
            int iX0,
            int iY0)
        {
            Excel.Range clsFirstCell = this.Cells[iY0 + 1, iX0 + 1];
            clsFirstCell.Activate();
            this.Application.ActiveWindow.FreezePanes = true;
        }

        private bool IsInWorkingArea(
            int iRow,
            int iColumn,
            int iCount1,
            int iCount2)
        {
            bool blnResult = false;

            if ((iRow >= 0) &&
                (iColumn >= 0))
            {
                int intParticipantRow = iRow / CClientData.MatchesInPair;
                int intParticipantColumn = iColumn / 2;

                if ((intParticipantRow < (iCount1 + iCount2)) &&
                    (intParticipantColumn < (iCount1 + iCount2)))
                {
                    switch (CClientData.GameMode)
                    {
                        case ClientGameModeEnum.Round:
                            blnResult = (intParticipantRow != intParticipantColumn);
                            break;
                        case ClientGameModeEnum.TeamByTeam:
                            blnResult =
                                ((intParticipantRow < iCount1) &&
                                (intParticipantColumn >= iCount1)) ||
                                ((intParticipantRow >= iCount1) &&
                                (intParticipantColumn < iCount1));
                            break;
                        case ClientGameModeEnum.OneToOne:
                            blnResult =
                                ((intParticipantRow < iCount1) &&
                                (intParticipantColumn >= iCount1) &&
                                (intParticipantRow == (intParticipantColumn - iCount1))) ||
                                ((intParticipantRow >= iCount1) &&
                                (intParticipantColumn < iCount1) &&
                                (intParticipantRow == (intParticipantColumn + iCount1)));
                            break;
                        default:
                            throw new Exception("Нераспознанное значение нумератора ClientGameMode.");
                    }
                }
            }
            return blnResult;
        }

        private bool IsInNameArea(
            int iRow,
            int iColumn,
            int iCount1,
            int iCount2)
        {
            bool blnResult = false;

            int intParticipantRow = iRow / CClientData.MatchesInPair;
            int intParticipantColumn = iColumn / 2;

            blnResult =
                ((iColumn == -1) &&
                (iRow >= 0) &&
                (intParticipantRow < (iCount1 + iCount2))) ||
                ((iRow == -1) &&
                (iColumn >= 0) &&
                (intParticipantColumn < (iCount1 + iCount2)));

            return blnResult;
        }

        private bool IsInCommandNameArea(
            int iRow,
            int iColumn,
            int iCount1,
            int iCount2)
        {
            bool blnResult = false;

            return blnResult;
        }

        private void CellUpdate(
            Excel.Range iTarget,
            int iCount1,
            int iCount2)
        {
            //Индекс групповой строки.
            int intGroupRow =
                (iTarget.Row - 1) / //Нумерация с 1
                (CClientData.MatchesInPair * (iCount1 + iCount2) + 2); //Разделитель и заголовок.

            //Индекс групповой колонки.
            int intGroupColumn =
                (iTarget.Column - 1) / //Нумерация с 1
                (2 * (iCount1 + iCount2) + 2 + CViewConst.EXTRA_COLUMNS);

            if ((intGroupRow == 0) &&
                (intGroupColumn == 0))
            {
                int intRow =
                    (iTarget.Row - 1) % //Нумерация с 1
                    (CClientData.MatchesInPair * (iCount1 + iCount2) + 2) - //Разделитель и заголовок.
                    2; //Разделитель и заголовок.

                int intColumn =
                    (((iTarget.Column - 1) % //Нумерация с 1
                    (2 * (iCount1 + iCount2) + 2 + CViewConst.EXTRA_COLUMNS)) - 2); //Разделитель и заголовок.

                int intXStart = XStartGet(intGroupColumn);
                int intYStart = YStartGet(intGroupRow);

                if (IsInWorkingArea(intRow, intColumn, iCount1, iCount2))
                {
                    WorkingAreaUpdate(iTarget, intXStart, intYStart, intRow, intColumn, iCount1, iCount2);
                }
                else if (IsInNameArea(intRow, intColumn, iCount1, iCount2))
                {
                    NameUpdate(iTarget, intXStart, intYStart, intRow, intColumn, iCount1, iCount2);
                }
                else if (IsInCommandNameArea(intRow, intColumn, iCount1, iCount2))
                {
                    CommandNameUpdate(iTarget, intXStart, intYStart, intRow, intColumn, iCount1, iCount2);
                }
            }
        }

        private void WorkingAreaUpdate(
            Excel.Range iTarget,
            int iX0,
            int iY0,
            int iRow,
            int iColumn,
            int iCount1,
            int iCount2)
        {
            int intMatchNumber = iRow % CClientData.MatchesInPair;
            iRow /= CClientData.MatchesInPair; //Номер своей команды.

            bool blnFirst = ((iColumn % 2) == 0); //Первая или вторая ячейка для счета матча.
            iColumn /= 2; //Получить номер команды противника.

            int intRow = iY0 + iRow * CClientData.MatchesInPair + 1;
            int intColumn = iX0 + iColumn * 2 + 1;
            Excel.Range clsScoreRange = CUtils.RectGet(this, intRow, intColumn, intRow, intColumn + 1);
            object[,] aobjValues = clsScoreRange.Value2;

            int? intValue1 = null;
            int? intValue2 = null;

            //Индексы начинаются с 1
            if ((aobjValues[1, 1] == null) ||
                (aobjValues[1, 1].GetType() == typeof(double)))
            {
                double? dblValue = (double?)aobjValues[1, 1];
                if (dblValue.HasValue)
                {
                    intValue1 = (int?)dblValue.Value;
                }
            }
            else
            {
                intValue1 = int.MinValue;
            }

            if ((aobjValues[1, 2] == null) ||
                (aobjValues[1, 2].GetType() == typeof(double)))
            {
                double? dblValue = (double?)aobjValues[1, 2];
                if (dblValue.HasValue)
                {
                    intValue2 = (int?)dblValue.Value;
                }
            }
            else
            {
                intValue2 = int.MinValue;
            }

            int intParticipant1 = 0;
            int intParticipant2 = 0;
            switch (CClientData.GameMode)
            {
                case ClientGameModeEnum.Round:
                    intParticipant1 = iRow;
                    intParticipant2 = iColumn;
                    break;
                case ClientGameModeEnum.TeamByTeam:
                    if ((iColumn >= iCount1) &&
                        (iRow < iCount1))
                    {
                        intParticipant1 = iRow;
                        intParticipant2 = iColumn - iCount1;
                    }
                    else if ((iColumn < iCount1) &&
                        (iRow >= iCount1))
                    {
                        intParticipant1 = iRow - iCount1;
                        intParticipant2 = iColumn;
                    }
                    else
                    {
                        throw new Exception("Редактирование в закрытой области");
                    }

                    break;
                case ClientGameModeEnum.OneToOne:
                    throw new NotImplementedException("Обработка значения нумератора ClientGameModeEnum.OneToOne");
                    break;
                case ClientGameModeEnum.LeagueGroup:
                    throw new NotImplementedException("Обработка значения нумератора ClientGameModeEnum.LeagueGroup");
                    break;
                default:
                    throw new Exception("Нераспознанное значение нумератора ClientGameModeEnum");
            }
            ThisWorkbook.ScoreUpdate(new Server.SScoreUpdate(intParticipant1, intParticipant2, intMatchNumber, intValue1, intValue2));



            //////////int intRow1 = iColumn;
            //////////int intColumn1 = iRow * 2;
            //////////if (blnFirst)
            //////////{
            //////////    intColumn1++;
            //////////}

            //////////Excel.Range clsOtherRange = Cells[
            //////////    iY0 + intRow1 * CClientData.MatchesInPair + intMatchNumber + 1,
            //////////    iX0 + intColumn1 + 1];

            //////////string strValue1 = null;
            //////////if (iTarget.Value2 != null)
            //////////{
            //////////    strValue1 = iTarget.Value2.ToString();
            //////////}
            //////////string strValue2 = null;
            //////////if (clsOtherRange.Value2 != null)
            //////////{
            //////////    strValue2 = clsOtherRange.Value2.ToString();
            //////////}

            //////////if (strValue1 != strValue2)
            //////////{
            //////////    clsOtherRange.Value2 = iTarget.Value2;
            //////////}

            RowUpdate(iX0, iY0, iRow, iCount1, iCount2);
            RowUpdate(iX0, iY0, iColumn, iCount1, iCount2);

            PlacesSet(iX0, iY0, iCount1, iCount2);
            ShowTotalMatches(iX0, iY0, iCount1, iCount2);
            ShowScore(iX0, iY0);
        }

        private void ShowScore(
            int iX0, 
            int iY0)
        {
            Server.STotalsForClient clsTotals = ThisWorkbook.TotalsForClientGet();

            StringBuilder clsBuilder = new StringBuilder();

            if ((CClientData.GameMode == ClientGameModeEnum.TeamByTeam) ||
                (CClientData.GameMode == ClientGameModeEnum.OneToOne))
            {
                clsBuilder.Append("Баллы: ");
                clsBuilder.Append(clsTotals.Plus);
                clsBuilder.Append(":");
                clsBuilder.Append(clsTotals.Minus);
                clsBuilder.Append(" = ");
                clsBuilder.Append(clsTotals.Difference);
                clsBuilder.AppendLine();

                clsBuilder.Append("Очки: ");
                clsBuilder.Append(clsTotals.Wins);
                clsBuilder.Append("/");
                clsBuilder.Append(clsTotals.Draws);
                clsBuilder.Append("/");
                clsBuilder.Append(clsTotals.Loses);
                clsBuilder.Append(" = ");
                clsBuilder.Append(clsTotals.Points1);
                clsBuilder.Append(":");
                clsBuilder.Append(clsTotals.Points2);
                clsBuilder.AppendLine();
            }

            int intTotalPlayed = clsTotals.MatchesPlayed;

            clsBuilder.Append("Игры: ");
            clsBuilder.Append(intTotalPlayed);
            clsBuilder.Append(" из ");
            clsBuilder.Append(clsTotals.TotalMatches);
            clsBuilder.Append(" (");
            clsBuilder.Append(clsTotals.TotalMatches - intTotalPlayed);
            clsBuilder.Append(")");

            this.Cells[iY0, iX0].Value2 = clsBuilder.ToString();
        }

        private void RowUpdate(
            int iX0,
            int iY0,
            int iRow,
            int iCount1,
            int iCount2)
        {
            //Конец таблицы.
            int intX1 = iX0 + CViewConst.EXTRA_COLUMNS + 2 * (iCount1 + iCount2);

            Excel.Range clsRange = CUtils.RectGet(
                this,
                iY0 + 1 + iRow * CClientData.MatchesInPair,
                iX0 + 1,
                iY0 + 1 + (iRow + 1) * CClientData.MatchesInPair - 1,
                intX1);
            object[,] aobjValues = clsRange.Value2;

            //////////List<Server.SScoreUpdate> lstUpdates = new List<Server.SScoreUpdate>();

            //////////for (int i = 0; i < iCount1 + iCount2; i++)
            //////////{
            //////////    bool blnInWorkArea = IsInWorkingArea(
            //////////        iRow * CClientData.MatchesInPair,
            //////////        i * 2,
            //////////        iCount1,
            //////////        iCount2);
            //////////    if (blnInWorkArea)
            //////////    {
            //////////        for (int j = 0; j < CClientData.MatchesInPair; j++)
            //////////        {
            //////////            Excel.Range clsFirst = this.Cells[iY0 + 1 + iRow * CClientData.MatchesInPair + j, iX0 + 1 + 2 * i];
            //////////            Excel.Range clsSecond = clsFirst.Offset[0, 1];

            //////////            double? dblValue1 = null;
            //////////            double? dblValue2 = null;
            //////////            bool blnNotANumber1 = true;
            //////////            bool blnNotANumber2 = true;

            //////////            if ((aobjValues[1 + j, 1 + 2 * i] == null) ||
            //////////                (aobjValues[1 + j, 1 + 2 * i].GetType() == typeof(double)))
            //////////            {
            //////////                dblValue1 = (double?)aobjValues[1 + j, 1 + 2 * i];
            //////////                blnNotANumber1 = false;
            //////////            }
            //////////            if ((aobjValues[1 + j, 1 + 2 * i + 1] == null) ||
            //////////                (aobjValues[1 + j, 1 + 2 * i + 1].GetType() == typeof(double)))
            //////////            {
            //////////                dblValue2 = (double?)aobjValues[1 + j, 1 + 2 * i + 1];
            //////////                blnNotANumber2 = false;
            //////////            }

            //////////            int? intValue1 = null;
            //////////            int? intValue2 = null;

            //////////            if (dblValue1.HasValue && dblValue2.HasValue)
            //////////            {
            //////////                if (dblValue1.Value < dblValue2.Value)
            //////////                {
            //////////                    clsFirst.Font.Color = CColorConst.LOSE;
            //////////                    clsSecond.Font.Color = CColorConst.LOSE;
            //////////                }
            //////////                else if (dblValue1 > dblValue2.Value)
            //////////                {
            //////////                    clsFirst.Font.Color = CColorConst.WIN;
            //////////                    clsSecond.Font.Color = CColorConst.WIN;
            //////////                }
            //////////                else
            //////////                {
            //////////                    clsFirst.Font.Color = CColorConst.DRAW;
            //////////                    clsSecond.Font.Color = CColorConst.DRAW;
            //////////                }

            //////////                intValue1 = (int)dblValue1.Value;
            //////////                intValue2 = (int)dblValue2.Value;
            //////////            }
            //////////            else
            //////////            {
            //////////                if (blnNotANumber1)
            //////////                {
            //////////                    clsFirst.Font.Color = CColorConst.WRONG;
            //////////                }
            //////////                else
            //////////                {
            //////////                    clsFirst.Font.Color = CColorConst.NEUTRAL;
            //////////                    if (dblValue1.HasValue)
            //////////                    {
            //////////                        intValue1 = (int)dblValue1.Value;
            //////////                    }
            //////////                }
            //////////                if (blnNotANumber2)
            //////////                {
            //////////                    clsSecond.Font.Color = CColorConst.WRONG;
            //////////                }
            //////////                else
            //////////                {
            //////////                    clsSecond.Font.Color = CColorConst.NEUTRAL;
            //////////                    if (dblValue2.HasValue)
            //////////                    {
            //////////                        intValue2 = (int)dblValue2.Value;
            //////////                    }
            //////////                }
            //////////            }
            //////////            int intParticipant1 = 0;
            //////////            int intParticipant2 = 0;
            //////////            switch (CClientData.GameMode)
            //////////            {
            //////////                case ClientGameModeEnum.Round:
            //////////                    intParticipant1 = iRow;
            //////////                    intParticipant2 = i;
            //////////                    break;
            //////////                case ClientGameModeEnum.TeamByTeam:
            //////////                    if ((i >= iCount1) &&
            //////////                        (iRow < iCount1))
            //////////                    {
            //////////                        intParticipant1 = iRow;
            //////////                        intParticipant2 = i - iCount1;
            //////////                    }
            //////////                    else if ((i < iCount1) &&
            //////////                        (iRow >= iCount1))
            //////////                    {
            //////////                        intParticipant1 = iRow - iCount1;
            //////////                        intParticipant2 = i;
            //////////                    }
            //////////                    else
            //////////                    {
            //////////                        throw new Exception("Редактирование в закрытой области");
            //////////                    }

            //////////                    break;
            //////////                case ClientGameModeEnum.OneToOne:
            //////////                    throw new NotImplementedException("Обработка значения нумератора ClientGameModeEnum.OneToOne");
            //////////                    break;
            //////////                case ClientGameModeEnum.LeagueGroup:
            //////////                    throw new NotImplementedException("Обработка значения нумератора ClientGameModeEnum.LeagueGroup");
            //////////                    break;
            //////////                default:
            //////////                    throw new Exception("Нераспознанное значение нумератора ClientGameModeEnum");
            //////////            }
            //////////            lstUpdates.Add(new Server.SScoreUpdate(intParticipant1, intParticipant2, j, intValue1, intValue2));
            //////////        }
            //////////    }
            //////////}

            //////////ThisWorkbook.ScoreUpdate(lstUpdates);

            Server.SParticipantTotalsForClient clsTotals = null;
            if (iRow < iCount1)
            {
                clsTotals = ThisWorkbook.ParticipantTotalsGet(0, iRow);
            }
            else
            {
                clsTotals = ThisWorkbook.ParticipantTotalsGet(1, iRow - iCount1);
            }

            int intMatches = clsTotals.MatchesPlayed;

            aobjValues[1, intX1 - iX0 + CViewConst.COLUMN_PLUS_OFFSET] = clsTotals.Plus;
            aobjValues[1, intX1 - iX0 + CViewConst.COLUMN_MINUS_OFFSET] = clsTotals.Minus;
            aobjValues[1, intX1 - iX0 + CViewConst.COLUMN_DIFF_OFFSET] = clsTotals.Difference;
            aobjValues[1, intX1 - iX0 + CViewConst.COLUMN_WIN_OFFSET] = clsTotals.Wins;
            aobjValues[1, intX1 - iX0 + CViewConst.COLUMN_DRAW_OFFSET] = clsTotals.Draws;
            aobjValues[1, intX1 - iX0 + CViewConst.COLUMN_LOST_OFFSET] = clsTotals.Loses;
            aobjValues[1, intX1 - iX0 + CViewConst.COLUMN_POINTS_OFFSET] = clsTotals.Points;
            aobjValues[1, intX1 - iX0 + CViewConst.COLUMN_MATCHES_OFFSET] = clsTotals.MatchesPlayed;

            clsRange.Value2 = aobjValues;

            int intMaxMatches = 0;
            switch (CClientData.GameMode)
            {
                case ClientGameModeEnum.Round:
                    intMaxMatches = iCount1 - 1;
                    break;
                case ClientGameModeEnum.TeamByTeam:
                    if (iRow < iCount1)
                    {
                        intMaxMatches = iCount2;
                    }
                    else
                    {
                        intMaxMatches = iCount1;
                    }
                    break;
                case ClientGameModeEnum.OneToOne:
                    intMaxMatches = CClientData.MatchesInPair;
                    break;
                default:
                    throw new Exception("Нераспознанное значение нумератора ClientGameMode.");
            }
            if (clsTotals.MatchesPlayed == intMaxMatches)
            {
                this.Cells[
                    iY0 + 1 + iRow * CClientData.MatchesInPair,
                    intX1 + CViewConst.COLUMN_MATCHES_OFFSET].Interior.Color = CColorConst.FINISHED_BACKGROUND;
                this.Cells[
                    iY0 + 1 + iRow * CClientData.MatchesInPair, iX0].Interior.Color = CColorConst.FINISHED_BACKGROUND;
            }
            else
            {
                this.Cells[
                    iY0 + 1 + iRow * CClientData.MatchesInPair,
                    intX1 + CViewConst.COLUMN_MATCHES_OFFSET].Interior.Color = CColorConst.NEUTRAL_BACKGROUND;
                this.Cells[
                    iY0 + 1 + iRow * CClientData.MatchesInPair, iX0].Interior.Color = CColorConst.NEUTRAL_BACKGROUND;
            }
        }

        private void NameUpdate(
            Excel.Range iTarget,
            int iX0,
            int iY0,
            int iRow,
            int iColumn,
            int iCount1,
            int iCount2)
        {
            int intRow1 = 0;
            int intColumn1 = 0;
            int intParticipantIndex = 0;
            if (iRow == -1)
            {
                intParticipantIndex = iColumn / 2;
                intRow1 = intParticipantIndex * CClientData.MatchesInPair;
                intColumn1 = -1;
            }
            else if (iColumn == -1)
            {
                intParticipantIndex = iRow / CClientData.MatchesInPair;
                intRow1 = -1;
                intColumn1 = intParticipantIndex * 2;
            }
            Excel.Range clsOtherRange = Cells[iY0 + intRow1 + 1, iX0 + intColumn1 + 1];
            string strValue1 = null;
            if (iTarget.Value2 != null)
            {
                strValue1 = iTarget.Value2.ToString();
            }
            string strValue2 = null;
            if (clsOtherRange.Value2 != null)
            {
                strValue2 = clsOtherRange.Value2.ToString();
            }

            string strName = null;
            if (intParticipantIndex < iCount1)
            {
                strName = ThisWorkbook.ParticipantNameUpdate(0, intParticipantIndex, strValue1);
                CClientData.Participants1[intParticipantIndex] = strName;
            }
            else if (intParticipantIndex < (iCount1 + iCount2))
            {
                strName = ThisWorkbook.ParticipantNameUpdate(0, intParticipantIndex, strValue1);
                CClientData.Participants2[intParticipantIndex - iCount1] = strName;
            }

            iTarget.Value2 = strName;
            clsOtherRange.Value2 = strName;
        }

        private void CommandNameUpdate(
            Excel.Range iTarget,
            int iX0,
            int iY0,
            int iRow,
            int iColumn,
            int iCount1,
            int iCount2)
        {
        }

        private void PlacesSet(
            int iX0,
            int iY0,
            int iCount1,
            int iCount2)
        {
            //Конец таблицы.
            int intX1 = iX0 + CViewConst.EXTRA_COLUMNS + 2 * (iCount1 + iCount2);

            //[Индекс в списке, количество очков], сортировка по второму полю.
            List<Tuple<int, int>> lstSorted = Server.CCommonData.ExternalSortedByPlaceIndicesGet(Server.CCommonData.Participants1);

            Excel.Range clsRange = CUtils.ColumnGet(
                this,
                iY0 + 1,
                iY0 + 1 + (iCount1 + iCount2) * CClientData.MatchesInPair,
                intX1 + CViewConst.COLUMN_PLACES_OFFSET);
            object[,] objValues = clsRange.Value2;

            foreach (Tuple<int, int> clsIndex in lstSorted)
            {
                objValues[1 + clsIndex.Item1 * CClientData.MatchesInPair, 1] = clsIndex.Item2;
            }

            if ((CClientData.GameMode == ClientGameModeEnum.TeamByTeam) ||
                (CClientData.GameMode == ClientGameModeEnum.OneToOne))
            {
                lstSorted = Server.CCommonData.ExternalSortedByPlaceIndicesGet(Server.CCommonData.Participants2);

                foreach (Tuple<int, int> clsIndex in lstSorted)
                {
                    objValues[1 + (clsIndex.Item1 + iCount1) * CClientData.MatchesInPair, 1] = clsIndex.Item2;
                }
            }

            clsRange.Value2 = objValues;
        }

        private void ShowTotalMatches(
            int iX0,
            int iY0,
            int iCount1,
            int iCount2)
        {
            //Конец таблицы.
            int intX1 = iX0 + CViewConst.EXTRA_COLUMNS + 2 * (iCount1 + iCount2);
            int intY1 = iY0 + (iCount1 + iCount2) * CClientData.MatchesInPair;

            Excel.Range clsRange = CUtils.ColumnGet(this, intY1 + 1, intY1 + 3, intX1 + CViewConst.COLUMN_MATCHES_OFFSET);
            object[,] objValues = clsRange.Value2;

            Server.STotalsForClient clsTotals = ThisWorkbook.TotalsForClientGet();

            objValues[1, 1] = clsTotals.TotalMatches;
            objValues[2, 1] = clsTotals.MatchesPlayed;
            objValues[3, 1] = clsTotals.TotalMatches - clsTotals.MatchesPlayed;

            clsRange.Value2 = objValues;
        }

        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(Sheet1_Startup);
            this.Shutdown += new System.EventHandler(Sheet1_Shutdown);
        }

        #endregion
    }
}
