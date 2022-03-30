using System.Text;
using TMPro;
using UnityEngine;

namespace HyperGnosys.MirrorIntegration
{
    public class UILobbyListItem : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TMP_Text lobbyNameText;
        [SerializeField] private TMP_Text hostUsernameText;
        [SerializeField] private TMP_Text connectedAndMaxPlayersText;
        public void SetLobbyListItem(SteamLobby lobby)
        {
            lobbyNameText.text = lobby.LobbyName;
            hostUsernameText.text = lobby.HostUsername;
            connectedAndMaxPlayersText.text = BuildConnectedAndMaxPlayers(lobby.ConnectedPlayers, lobby.MaxPlayers);
        }
        private string BuildConnectedAndMaxPlayers(int connectedPlayers, int maxPlayers)
        {
            StringBuilder connectedAndMaxPlayers = new StringBuilder();
            connectedAndMaxPlayers.Append(connectedPlayers);
            connectedAndMaxPlayers.Append("/");
            connectedAndMaxPlayers.Append(maxPlayers);
            return connectedAndMaxPlayers.ToString();
        }
    }
}