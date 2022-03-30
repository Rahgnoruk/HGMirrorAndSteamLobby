namespace HyperGnosys.MirrorIntegration
{
    public class CreateSteamLobbyInfo
    {
        public string lobbyName;
        public string lobbyPassword;
        public int maxPlayers;
        public string description;

        public CreateSteamLobbyInfo(string lobbyName, string lobbyPassword, int maxPlayers, string description)
        {
            this.lobbyName = lobbyName;
            this.lobbyPassword = lobbyPassword;
            this.maxPlayers = maxPlayers;
            this.description = description;
        }
    }
}