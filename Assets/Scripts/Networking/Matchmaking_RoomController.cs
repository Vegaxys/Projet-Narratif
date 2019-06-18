using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using TMPro;

namespace Vegaxys
{
    public class Matchmaking_RoomController :MonoBehaviourPunCallbacks
    {
        [SerializeField] private int multiplayerSceneIndex;   //Join a lobby

        [SerializeField] private GameObject lobbyPanel;             //Panel for lobby
        [SerializeField] private GameObject roomPanel;              //main panel
        [SerializeField] private GameObject listplayerPanel;        //players panel

        [SerializeField] private Button startButton;      //Prefab of a room for the UI

        [SerializeField] private Transform playerContainer;           //panel that contains rooms
        [SerializeField] private GameObject playerPrefab;           //panel that contains rooms
        [SerializeField] private TextMeshProUGUI roomNameText;           //panel that contains rooms

        //Destroy tout les player dans le transform container
        private void ClearPlayerListing() {
            for (int i = playerContainer.childCount - 1; i >= 0; i--) {
                Destroy(playerContainer.GetChild(i).gameObject);
            }
        }

        //Instantier les joueur dans la salle en question
        private void ListPlayer() {
            foreach (Player player in PhotonNetwork.PlayerList) {
                GameObject playerInstance = Instantiate(playerPrefab, playerContainer);
                TextMeshProUGUI _text = playerInstance.GetComponentInChildren<TextMeshProUGUI>();
                _text.text = player.NickName;
            }
        }

        public override void OnJoinedRoom() {
            roomPanel.SetActive(true);
            listplayerPanel.SetActive(true);
            lobbyPanel.SetActive(false);

            roomNameText.text = PhotonNetwork.CurrentRoom.Name;
            startButton.interactable = PhotonNetwork.IsMasterClient;

            ClearPlayerListing();
            ListPlayer();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer) {
            ClearPlayerListing();
            ListPlayer();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer) {
            ClearPlayerListing();
            ListPlayer();
            startButton.interactable = PhotonNetwork.IsMasterClient;
        }

        public void StartButton() {
            if (PhotonNetwork.IsMasterClient) {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.LoadLevel(multiplayerSceneIndex);
            }
        }

        private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode) {
            int currentScene = 0;
            currentScene = scene.buildIndex;
            if (currentScene == multiplayerSceneIndex) {
                CreatePlayer();
            }
        }

        private void CreatePlayer() {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
        }


        private IEnumerator RejoinLobby() {
            yield return new WaitForSeconds(1);
            PhotonNetwork.JoinLobby();
        }

        public void BackOnClick() {
            roomPanel.SetActive(false);
            listplayerPanel.SetActive(false);
            lobbyPanel.SetActive(true);

            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LeaveLobby();
            StartCoroutine(RejoinLobby());
        }
        public void SetCharacterID(int ID) {
            PlayerInfos.instance.characterID = ID;
        }
    }
}
