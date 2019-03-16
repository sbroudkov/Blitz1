using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Blitz1.Server
{
    public enum ServerGameModeEnum
    {
        None = 0,
        Round = 1,
        TeamByTeam = 2,
        OneToOne = 3,
        LeagueGroup = 4,
    }

    internal static class CCommonData
    {
        private static SGlobalState mclsState = new SGlobalState();

        public static List<SParticipant> Participants1
        {
            get
            {
                return mclsState.Tournament.Groups[0].Teams[0].Participants;
            }
        }

        public static List<SParticipant> Participants2
        {
            get
            {
                return mclsState.Tournament.Groups[0].Teams[1].Participants;
            }
        }

        public static int PointsForWinning
        {
            get
            {
                return mclsState.Tournament.PointsForWinning;
            }
            set
            {
                mclsState.Tournament.PointsForWinning = value;
            }
        }

        public static int PointsForDraw
        {
            get
            {
                return mclsState.Tournament.PointsForDraw;
            }
            set
            {
                mclsState.Tournament.PointsForDraw = value;
            }
        }

        public static int MatchesInPair
        {
            get
            {
                int intResult = 1;
                if (mclsState.Tournament.GameMode == ServerGameModeEnum.OneToOne)
                {
                    intResult = mclsState.Tournament.MatchesInPair;
                }
                return intResult;
            }
        }

        public static int ActualMatchesInPair
        {
            get
            {
                return mclsState.Tournament.MatchesInPair;
            }
            set
            {
                mclsState.Tournament.MatchesInPair = value;
            }
        }

        public static int TotalMatches
        {
            get
            {
                int intResult = 0;

                int intCount1 = mclsState.Tournament.Groups[0].Teams[0].Participants.Count;
                int intCount2 = mclsState.Tournament.Groups[0].Teams[1].Participants.Count;
                switch (mclsState.Tournament.GameMode)
                {
                    case ServerGameModeEnum.Round:
                        intResult = intCount1 * (intCount1 - 1) / 2;
                        break;
                    case ServerGameModeEnum.TeamByTeam:
                        intResult = intCount1 * intCount2;
                        break;
                    case ServerGameModeEnum.OneToOne:
                        intResult = MatchesInPair * intCount1; // в этом режиме intCount1 должно быть равно intCount2;
                        break;
                    default:
                        throw new Exception("Нераспознанное значение нумератора ServerGameModeEnum.");
                }
                return intResult;
            }
        }

        public static ServerGameModeEnum GameMode
        {
            get
            {
                return mclsState.Tournament.GameMode;
            }
            set
            {
                mclsState.Tournament.GameMode = value;
            }
        }

        public static string TeamName1
        {
            get
            {
                return mclsState.Tournament.Groups[0].Teams[0].Name;
            }
            set
            {
                mclsState.Tournament.Groups[0].Teams[0].Name = value;
            }
        }

        public static string TeamName2
        {
            get
            {
                return mclsState.Tournament.Groups[0].Teams[1].Name;
            }
            set
            {
                mclsState.Tournament.Groups[0].Teams[1].Name = value;
            }
        }

        public static string TotalsStringGet(int iGroup)
        {
            string strResult = default(string);

            List<SParticipant> lstData = null;
            string strTeamName = null;
            if (iGroup == 0)
            {
                lstData = Participants1;
                strTeamName = TeamName1;
            }
            else
            {
                lstData = Participants2;
                strTeamName = TeamName2;
            }

            //[Индекс в списке, количество очков], сортировка по второму полю.
            List<Tuple<int, int>> lstSorted = CCommonData.SortedByPlaceIndicesGet(lstData);

            StringBuilder clsStringBuilder = new StringBuilder();

            if ((GameMode == ServerGameModeEnum.TeamByTeam) ||
                (GameMode == ServerGameModeEnum.OneToOne))
            {
                clsStringBuilder.Append(strTeamName);
                clsStringBuilder.AppendLine();
                clsStringBuilder.Append("———————————————————————————————————————");
                clsStringBuilder.AppendLine();
            }

            for (int i = 0; i < lstSorted.Count; i++)
            {
                SParticipant clsParticipant = lstData[lstSorted[i].Item1];

                string strPlace = EmodjiNumberGet(clsParticipant.Totals.Place);
                clsStringBuilder.Append(strPlace);
                clsStringBuilder.Append(" ");
                clsStringBuilder.Append(clsParticipant.Name);
                clsStringBuilder.Append(" ");
                clsStringBuilder.Append(clsParticipant.Totals.Plus);
                clsStringBuilder.Append(":");
                clsStringBuilder.Append(clsParticipant.Totals.Minus);
                clsStringBuilder.Append("=");
                clsStringBuilder.Append(clsParticipant.Totals.Difference);

                clsStringBuilder.Append("   ");
                clsStringBuilder.Append(clsParticipant.Totals.Wins);
                clsStringBuilder.Append("/");
                clsStringBuilder.Append(clsParticipant.Totals.Draws);
                clsStringBuilder.Append("/");
                clsStringBuilder.Append(clsParticipant.Totals.Loses);
                clsStringBuilder.Append("=");
                clsStringBuilder.Append(clsParticipant.Totals.Points);

                switch (clsParticipant.Totals.Place)
                {
                    case 1:
                        clsStringBuilder.Append(" 🏆");
                        break;
                    case 2:
                        clsStringBuilder.Append(" 🏅");
                        break;
                    case 3:
                        clsStringBuilder.Append(" 🎖");
                        break;
                }
                clsStringBuilder.AppendLine();
            }

            if ((GameMode == ServerGameModeEnum.TeamByTeam) ||
                (GameMode == ServerGameModeEnum.OneToOne))
            {
                clsStringBuilder.Append("———————————————————————————————————————");
                clsStringBuilder.AppendLine();
                clsStringBuilder.Append("Итого:");
                clsStringBuilder.Append("           ");
                clsStringBuilder.Append(lstData.Sum(x => x.Totals.Plus));
                clsStringBuilder.Append(":");
                clsStringBuilder.Append(lstData.Sum(x => x.Totals.Minus));
                clsStringBuilder.Append("=");
                clsStringBuilder.Append(lstData.Sum(x => x.Totals.Difference));

                clsStringBuilder.Append("   ");
                clsStringBuilder.Append(lstData.Sum(x => x.Totals.Wins));
                clsStringBuilder.Append("/");
                clsStringBuilder.Append(lstData.Sum(x => x.Totals.Draws));
                clsStringBuilder.Append("/");
                clsStringBuilder.Append(lstData.Sum(x => x.Totals.Loses));
                clsStringBuilder.Append("=");
                clsStringBuilder.Append(lstData.Sum(x => x.Totals.Points));
                clsStringBuilder.AppendLine();
            }

            strResult = clsStringBuilder.ToString();

            return strResult;
        }

        //////////public static void ScoreUpdate(List<SScoreUpdate> iUpdates)
        //////////{
        //////////    foreach (SScoreUpdate clsUpdate in iUpdates)
        //////////    {
        //////////        int intTeam2 = 0;
        //////////        if (mclsState.Tournament.GameMode != ServerGameModeEnum.Round)
        //////////        {
        //////////            intTeam2 = 1;
        //////////        }
        //////////        SParticipant clsParticipant1 = mclsState.Tournament.Groups[0].Teams[0].Participants[clsUpdate.Participant1];
        //////////        SParticipant clsParticipant2 = mclsState.Tournament.Groups[0].Teams[intTeam2].Participants[clsUpdate.Participant2];

        //////////        List<SMatch> lstMatches = mclsState.Tournament.Groups[0].Matches.
        //////////            Where(x =>
        //////////                (((x.First == clsParticipant1) &&
        //////////                (x.Second == clsParticipant2)) ||
        //////////                ((x.First == clsParticipant2) &&
        //////////                (x.Second == clsParticipant1))) &&
        //////////                (x.Round == clsUpdate.Round)).
        //////////            ToList();

        //////////        if (lstMatches.Count() > 1)
        //////////        {
        //////////            throw new Exception("Дублирование данных");
        //////////        }
        //////////        else
        //////////        {
        //////////            if (lstMatches.Count() == 1)
        //////////            {
        //////////                if (lstMatches[0].First == clsParticipant1)
        //////////                {
        //////////                    lstMatches[0].FirstPoints = clsUpdate.Score1;
        //////////                    lstMatches[0].SecondPoints = clsUpdate.Score2;
        //////////                }
        //////////                else
        //////////                {
        //////////                    lstMatches[0].FirstPoints = clsUpdate.Score2;
        //////////                    lstMatches[0].SecondPoints = clsUpdate.Score1;
        //////////                }
        //////////            }
        //////////            else
        //////////            {
        //////////                SMatch clsMatch = new SMatch(clsParticipant1, clsParticipant2, clsUpdate.Round, clsUpdate.Score1, clsUpdate.Score2);
        //////////                mclsState.Tournament.Groups[0].Matches.Add(clsMatch);
        //////////            }
        //////////        }
        //////////    }
        //////////}

        public static void ScoreUpdate(SScoreUpdate iUpdate)
        {
            int intTeam2 = 0;
            if (mclsState.Tournament.GameMode != ServerGameModeEnum.Round)
            {
                intTeam2 = 1;
            }
            SParticipant clsParticipant1 = mclsState.Tournament.Groups[0].Teams[0].Participants[iUpdate.Participant1];
            SParticipant clsParticipant2 = mclsState.Tournament.Groups[0].Teams[intTeam2].Participants[iUpdate.Participant2];

            List<SMatch> lstMatches = mclsState.Tournament.Groups[0].Matches.
                Where(x =>
                    (((x.First == clsParticipant1) &&
                    (x.Second == clsParticipant2)) ||
                    ((x.First == clsParticipant2) &&
                    (x.Second == clsParticipant1))) &&
                    (x.Round == iUpdate.Round)).
                ToList();

            if (lstMatches.Count() > 1)
            {
                throw new Exception("Дублирование данных");
            }
            else
            {
                if (lstMatches.Count() == 1)
                {
                    if (lstMatches[0].First == clsParticipant1)
                    {
                        lstMatches[0].FirstPoints = iUpdate.Score1;
                        lstMatches[0].SecondPoints = iUpdate.Score2;
                    }
                    else
                    {
                        lstMatches[0].FirstPoints = iUpdate.Score2;
                        lstMatches[0].SecondPoints = iUpdate.Score1;
                    }
                }
                else
                {
                    SMatch clsMatch = new SMatch(clsParticipant1, clsParticipant2, iUpdate.Round, iUpdate.Score1, iUpdate.Score2);
                    mclsState.Tournament.Groups[0].Matches.Add(clsMatch);
                }
            }
        }

        internal static STotalsForClient TotalsGet()
        {
            //
            STotalsForClient clsResult = default(STotalsForClient);

            clsResult = new STotalsForClient();
            clsResult.Plus = Participants1.Sum(x => x.Totals.Plus);
            clsResult.Minus = Participants1.Sum(x => x.Totals.Minus);
            clsResult.Difference = Participants1.Sum(x => x.Totals.Difference);
            clsResult.Wins = Participants1.Sum(x => x.Totals.Wins);
            clsResult.Draws = Participants1.Sum(x => x.Totals.Draws);
            clsResult.Loses = Participants1.Sum(x => x.Totals.Loses);
            clsResult.Points1 = Participants1.Sum(x => x.Totals.Points);
            clsResult.Points2 = Participants2.Sum(x => x.Totals.Points);
            clsResult.MatchesPlayed = Participants1.Sum(x => x.Totals.MatchesPlayed);

            if (mclsState.Tournament.GameMode == ServerGameModeEnum.Round)
            {
                clsResult.MatchesPlayed /= 2;
            }

            clsResult.TotalMatches = TotalMatches;

            return clsResult;
        }

        public static SParticipantTotalsForClient ParticipantTotalsGet(int iTeamIndex, int iParticipantIndex)
        {
            SParticipantTotalsForClient clsResult = default(SParticipantTotalsForClient);

            clsResult = new Server.SParticipantTotalsForClient(iTeamIndex);

            List<SParticipant> lstParticipants = mclsState.Tournament.Groups[0].Teams[iTeamIndex].Participants;
            SParticipant clsParticipant = lstParticipants[iParticipantIndex];

            clsResult.Name = clsParticipant.Name;

            List<SMatch> lstMatches = mclsState.Tournament.Groups[0].Matches.
                Where(x => 
                    x.FirstPoints.HasValue &&
                    x.SecondPoints.HasValue).
                ToList();

            lstParticipants.ForEach(x => x.Totals = new SParticipantTotals());

            foreach(SMatch clsMatch in lstMatches)
            {
                int intValue1 = 0;
                int intValue2 = 0;
                SParticipant clsParticipant1;
                SParticipant clsParticipant2;
                if (clsMatch.First.Team == mclsState.Tournament.Groups[0].Teams[0])
                {
                    intValue1 = clsMatch.FirstPoints.Value;
                    intValue2 = clsMatch.SecondPoints.Value;
                    clsParticipant1 = clsMatch.First;
                    clsParticipant2 = clsMatch.Second;
                }
                else
                {
                    intValue1 = clsMatch.SecondPoints.Value;
                    intValue2 = clsMatch.FirstPoints.Value;
                    clsParticipant1 = clsMatch.Second;
                    clsParticipant2 = clsMatch.First;
                }

                if (intValue1 > intValue2)
                {
                    clsParticipant1.Totals.Wins++;
                    clsParticipant2.Totals.Loses++;
                }
                else if (intValue1 < intValue2)
                {
                    clsParticipant1.Totals.Loses++;
                    clsParticipant2.Totals.Wins++;
                }
                else
                {
                    clsParticipant1.Totals.Draws++;
                    clsParticipant2.Totals.Draws++;
                }
                clsParticipant1.Totals.Plus += intValue1;
                clsParticipant1.Totals.Minus += intValue2;
                clsParticipant2.Totals.Plus += intValue2;
                clsParticipant2.Totals.Minus += intValue1;
            }

            clsResult.Plus = clsParticipant.Totals.Plus;
            clsResult.Minus = clsParticipant.Totals.Minus;
            clsResult.Difference = clsParticipant.Totals.Difference;

            clsResult.Wins = clsParticipant.Totals.Wins;
            clsResult.Loses = clsParticipant.Totals.Loses;
            clsResult.Draws = clsParticipant.Totals.Draws;

            clsResult.Points = clsParticipant.Totals.Points;

            clsResult.MatchesPlayed = clsParticipant.Totals.MatchesPlayed;

            //[Индекс в списке, количество очков], сортировка по второму полю.
            List<Tuple<int, int>> lstSorted = SortedByPlaceIndicesGet(lstParticipants);

            for (int i = 0; i < lstSorted.Count; i++)
            {
                SParticipant clsItem = lstParticipants[lstSorted[i].Item1];
                clsItem.Totals.Place = lstSorted[i].Item2;
            }

            clsResult.Place = clsParticipant.Totals.Place;

            return clsResult;
        }

        public static List<SParticipantTotalsForClient> DataForPlacesGet()
        {
            List<SParticipantTotalsForClient> lstResult = DataForPlacesGet(0, Participants1);

            if ((GameMode == ServerGameModeEnum.TeamByTeam) ||
                (GameMode == ServerGameModeEnum.OneToOne))
            {
                lstResult.AddRange(DataForPlacesGet(1, Participants2));
            }

            return lstResult;
        }

        private static List<SParticipantTotalsForClient> DataForPlacesGet(
            int iTeamIndex,
            List<SParticipant> iData)
        {
            List<SParticipantTotalsForClient> lstResult = new List<Server.SParticipantTotalsForClient>();

            //[Индекс в списке, количество очков], сортировка по второму полю.
            List<Tuple<int, int>> lstSorted = SortedByPlaceIndicesGet(iData);

            for (int i = 0; i < lstSorted.Count; i++)
            {
                SParticipant clsItem = iData[lstSorted[i].Item1];

                SParticipantTotalsForClient clsParticipant = new SParticipantTotalsForClient(iTeamIndex, clsItem);
                lstResult.Add(clsParticipant);
            }

            return lstResult;
        }

        internal static int PointsGet(SParticipantTotals iTotals)
        {
            return mclsState.Tournament.PointsForWinning * iTotals.Wins + mclsState.Tournament.PointsForDraw * iTotals.Draws;
        }

        public static void ParticipantAdd(
            string iName,
            int iTeamIndex)
        {
            STeam clsTeam = mclsState.Tournament.Groups[0].Teams[iTeamIndex];
            clsTeam.Participants.Add(new SParticipant(iName, clsTeam.Participants.Count, clsTeam));
        }

        public static string[] ParticipantNamesGet(List<SParticipant> iData)
        {
            return iData
                .OrderBy(x => x.Position)
                .Select(x => x.Name)
                .ToArray();
        }

        //Временные заглушки - пробросы вызовов в приватные методы.
        //В дальнейшем интерфейс взаимодействия с UI компонентом нужно изменить.
        public static List<Tuple<int, int>> ExternalSortedByPlaceIndicesGet(List<SParticipant> iData)
        {
            return SortedByPlaceIndicesGet(iData);
        }

        internal static string ParticipantNameUpdate(
            int iTeamIndex,
            int iParticipantIndex,
            string iName)
        {
            string strResult = default(string);

            mclsState.Tournament.Groups[0].Teams[iTeamIndex].Participants[iParticipantIndex].Name = iName;
            strResult = iName;

            return strResult;
        }

        private static List<Tuple<int, int>> SortedByPlaceIndicesGet(List<SParticipant> iData)
        {
            List<SParticipant> lstSorted = iData
                .OrderByDescending(x => x.Totals.Points)
                .ThenByDescending(x => x.Totals.Wins)
                .ThenByDescending(x => x.Totals.Plus)
                .ThenByDescending(x => x.Totals.Difference)
                .ToList();
            lstSorted.ForEach(x => x.Totals.Place = lstSorted.IndexOf(x) + 1);

            return iData
                .Select(x => new Tuple<int, int>(iData.IndexOf(x), x.Totals.Place))
                .OrderBy(x => x.Item2)
                .ToList();
        }

        public static string Serialize()
        {
            string strReturn = null;

            using (MemoryStream clsStream = new MemoryStream())
            {
                using (StreamReader clsReader = new StreamReader(clsStream, Encoding.UTF8))
                {
                    DataContractSerializer clsSerializer = new DataContractSerializer(mclsState.GetType());
                    using (XmlDictionaryWriter clsWriter = XmlDictionaryWriter.CreateTextWriter(clsStream, Encoding.UTF8))
                    {
                        clsSerializer.WriteObject(clsWriter, mclsState);
                        clsWriter.Flush();
                        clsStream.Position = 0;
                        strReturn = clsReader.ReadToEnd();
                    }
                }
            }
            return strReturn;
        }

        public static void Deserialize(string iSerialized)
        {
            try
            {
                DataContractSerializer clsSerializer = new DataContractSerializer(mclsState.GetType());

                using (MemoryStream clsStream = new MemoryStream())
                {
                    using (StreamWriter clsWriter = new StreamWriter(clsStream, Encoding.UTF8))
                    {
                        clsWriter.Write(iSerialized);
                        clsWriter.Flush();
                        clsStream.Position = 0;
                        using (XmlDictionaryReader clsReader = XmlDictionaryReader.CreateTextReader(clsStream, XmlDictionaryReaderQuotas.Max))
                        {
                            mclsState = (SGlobalState)clsSerializer.ReadObject(clsReader);
                        }
                    }
                }
            }
            catch(Exception clsException)
            {
                MessageBox.Show(
                    "Ошибка восстановления данных!" + Environment.NewLine + clsException.Message, 
                    "Ошибка", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        private static string EmodjiNumberGet(int iNumber)
        {
            string strResult = null;

            int intNumber = iNumber;
            if (intNumber < 0)
            {
                intNumber = -intNumber;
            }

            if (intNumber == 10)
            {
                strResult = "🔟";
            }
            else
            {
                strResult = intNumber.ToString();
                int intLength = strResult.Length;
                for (int i = 0; i < intLength; i++)
                {
                    strResult = strResult.Insert(2 * i + 1, '\u20E3'.ToString());
                }
            }

            if (iNumber < 0)
            {
                strResult = "➖" + strResult;
            }

            return strResult;
        }
    }
}
