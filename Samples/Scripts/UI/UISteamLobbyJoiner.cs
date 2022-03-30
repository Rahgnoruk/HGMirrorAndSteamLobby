using HyperGnosys.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HyperGnosys.MirrorIntegration
{
    public class UISteamLobbyJoiner : MonoBehaviour
    {
        [SerializeField] private bool debugging = false;
        [Header("Controller")]
        [SerializeField] private SteamLobbyJoiner steamLobbyJoiner;
        [Header("Search UI Elements")]
        [SerializeField] private TMP_InputField searchInput;
        [SerializeField] private TMP_Dropdown searchDropDown;
        [SerializeField] private Button searchButton;
        [Header("Lobby List UI Elements")]
        [SerializeField] private Transform lobbyListContainer;
        [SerializeField] private UILobbyListItem lobbyListItemPrefab;
        [Header("Lobby Info Panel UI Elements")]
        [SerializeField] private TMP_Text lobbyNameText;
        [SerializeField] private TMP_Text hostSteamUsernameText;
        [SerializeField] private TMP_Text descriptionText;
        private List<UILobbyListItem> lobbiesDisplayed = new List<UILobbyListItem>();
        private string searchText;
        private SteamLobby selectedLobby;
        private void Start()
        {
            if(steamLobbyJoiner == null)
            {
                return;
            }
            steamLobbyJoiner.OnGotLobbyList.AddListener(DisplayLobbyList);
            steamLobbyJoiner.GetLobbyList();
        }

        public void Search()
        {
            HGDebug.Log($"Searching with option {searchDropDown.value}", this, debugging);

            int displayedLobbiesAmount = lobbiesDisplayed.Count;
            for (int i = displayedLobbiesAmount - 1; i >= 0; i--)
            {
                UILobbyListItem poppedLobby = lobbiesDisplayed[i];
                lobbiesDisplayed.Remove(poppedLobby);
                Destroy(poppedLobby.gameObject);
            }
            if (searchDropDown.value == 0)
            {
                SearchByLobbyName();
            } 
            else if (searchDropDown.value == 1)
            {
                SearchByHostUsername();
            }
        }
        public void DisplayLobbyList(List<SteamLobby> lobbyList)
        {
            foreach(SteamLobby lobby in lobbyList)
            {
                UILobbyListItem lobbyInstance = Instantiate(lobbyListItemPrefab, lobbyListContainer);
                lobbiesDisplayed.Add(lobbyInstance);
                Button lobbyInstanceButton = lobbyInstance.GetComponent<Button>();
                if(lobbyInstanceButton != null)
                {
                    lobbyInstanceButton.onClick.AddListener(() => {
                         DisplayLobby(lobby);
                     });
                }
                lobbyInstance.SetLobbyListItem(lobby);
            }
        }
        public void DisplayLobby(SteamLobby lobby)
        {
            if(lobbyNameText == null || hostSteamUsernameText == null || descriptionText == null)
            {
                return;
            }
            selectedLobby = lobby;
            lobbyNameText.text = lobby.LobbyName;
            hostSteamUsernameText.text = lobby.HostUsername;
            descriptionText.text = lobby.Description;
        }
        public void JoinLobby()
        {
            if (selectedLobby == null)
            {
                return;
            }
            steamLobbyJoiner.JoinLobby(selectedLobby);
        }
        public void SetSearchText(string text)
        {
            searchText = text;
        }
        private void SearchByLobbyName()
        {
            if (steamLobbyJoiner == null)
            {
                return;
            }
            steamLobbyJoiner.FindLobbyWithLobbyName(searchText);
        }
        private void SearchByHostUsername()
        {
            if (steamLobbyJoiner == null)
            {
                return;
            }
            steamLobbyJoiner.FindLobbyWithHostUsername(searchText);
        }
    }
}