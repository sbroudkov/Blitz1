using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Blitz1.Server
{
    //public для обеспечения сериализации
    [DataContract (IsReference = true)]
    public class STournament
    {
        [DataMember]
        public string Name;
        [DataMember]
        public ServerGameModeEnum GameMode;
        [DataMember]
        public int PointsForWinning;
        [DataMember]
        public int PointsForDraw;
        [DataMember]
        public int MatchesInPair;
        [DataMember]
        public List<SGroup> Groups;

        public STournament()
        {
            Groups = new List<SGroup>();
        }
    }
}
