using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Blitz1.Server
{
    //public для обеспечения сериализации
    [DataContract(IsReference = true)]
    public class STeam
    {
        [DataMember]
        public string Name;
        [DataMember]
        public List<SParticipant> Participants;

        public STeam()
        {
            Participants = new List<SParticipant>();
        }
    }
}
