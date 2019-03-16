using System.Runtime.Serialization;

namespace Blitz1.Server
{
    //public для обеспечения сериализации
    [DataContract(IsReference = true)]
    public class SParticipant
    {
        [DataMember]
        public STeam Team;
        [DataMember]
        public int Position;
        [DataMember]
        public string Name;
        [DataMember]
        public SParticipantTotals Totals;

        public SParticipant()
        {
            Totals = new SParticipantTotals();
        }

        public SParticipant(
            string iName,
            int iPosition,
            STeam iTeam) : this()
        {
            Name = iName;
            Position = iPosition;
            Team = iTeam;
        }
    }
}
