using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Microsoft.Office.Tools.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace Blitz1.Client
{
    internal static class CUtils
    {
        public static void SheetReset(WorksheetBase iSheet)
        {
            iSheet.Unprotect();
            foreach (Excel.Shape clsShape in iSheet.Shapes)
            {
                clsShape.Delete();
            }

            iSheet.Cells.Clear();
            iSheet.Cells.ClearFormats();

            iSheet.Columns.ColumnWidth = iSheet.StandardWidth;
            iSheet.Columns.RowHeight = iSheet.StandardHeight;
            iSheet.Activate();
            iSheet.Cells[1, 1].Select();
        }

        public static Excel.Range RectGet(
            WorksheetBase iSheet,
            int iy0, 
            int ix0, 
            int iy1, 
            int ix1)
        {
            return iSheet.Range[iSheet.Cells[iy0, ix0], iSheet.Cells[iy1, ix1]];
        }

        public static Excel.Range ColumnGet(
            WorksheetBase iSheet,
            int iy0, 
            int iy1,
            int ix0)
        {
            return RectGet(iSheet, iy0, ix0, iy1, ix0);
        }

        public static Excel.Range RowGet(
            WorksheetBase iSheet,
            int iy0,
            int ix0,
            int ix1)
        {
            return RectGet(iSheet, iy0, ix0, iy0, ix1);
        }

        public static void BoxSet(
            Excel.Range iRange,
            Excel.XlBorderWeight iWeight)
        {
            BorderSet(
                iRange,
                new List<Excel.XlBordersIndex> { 
                    Excel.XlBordersIndex.xlEdgeLeft, 
                    Excel.XlBordersIndex.xlEdgeTop, 
                    Excel.XlBordersIndex.xlEdgeRight, 
                    Excel.XlBordersIndex.xlEdgeBottom },
                new List<Excel.XlLineStyle> {
                    Excel.XlLineStyle.xlContinuous,
                    Excel.XlLineStyle.xlContinuous,
                    Excel.XlLineStyle.xlContinuous,
                    Excel.XlLineStyle.xlContinuous
                },
                new List<Excel.XlBorderWeight> { 
                    iWeight,
                    iWeight,
                    iWeight,
                    iWeight});
        }

        public static void BorderSet(
            Excel.Range iRange,
            List<Excel.XlBordersIndex> iBorders,
            List<Excel.XlLineStyle> iStyles,
            List<Excel.XlBorderWeight> iWeights)
        {
            for (int i = 0; i < iBorders.Count; i++)
            {
                iRange.Borders[iBorders[i]].LineStyle = iStyles[i];
                iRange.Borders[iBorders[i]].Weight = iWeights[i];
            }
        }

        public static void InternalProtect(WorksheetBase iSheet)
        {
            iSheet.Protect(allowFormattingCells: true, allowFormattingColumns: true, allowFormattingRows: true);
        }

        public static void UpdateExecute(
            WorksheetBase iSheet,
            System.Action iAction)
        {
            bool blnProtection = iSheet.ProtectContents;
            iSheet.Application.ScreenUpdating = false;

            if (blnProtection)
            {
                iSheet.Unprotect();
            }
            try
            {
                CultureInfo clsCulture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                iAction.Invoke();
                Thread.CurrentThread.CurrentCulture = clsCulture;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (blnProtection)
                {
                    InternalProtect(iSheet);
                }
                iSheet.Application.ScreenUpdating = true;
            }
        }

    }
}
