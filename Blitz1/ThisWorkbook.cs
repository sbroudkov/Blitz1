using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Blitz1.Server;
using Excel = Microsoft.Office.Interop.Excel;

namespace Blitz1.Client
{
    public partial class ThisWorkbook
    {
        private const int DATA_CHUNK_SIZE = 32000;

        private void ThisWorkbook_Startup(object sender, System.EventArgs e)
        {
            bool blnResult = StateRestore();
            if (!blnResult)
            {
                FControl clsForm = new FControl();
                DialogResult enmResult = clsForm.ShowDialog();
                if (enmResult == DialogResult.OK)
                {
                    ToServerConvert();
                    CClientData.MatchesInPair = Server.CCommonData.MatchesInPair;

                    Globals.WSPlaces.GroupsCreate();
                    Globals.WSTable.Activate();
                    Globals.WSTable.GroupCreate(0, 0);
                    StateSave();
                }
            }
            Globals.WSTable.Init();
        }

        private void ToServerConvert()
        {
            Server.CCommonData.GameMode = CConvertor.Convert(CClientData.GameMode);
            Server.CCommonData.PointsForWinning = CClientData.PointsForWinning;
            Server.CCommonData.PointsForDraw = CClientData.PointsForDraw;
            Server.CCommonData.ActualMatchesInPair = CClientData.MatchesInPair;

            Server.CCommonData.TeamName1 = CClientData.TeamName1;
            Server.CCommonData.TeamName2 = CClientData.TeamName2;

            foreach (string strItem in CClientData.Participants1)
            {
                Server.CCommonData.ParticipantAdd(strItem, 0);
            }

            foreach (string strItem in CClientData.Participants2)
            {
                Server.CCommonData.ParticipantAdd(strItem, 1);
            }
        }

        private void StateSave()
        {
            Excel.Worksheet clsDataSheet = Sheets.OfType<Excel.Worksheet>().FirstOrDefault(ws => ws.Name == "Data");
            if (clsDataSheet == null)
            {
                clsDataSheet = Sheets.Add() as Excel.Worksheet;
                clsDataSheet.Name = "Data";
                clsDataSheet.Visible = Excel.XlSheetVisibility.xlSheetVeryHidden;
            }

            string strSerialized = Server.CCommonData.Serialize();
            Excel.Range clsCell = clsDataSheet.Cells[1, 1];
            int intPos = 0;
            while (intPos < strSerialized.Length)
            {
                clsCell.NumberFormat = "@";
                clsCell.Value2 = strSerialized.Substring(intPos, Math.Min(DATA_CHUNK_SIZE, strSerialized.Length - intPos));
                intPos += DATA_CHUNK_SIZE;
                clsCell = clsCell.Offset[1, 0];
            }
        }

        private bool StateRestore()
        {
            bool blnResult = false;

            Excel.Worksheet clsDataSheet = this.Sheets.OfType<Excel.Worksheet>().FirstOrDefault(ws => ws.Name == "Data");
            if (clsDataSheet != null)
            {
                StringBuilder clsBuilder = new StringBuilder();
                Excel.Range clsCell = clsDataSheet.Cells[1, 1];
                while (clsCell.Value2 != null)
                {
                    string strValue = clsCell.Value2.ToString();
                    clsBuilder.Append(strValue);
                    clsCell = clsCell.Offset[1, 0];
                }
                string strSerialized = clsBuilder.ToString();
                Server.CCommonData.Deserialize(strSerialized);
                blnResult = true;
            }
            return blnResult;
        }

        internal static string TotalsStringGet(int iGroup)
        {
            string strResult = default(string);

            strResult = Server.CCommonData.TotalsStringGet(iGroup);

            return strResult;
        }

        internal static List<Server.SParticipantTotalsForClient> DataForPlacesGet()
        {
            List<Server.SParticipantTotalsForClient> lstResult = default(List<Server.SParticipantTotalsForClient>);

            lstResult = Server.CCommonData.DataForPlacesGet();

            return lstResult;
        }

        internal static Server.SParticipantTotalsForClient ParticipantTotalsGet(
            int iTeamIndex, 
            int iParticipantIndex)
        {
            Server.SParticipantTotalsForClient clsResult = default(Server.SParticipantTotalsForClient);

            clsResult = Server.CCommonData.ParticipantTotalsGet(iTeamIndex, iParticipantIndex);

            return clsResult;
        }

        internal static Server.STotalsForClient TotalsForClientGet()
        {
            Server.STotalsForClient clsResult = default(Server.STotalsForClient);

            clsResult = Server.CCommonData.TotalsGet();

            return clsResult;
        }

        //////////internal static void ScoreUpdate(List<SScoreUpdate> iUpdates)
        //////////{
        //////////    Server.CCommonData.ScoreUpdate(iUpdates);
        //////////}

        internal static void ScoreUpdate(SScoreUpdate iUpdate)
        {
            Server.CCommonData.ScoreUpdate(iUpdate);
        }

        internal static string ParticipantNameUpdate(
            int iTeamIndex, 
            int iParticipantIndex,
            string iName)
        {
            string strResult = default(string);

            strResult = Server.CCommonData.ParticipantNameUpdate(iTeamIndex, iParticipantIndex, iName);

            return strResult;
        }

        private void ThisWorkbook_BeforeSave(
            bool iSaveAsUI, 
            ref bool rCancel)
        {
            StateSave();
        }

        private void ThisWorkbook_Shutdown(
            object iSender, 
            EventArgs iArgs)
        {
        }

        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.BeforeSave += new Microsoft.Office.Interop.Excel.WorkbookEvents_BeforeSaveEventHandler(this.ThisWorkbook_BeforeSave);
            this.Startup += new System.EventHandler(this.ThisWorkbook_Startup);
            this.Shutdown += new System.EventHandler(this.ThisWorkbook_Shutdown);

        }

        #endregion

    }
}
