using System.Runtime.Serialization;

namespace Blitz1.Server
{
    [DataContract(IsReference = true)]
    public class SParticipantTotalsForClient
    {
        [DataMember]
        public int TeamIndex;
        [DataMember]
        public int Place;
        [DataMember]
        public string Name;
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
        public int MatchesPlayed;
        [DataMember]
        public int Points;

        internal SParticipantTotalsForClient(int iTeamIndex)
        {
            this.TeamIndex = iTeamIndex;
        }

        internal SParticipantTotalsForClient(
            int iTeamIndex,
            SParticipant iParticipant) :
            this (iTeamIndex)
        {
            this.Name = iParticipant.Name;
            this.Plus = iParticipant.Totals.Plus;
            this.Minus = iParticipant.Totals.Minus;
            this.Difference = iParticipant.Totals.Difference;
            this.Wins = iParticipant.Totals.Wins;
            this.Draws = iParticipant.Totals.Draws;
            this.Loses = iParticipant.Totals.Loses;
            this.MatchesPlayed = iParticipant.Totals.MatchesPlayed;
            this.Points = iParticipant.Totals.Points;
            this.Place = iParticipant.Totals.Place;
        }
    }
}
