using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HyperGnosys.MirrorIntegration
{
    public class UISteamLobbyMaker : MonoBehaviour
    {
        [Header("Controller")]
        [SerializeField] private SteamLobbyMaker steamLobbyMaker;

        [Header("UI Elements")]
        [SerializeField] private TMP_InputField lobbyNameInput;
        [SerializeField] private TMP_InputField lobbyPasswordInput;
        [SerializeField] private TMP_InputField maxPlayersInput;
        [SerializeField] private TMP_InputField descriptionInput;
        [SerializeField] private List<Selectable> hostButtons = new List<Selectable>();

        private bool lobbyInfoComplete = false;
        private CreateSteamLobbyInfo lobbyInfo;
        private string lobbyName;
        private string lobbyPassword;
        private int maxPlayers;
        private string description;

        private void Awake()
        {
            if (maxPlayersInput != null)
            {
                maxPlayersInput.contentType = TMP_InputField.ContentType.IntegerNumber;
            }
            ValidateLobbyInfo();
        }
        public void HostPublic()
        {
            if (!lobbyInfoComplete)
            {
                return;
            }
            lobbyInfo = CreateLobbyInfo();
            steamLobbyMaker.HostPublic(lobbyInfo);
        }
        public void HostFriendsOnly()
        {
            if (!lobbyInfoComplete)
            {
                return;
            }
            lobbyInfo = CreateLobbyInfo();
            steamLobbyMaker.HostFriendsOnly(lobbyInfo);
        }
        public void SetLobbyName()
        {
            if (lobbyNameInput == null)
            {
                lobbyInfoComplete = false;
                return;
            }
            lobbyName = lobbyNameInput.text;
            ValidateLobbyInfo();
        }
        public void SetMaxPlayers()
        {
            if (maxPlayersInput == null)
            {
                lobbyInfoComplete = false;
                return;
            }
            int.TryParse(maxPlayersInput.text, out maxPlayers);
            if(maxPlayers < 2)
            {
                maxPlayersInput.text = "2";
                maxPlayers = 2;
            }
            ValidateLobbyInfo();
        }
        public void SetLobbyPassword()
        {
            if (lobbyPasswordInput == null)
            {
                lobbyInfoComplete = false;
                return;
            }
            if (string.IsNullOrEmpty(lobbyPasswordInput.text))
            {
                lobbyPassword = SteamLobbyMaker.NoPassword;
            }
            else
            {
                lobbyPassword = lobbyPasswordInput.text;
            }
            ValidateLobbyInfo();
        }
        public void SetLobbyDescription()
        {
            if (descriptionInput == null)
            {
                lobbyInfoComplete = false;
                return;
            }
            description = descriptionInput.text;
            ValidateLobbyInfo();
        }
        private void ValidateLobbyInfo()
        {
            bool hasName = !string.IsNullOrEmpty(lobbyName);
            bool hasDescription = !string.IsNullOrEmpty(description);
            lobbyInfoComplete = hasName && hasDescription;
            foreach(Selectable uiElement in hostButtons)
            {
                uiElement.enabled = lobbyInfoComplete;
            }
        }
        private CreateSteamLobbyInfo CreateLobbyInfo()
        {
            return new CreateSteamLobbyInfo(lobbyName, lobbyPassword, maxPlayers, description);
        }
    }
}