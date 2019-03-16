using System.Runtime.Serialization;

namespace Blitz1.Server
{
    //public для обеспечения сериализации
    [DataContract(IsReference = true)]
    public class SMatch
    {
        [DataMember]
        public int Round;
        [DataMember]
        public SParticipant First;
        [DataMember]
        public SParticipant Second;
        [DataMember]
        public int? FirstPoints;
        [DataMember]
        public int? SecondPoints;

        public SMatch()
        {

        }

        public SMatch(
            SParticipant iFirst,
            SParticipant iSecond,
            int iRound,
            int? iFirstPoints,
            int? iSecondPoints)
        {
            First = iFirst;
            Second = iSecond;
            Round = iRound;
            FirstPoints = iFirstPoints;
            SecondPoints = iSecondPoints;
        }
    }
}
