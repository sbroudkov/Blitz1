using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Blitz1.Server
{
    //public для обеспечения сериализации
    [DataContract (IsReference = true)]
    public class SGlobalState
    {
        [DataMember]
        public STournament Tournament;

        public SGlobalState()
        {
            Tournament = new STournament();
            SGroup clsGroup = new SGroup();
            Tournament.Groups.Add(clsGroup);
            STeam clsTeam = new STeam();
            clsGroup.Teams.Add(clsTeam);
            clsTeam = new STeam();
            clsGroup.Teams.Add(clsTeam);
        }
    }
}
