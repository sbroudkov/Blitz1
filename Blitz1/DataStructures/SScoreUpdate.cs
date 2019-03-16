using System.Runtime.Serialization;

namespace Blitz1.Server
{
    [DataContract(IsReference = true)]
    public class SScoreUpdate
    {
        public int Participant1;
        public int Participant2;
        public int Round;
        public int? Score1;
        public int? Score2;

        public SScoreUpdate(
            int iParticipant1,
            int iParticipant2,
            int iRound,
            int? iScore1,
            int? iScore2)
        {
            Participant1 = iParticipant1;
            Participant2 = iParticipant2;
            Round = iRound;
            Score1 = iScore1;
            Score2 = iScore2;
        }
    }
}
