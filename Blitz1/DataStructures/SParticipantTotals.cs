using System.Runtime.Serialization;

namespace Blitz1.Server
{
    //public для обеспечения сериализации
    [DataContract(IsReference = true)]
    public class SParticipantTotals
    {
        [DataMember]
        public int Plus;
        [DataMember]
        public int Minus;
        [DataMember]
        public int Wins;
        [DataMember]
        public int Draws;
        [DataMember]
        public int Loses;
        [DataMember]
        public int Place;

        public int Difference { get { return Plus - Minus; } }
        public int MatchesPlayed { get { return Wins + Draws + Loses; } }
        public int Points { get { return CCommonData.PointsGet(this); } }
    }
}
