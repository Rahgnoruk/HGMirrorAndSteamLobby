using Steamworks;

namespace HyperGnosys.MirrorIntegration
{
    public class MatchIDSteamLobby: SteamLobby
    {
        public string MatchID { get; private set; }
        public MatchIDSteamLobby(CSteamID lobbySteamID, string hostSteamID, string hostUsername, 
            string lobbyName, string matchID, string password, int connectedPlayers, int maxPlayers, string description)
            : base(lobbySteamID, hostSteamID, hostUsername, lobbyName, password, connectedPlayers, maxPlayers, description)
        {
            MatchID = matchID;
        }
        public MatchIDSteamLobby(CSteamID lobbySteamID, string hostSteamID, 
            string hostUsername, string matchID, int connectedPlayers, CreateSteamLobbyInfo lobbyInfo)
            : base(lobbySteamID, hostSteamID, hostUsername, connectedPlayers, lobbyInfo)
        {
            MatchID = matchID;
        }
    }
}