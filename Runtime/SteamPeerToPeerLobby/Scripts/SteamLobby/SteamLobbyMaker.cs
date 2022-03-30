using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace HyperGnosys.MirrorIntegration
{
    public class SteamLobbyMaker : MonoBehaviour
    {
        [Header("Visual Feedback Events")]
        [SerializeField] private UnityEvent onLobbyCreationFailed = new UnityEvent();
        [SerializeField] private UnityEvent onLobbyCreationSucceded = new UnityEvent();
        
        [Header("Resulting Lobby Info Event")]
        [SerializeField] private LobbyInfoScriptableEvent OnCreatedLobby;

        protected Callback<LobbyCreated_t> lobbyCreated;
        public const string NoPassword = "";
        private CreateSteamLobbyInfo lobbyInfo;
        public void Start()
        {
            if (!SteamManager.Initialized)
            {
                //TBD Indica que no se a abierto Steam en la computadora
                return;
            }
            lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        }

        public void HostPublic(CreateSteamLobbyInfo lobbyInfo)
        {
            if (!SteamManager.Initialized)
            {
                //TBD Indica que no se a abierto Steam en la computadora
                return;
            }
            this.lobbyInfo = lobbyInfo;
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, lobbyInfo.maxPlayers);
        }

        public void HostFriendsOnly(CreateSteamLobbyInfo lobbyInfo)
        {
            if (!SteamManager.Initialized)
            {
                //TBD Indica que no se a abierto Steam en la computadora
                return;
            }
            this.lobbyInfo = lobbyInfo;
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, lobbyInfo.maxPlayers);
        }
        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            if (!SteamManager.Initialized)
            {
                //TBD Indica que no se a abierto Steam en la computadora
                return;
            }
            if (callback.m_eResult != EResult.k_EResultOK)
            {
                onLobbyCreationFailed.Invoke();
                return;
            }
            onLobbyCreationSucceded.Invoke();
            CSteamID lobbyID = new CSteamID(callback.m_ulSteamIDLobby);
            string hostSteamID = SteamUser.GetSteamID().ToString();
            string hostSteamUsername = SteamFriends.GetPersonaName();
            SteamMatchmaking.SetLobbyData(lobbyID, SteamLobbyDataKeys.HostSteamIDKey, hostSteamID);
            SteamMatchmaking.SetLobbyData(lobbyID, SteamLobbyDataKeys.HostSteamUsername, hostSteamUsername);
            SteamMatchmaking.SetLobbyData(lobbyID, SteamLobbyDataKeys.LobbyNameKey, lobbyInfo.lobbyName);
            SteamMatchmaking.SetLobbyData(lobbyID, SteamLobbyDataKeys.PasswordKey, lobbyInfo.lobbyPassword);
            SteamMatchmaking.SetLobbyData(lobbyID, SteamLobbyDataKeys.ConnectedPlayersKey, "1");
            SteamMatchmaking.SetLobbyData(lobbyID, SteamLobbyDataKeys.MaxPlayersKey, lobbyInfo.maxPlayers.ToString());
            SteamMatchmaking.SetLobbyData(lobbyID, SteamLobbyDataKeys.GameDescription, lobbyInfo.description);
            LobbyInfo createdLobby = new LobbyInfo(hostSteamID, hostSteamUsername, lobbyInfo.lobbyName,
                1, lobbyInfo.maxPlayers, lobbyInfo.description);
            OnCreatedLobby.Raise(createdLobby);
        }
        public UnityEvent OnLobbyCreationFailed { get => onLobbyCreationFailed; set => onLobbyCreationFailed = value; }
        public UnityEvent OnLobbyCreationSucceded { get => onLobbyCreationSucceded; set => onLobbyCreationSucceded = value; }
    }
}