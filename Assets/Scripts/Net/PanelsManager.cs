using UnityEngine;

namespace Vegaxys {
    public class PanelsManager :MonoBehaviour
    {
        [SerializeField] private CreateOrJoinRoomUI createOrJoinRoom;
        public CreateOrJoinRoomUI CreateOrJoinRoom { get { return createOrJoinRoom; } }

        [SerializeField] private CurrentRoomUI currentRoom;
        public CurrentRoomUI CurrentRoom { get { return currentRoom; } }

        private void Awake() {
            FirstInitialize();
        }

        private void FirstInitialize() {
            CurrentRoom.FirstInitialize(this);
            CreateOrJoinRoom.FirstInitialize(this);
        }
    }
}
