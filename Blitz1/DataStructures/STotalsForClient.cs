using System.Runtime.Serialization;

namespace Blitz1.Server
{
    [DataContract(IsReference = true)]
    public class STotalsForClient
    {
        [DataMember]
        public int Plus;
        [DataMember]
        public int Minus;
        [DataMember]
        public int Difference;
        [DataMember]
        public int Wins;
        [DataMember]
        public int Draws;
        [DataMember]
        public int Loses;
        [DataMember]
        public int Points1;
        [DataMember]
        public int Points2;
        [DataMember]
        public int MatchesPlayed;
        [DataMember]
        public int TotalMatches;
    }
}
