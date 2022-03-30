using Steamworks;

namespace HyperGnosys.MirrorIntegration
{
    public class SteamLobby
    {
        public CSteamID LobbySteamID { get; protected set; }
        public string HostSteamID { get; protected set; }
        public string HostUsername { get; protected set; }
        public string LobbyName { get; protected set; }
        public string Password { get; protected set; }
        public int ConnectedPlayers { get; protected set; }
        public int MaxPlayers { get; protected set; }
        public string Description { get; protected set; }
        public SteamLobby(CSteamID lobbySteamID, string hostSteamID, string hostUsername,
            string lobbyName, string password, int connectedPlayers, int maxPlayers, string description)
        {
            LobbySteamID = lobbySteamID;
            HostSteamID = hostSteamID;
            HostUsername = hostUsername;
            LobbyName = lobbyName;
            Password = password;
            ConnectedPlayers = connectedPlayers;
            MaxPlayers = maxPlayers;
            Description = description;
        }
        public SteamLobby(CSteamID lobbySteamID, string hostSteamID, string hostUsername, int connectedPlayers,
            CreateSteamLobbyInfo lobbyInfo)
        {
            LobbySteamID = lobbySteamID;
            HostSteamID = hostSteamID;
            HostUsername = hostUsername;
            ConnectedPlayers = connectedPlayers;
            LobbyName = lobbyInfo.lobbyName;
            Password = lobbyInfo.lobbyPassword;
            MaxPlayers = lobbyInfo.maxPlayers;
            Description = lobbyInfo.description;
        }
    }
}