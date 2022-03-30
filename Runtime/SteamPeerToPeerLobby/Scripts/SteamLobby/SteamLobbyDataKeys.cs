namespace HyperGnosys.MirrorIntegration
{
    public static class SteamLobbyDataKeys
    {
        ///Host Steam ID to connect through Mirror's Steam Transport
        public const string HostSteamIDKey = "HostAddress";
        public const string HostSteamUsername = "HostUsername";
        public const string LobbyNameKey = "LobbyName";
        ///Needs a database that holds all the generated IDs to make sure 
        ///the new one is unique
        public const string PasswordKey = "WobaLobbaDubDub";
        public const string ConnectedPlayersKey = "ConnectedPlayers";
        public const string MaxPlayersKey = "MaxPlayers";
        public const string MatchIDKey = "MatchID";
        ///Favourites
        ///Easy Anti Cheat
        ///Game Settings Info
        public const string GameDescription = "Description";
        ///Map Info
        public const string MapNameKey = "MapName";
        public const string PingKey = "Ping";
    }
}