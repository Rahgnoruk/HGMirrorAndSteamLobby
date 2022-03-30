using Mirror;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HyperGnosys.MirrorIntegration
{
    public class SteamLobbyJoiner : MonoBehaviour
    {
        [Header("Visual Feedback Event")]
        [SerializeField] private UnityEvent onLobbyJoinFailed = new UnityEvent();
        [Header("Resulting Lobby Info Event")]
        [SerializeField] private LobbyInfoScriptableEvent OnGotLobbyToJoin;

        protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
        protected Callback<LobbyEnter_t> lobbyEnter;
        protected Callback<LobbyMatchList_t> lobbyMatchList;
        private UnityEvent<List<SteamLobby>> onGotLobbyList = new UnityEvent<List<SteamLobby>>();
        private List<SteamLobby> lobbies = new List<SteamLobby>();
        private SteamLobby lobbyToJoin;
        private void Awake()
        {
            if (!SteamManager.Initialized)
            {
                //TBD Indica que no se a abierto Steam en la computadora
                return;
            }
            gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
            lobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
            lobbyMatchList = Callback<LobbyMatchList_t>.Create(OnLobbyMatchList);
        }
        public void GetLobbyList()
        {
            SteamMatchmaking.RequestLobbyList();
        }
        public void FindLobbyWithLobbyName(string lobbyNameSearchedFor)
        {
            SteamMatchmaking.AddRequestLobbyListStringFilter(SteamLobbyDataKeys.LobbyNameKey,
                lobbyNameSearchedFor, ELobbyComparison.k_ELobbyComparisonEqualToOrGreaterThan);
            SteamMatchmaking.RequestLobbyList();
        }
        public void FindLobbyWithHostUsername(string hostUsernameSearchedFor)
        {
            SteamMatchmaking.AddRequestLobbyListStringFilter(SteamLobbyDataKeys.HostSteamUsername,
                hostUsernameSearchedFor, ELobbyComparison.k_ELobbyComparisonEqualToOrGreaterThan);
            SteamMatchmaking.RequestLobbyList();
        }
        public void JoinLobby(SteamLobby lobby)
        {
            lobbyToJoin = lobby;
            SteamMatchmaking.JoinLobby(lobby.LobbySteamID);
        }
        //Se llama cuando le dan Join Game en Steam Overlay/Lista de amigos. Solo se usa para eso.
        private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
        {
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }
        //Es la respuesta a SteamMatchMaking.JoinLobby, pero tambien se llama en el host cuando se une a su propio Lobby
        private void OnLobbyEnter(LobbyEnter_t callback)
        {
            CSteamID lobbyID = new CSteamID(callback.m_ulSteamIDLobby);

            SteamLobby lobby = RetreiveLobbyData(lobbyID);
            if (NetworkServer.active)
            {
                //We are the host. No need to do this
                return;
            }
            LobbyInfo lobbyToJoin = new LobbyInfo(lobby.HostSteamID, lobby.HostUsername,lobby.LobbyName,
                lobby.ConnectedPlayers, lobby.MaxPlayers, lobby.Description);
            OnGotLobbyToJoin.Raise(lobbyToJoin);
        }
        private void OnLobbyMatchList(LobbyMatchList_t callback)
        {
            lobbies.Clear();
            for (int i = 0; i < callback.m_nLobbiesMatching; i++)
            {
                CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
                SteamLobby lobby = RetreiveLobbyData(lobbyID);
                lobbies.Add(lobby);
            }
            onGotLobbyList.Invoke(lobbies);
        }
        private SteamLobby RetreiveLobbyData(CSteamID lobbyID)
        {
            string hostSteamID = SteamMatchmaking.GetLobbyData(lobbyID, SteamLobbyDataKeys.HostSteamIDKey);
            string lobbyName = SteamMatchmaking.GetLobbyData(lobbyID, SteamLobbyDataKeys.LobbyNameKey);
            string hostUsername = SteamMatchmaking.GetLobbyData(lobbyID, SteamLobbyDataKeys.HostSteamUsername);
            string password = SteamMatchmaking.GetLobbyData(lobbyID, SteamLobbyDataKeys.PasswordKey);
            string connectedPlayersString = SteamMatchmaking.GetLobbyData(lobbyID, SteamLobbyDataKeys.ConnectedPlayersKey);
            int connectedPlayers = 0;
            int.TryParse(connectedPlayersString, out connectedPlayers);
            string maxPlayerString = SteamMatchmaking.GetLobbyData(lobbyID, SteamLobbyDataKeys.MaxPlayersKey);
            int maxPlayers = 0;
            int.TryParse(maxPlayerString, out maxPlayers);
            string description = SteamMatchmaking.GetLobbyData(lobbyID, SteamLobbyDataKeys.GameDescription);

            return new SteamLobby(lobbyID, hostSteamID, hostUsername, lobbyName,
                password, connectedPlayers, maxPlayers, description);
        }
        public UnityEvent OnLobbyJoinFailed { get => onLobbyJoinFailed; set => onLobbyJoinFailed = value; }
        public UnityEvent<List<SteamLobby>> OnGotLobbyList { get => onGotLobbyList; set => onGotLobbyList = value; }
    }
}