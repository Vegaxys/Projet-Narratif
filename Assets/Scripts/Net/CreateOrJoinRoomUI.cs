using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegaxys {
    public class CreateOrJoinRoomUI :MonoBehaviour
    {
        [SerializeField] private CreateRoomMenu createRoomMenu;
        [SerializeField] private RoomListingMenu roomListingMenu;
        private PanelsManager panelsManager;

        public void FirstInitialize(PanelsManager panels) {
            panelsManager = panels;
            roomListingMenu.FirstInitialize(panels);
            createRoomMenu.FirstInitialize(panels);
        }
    }
}
