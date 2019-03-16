using System.Collections.Generic;

namespace Blitz1.Client
{
    public enum ClientGameModeEnum
    {
        None = 0,
        Round = 1,
        TeamByTeam = 2,
        OneToOne = 3,
        LeagueGroup = 4,
    }

    internal static class CClientData
    {
        public static ClientGameModeEnum GameMode;
        public static int PointsForWinning;
        public static int PointsForDraw;
        public static int MatchesInPair;
        public static string TeamName1;
        public static string TeamName2;
        public static List<string> Participants1;
        public static List<string> Participants2;

        static CClientData()
        {
            GameMode = ClientGameModeEnum.Round;
            PointsForWinning = 2;
            PointsForDraw = 1;
            MatchesInPair = 2;
            Participants1 = new List<string>();
            Participants2 = new List<string>();
        }
    }
}
