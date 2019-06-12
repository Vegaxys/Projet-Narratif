using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vegaxys
{
    public class LobbyRoom :MonoBehaviourPunCallbacks, IInRoomCallbacks
    {
        #region Public Fields

        public static LobbyRoom instance;

        public int playersInRoom;
        public int myIndexInRoom;
        public int playersInGame;
        public int gameSceneIndex;
        public int currentScene;
        public bool isGameLoaded;

        #endregion


        #region Private Fields

        private PhotonView photonV;
        Player[] photonPlayers;

        #endregion


        #region MonoBehaviour CallBacks

        private void Awake() {
            if (instance == null) {
                instance = this;
            } else {
                if (instance != null) {
                    Destroy(instance.gameObject);
                    instance = this;
                }
            }
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start() {
            photonV = GetComponent<PhotonView>();
        }

        public override void OnEnable() {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
            SceneManager.sceneLoaded += OnSceneFinishedLoading;
        }

        public override void OnDisable() {
            base.OnDisable();
            PhotonNetwork.RemoveCallbackTarget(this);
            SceneManager.sceneLoaded -= OnSceneFinishedLoading;
        }

        #endregion


        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnJoinedRoom() {
            base.OnJoinedRoom();
            print("Player is now in a room");
            photonPlayers = PhotonNetwork.PlayerList;
            playersInRoom = photonPlayers.Length;
            PhotonNetwork.NickName = myIndexInRoom.ToString();
            if (!PhotonNetwork.IsMasterClient) {
                return;
            }
            StartGame();
        }

        #endregion


        #region Private Methods

        private void StartGame() {
            print("Loading Level");
            PhotonNetwork.LoadLevel(gameSceneIndex);
        }

        private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode) {
            currentScene = scene.buildIndex;
            if (currentScene == gameSceneIndex) {
                CreatePlayer();
            }
        }

        private void CreatePlayer() {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefab", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
        }

        #endregion

    }
}
