using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Blitz1.Server
{
    //public для обеспечения сериализации
    [DataContract(IsReference = true)]
    public class SGroup
    {
        [DataMember]
        public string GroupName;
        [DataMember]
        public List<STeam> Teams;
        [DataMember]
        public List<SMatch> Matches;

        public SGroup()
        {
            Teams = new List<STeam>();
            Matches = new List<SMatch>();
        }
    }
}
