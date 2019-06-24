using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

namespace Vegaxys
{
    public class RoomListingMenu :MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform content;
        [SerializeField] private RoomObjectUI roomPrefab;
        private List<RoomObjectUI> objectUIs = new List<RoomObjectUI>();
        private PanelsManager panelsManager;

        public void FirstInitialize(PanelsManager panels) {
            panelsManager = panels;
        }

        public override void OnJoinedRoom() {
            panelsManager.CurrentRoom.Show();
            content.DestroyChildren();
            objectUIs.Clear();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList) {
            foreach (RoomInfo item in roomList) {
                //Remove this room
                if (item.RemovedFromList) {
                    int index = objectUIs.FindIndex(x => x.RoomInfo.Name == item.Name);
                    if(index != -1) {
                        Destroy(objectUIs[index].gameObject);
                        objectUIs.RemoveAt(index);
                    }
                }
                //Add this room
                else {
                    int index = objectUIs.FindIndex(x => x.RoomInfo.Name == item.Name);
                    if(index == -1) { 
                        RoomObjectUI objectUI = Instantiate(roomPrefab, content);
                        if (objectUI != null) {
                            objectUI.SetRoomInfo(item);
                            objectUIs.Add(objectUI);
                        }
                    } else {
                        //Modify listing
                    }
                }
            }
        }
    }
}
