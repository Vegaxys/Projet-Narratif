using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vegaxys {
    public class CurrentRoomUI :MonoBehaviour
    {
        [SerializeField] private PlayerListingMenu playerListingMenu;
        public LeaveRoomMenu leaveRoomMenu;
        private PanelsManager panelsManager;

        public void FirstInitialize(PanelsManager panels) {
            panelsManager = panels;
            playerListingMenu.FirstInitialize(panels);
            leaveRoomMenu.FirstInitialize(panels);
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }
    }
}
