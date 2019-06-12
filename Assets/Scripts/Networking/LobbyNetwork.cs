using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
namespace Vegaxys
{
    public class LobbyNetwork :MonoBehaviourPunCallbacks
    {
        #region Public Fields

        public string sceneName;
        public static LobbyNetwork instance;

        #endregion


        #region Private Serializable Fields

        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField] private byte maxPlayersPerRoom = 4;
        [SerializeField] private Button createRoomButton;
        [SerializeField] private Button cancelButton;

        #endregion


        #region Private Fields


        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        private string gameVersion = "1";
        /// <summary>
        /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
        /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        /// </summary>
        private bool isConnecting;

        #endregion


        #region MonoBehaviour CallBacks

        void Awake() {
            instance = this;
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        void Start() {
            PhotonNetwork.ConnectUsingSettings();
            createRoomButton.interactable = false;
            cancelButton.interactable = false;
        }

        #endregion


        #region Public Methods

        public void JoinRoom() {
            createRoomButton.interactable = false;
            cancelButton.interactable = true;
            PhotonNetwork.JoinRandomRoom();
        }

        public void OnCancelButtonClick() {
            createRoomButton.interactable = true;
            cancelButton.interactable = false;
            PhotonNetwork.LeaveRoom();
            print("Room leaved");
        }

        #endregion


        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster() {
            Debug.Log("Player is connected to Photon");
            PhotonNetwork.AutomaticallySyncScene = true;
            createRoomButton.interactable = true;

        }

        public override void OnDisconnected(DisconnectCause cause) {
            Debug.LogWarningFormat("Vegaxys/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message) {
            Debug.Log("No random room available, so we create one.");
            CreateRoom();
        }

        public override void OnCreateRoomFailed(short returnCode, string message) {
            Debug.Log("This room already exist.");
            CreateRoom();
        }

        #endregion


        #region Private Methods

        private void CreateRoom() {
            int randomRoomName = Random.Range(0, 10000);
            RoomOptions roomOpt = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom("Room#" + randomRoomName, roomOpt);
        }

        #endregion
    }
}
