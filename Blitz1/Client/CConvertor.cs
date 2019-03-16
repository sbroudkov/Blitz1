using System;
using Blitz1.Client;
using Blitz1.Server;

namespace Blitz1
{
    internal static class CConvertor
    {
        public static ClientGameModeEnum Convert(ServerGameModeEnum iGameMode)
        {
            ClientGameModeEnum enmResult = ClientGameModeEnum.None;

            switch (iGameMode)
            {
                case ServerGameModeEnum.Round:
                    enmResult = ClientGameModeEnum.Round;
                    break;
                case ServerGameModeEnum.TeamByTeam:
                    enmResult = ClientGameModeEnum.TeamByTeam;
                    break;
                case ServerGameModeEnum.OneToOne:
                    enmResult = ClientGameModeEnum.OneToOne;
                    break;
                case ServerGameModeEnum.LeagueGroup:
                    enmResult = ClientGameModeEnum.LeagueGroup;
                    break;
                default:
                    throw new Exception("Указано нераспознанное значение нумератора ServerGameModeEnum");
            }

            return enmResult;
        }

        public static ServerGameModeEnum Convert(ClientGameModeEnum iGameMode)
        {
            ServerGameModeEnum enmResult = ServerGameModeEnum.None;

            switch (iGameMode)
            {
                case ClientGameModeEnum.Round:
                    enmResult = ServerGameModeEnum.Round;
                    break;
                case ClientGameModeEnum.TeamByTeam:
                    enmResult = ServerGameModeEnum.TeamByTeam;
                    break;
                case ClientGameModeEnum.OneToOne:
                    enmResult = ServerGameModeEnum.OneToOne;
                    break;
                case ClientGameModeEnum.LeagueGroup:
                    enmResult = ServerGameModeEnum.LeagueGroup;
                    break;
                default:
                    throw new Exception("Указано нераспознанное значение нумератора ClientGameModeEnum");
            }

            return enmResult;
        }
    }
}
